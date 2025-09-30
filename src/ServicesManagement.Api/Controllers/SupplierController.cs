using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicesManagement.Api.Data;
using ServicesManagement.Api.Dtos;
using ServicesManagement.Api.Models;

namespace ServicesManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SupplierController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SupplierController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<GetSupplierDto>>> GetAll()
    {
        var result = await _context.Suppliers.ToListAsync();
        var resultDto = _mapper.Map<IEnumerable<GetSupplierDto>>(result);
        return Ok(resultDto);
    }

    [HttpGet("getById/{id:int}")]
    public async Task<ActionResult<GetSupplierDto>> GetById(int id)
    {
        var result = await _context.Suppliers
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return NotFound();
        }

        var resultDto = _mapper.Map<GetSupplierDto>(result);
        return Ok(resultDto);
    }

    [HttpPost("add")]
    public async Task<ActionResult<Supplier>> Add(AddSupplierDto supplierDto)
    {
        var emailExist = await _context.Suppliers.AnyAsync(s => s.Email == supplierDto.Email);
        if (emailExist)
        {
            return Conflict
            (
                new
                {
                    field = "email",
                    msg = "Ya existe un proveedor con este correo"
                }
            );    
        }
        
        var nameExist = await _context.Suppliers.AnyAsync(s => s.Name == supplierDto.Name);
        if (nameExist)
        {
            return Conflict
            (
                new
                {
                    field = "name",
                    msg = "Ya existe un proveedor con este nombre"
                }
            );    
        }

        var newSupplier = _mapper.Map<Supplier>(supplierDto);
        
        _context.Suppliers.Add(newSupplier);
        await _context.SaveChangesAsync();
        
        var supplierResponseDto = _mapper.Map<GetSupplierDto>(newSupplier);

        return CreatedAtAction
        (
            nameof(Add),
            new { id = newSupplier.Id },
            supplierResponseDto
        );
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateSupplierDto updatedSupplierDto)
    {
        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier == null)
        { 
            return NotFound();   
        }

        if (updatedSupplierDto.Email != null && updatedSupplierDto.Email != supplier.Email)
        {
            var emailExist = await _context.Suppliers.AnyAsync(s => s.Email == updatedSupplierDto.Email && s.Id != id);
            if (emailExist)
            {
                return Conflict
                (
                    new
                    {
                        field = "email",
                        msg = "Ya existe un proveedor con este correo"
                    }
                );    
            }
        }
        
        if (updatedSupplierDto.Name != null && updatedSupplierDto.Name != supplier.Name)
        {
            var nameExist = await _context.Suppliers.AnyAsync(s => s.Name == updatedSupplierDto.Name && s.Id != id);
            if (nameExist)
            {
                return Conflict
                (
                    new
                    {
                        field = "name",
                        msg = "Ya existe un proveedor con este nombre"
                    }
                );    
            }
        }
        
        if (updatedSupplierDto.Name != null) 
        {
            supplier.Name = updatedSupplierDto.Name;
        }
        if (updatedSupplierDto.Description != null) 
        {
            supplier.Description = updatedSupplierDto.Description;
        }
        if (updatedSupplierDto.Phone != null) 
        {
            supplier.Phone = updatedSupplierDto.Phone;
        }
        if (updatedSupplierDto.Email != null) 
        {
            supplier.Email = updatedSupplierDto.Email;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier == null)
        {
            return NotFound();
        }

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
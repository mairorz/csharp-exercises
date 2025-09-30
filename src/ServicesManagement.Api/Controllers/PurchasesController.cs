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
public class PurchasesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PurchasesController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<GetPurchasesDto>>> GetAll()
    {
        var result = await _context.Purchases
            .Include(p => p.Supplier)
            .Include(p => p.Input)
                .ThenInclude(i => i.Category)
            .ToListAsync();
            
        var resultDto = _mapper.Map<IEnumerable<GetPurchasesDto>>(result);
        return Ok(resultDto);
    }

    [HttpGet("getById/{id:int}")]
    public async Task<ActionResult<GetPurchasesDto>> GetById(int id)
    {
        var result = await _context.Purchases
            .Include(p => p.Supplier)
            .Include(p => p.Input)
                .ThenInclude(i => i.Category)
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return NotFound();
        }

        var resultDto = _mapper.Map<GetPurchasesDto>(result);
        return Ok(resultDto);
    }

    [HttpPost("add")]
    public async Task<ActionResult<Purchases>> Add(AddPurchasesDto purchaseDto)
    {
        var supplierExist = await _context.Suppliers.AnyAsync(s => s.Id == purchaseDto.SupplierId);
        if (!supplierExist)
        {
            return NotFound
            (
                new
                {
                    field = "supplierId",
                    msg = "El proveedor asociado no existe"
                }
            );
        }
        
        var inputExist = await _context.Inputs.AnyAsync(i => i.Id == purchaseDto.InputId);
        if (!inputExist)
        {
            return NotFound
            (
                new
                {
                    field = "inputId",
                    msg = "El insumo asociado no existe"
                }
            );
        }

        var newPurchase = _mapper.Map<Purchases>(purchaseDto);
        newPurchase.PurchasedDate = DateTime.UtcNow;
        
        _context.Purchases.Add(newPurchase);
        await _context.SaveChangesAsync();
        
        var purchaseResponse = await _context.Purchases
            .Include(p => p.Supplier)
            .Include(p => p.Input)
            .Where(p => p.Id == newPurchase.Id)
            .FirstOrDefaultAsync();

        var purchaseResponseDto = _mapper.Map<GetPurchasesDto>(purchaseResponse);

        return CreatedAtAction
        (
            nameof(Add),
            new { id = newPurchase.Id },
            purchaseResponseDto
        );
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, UpdatePurchasesDto updatedPurchaseDto)
    {
        var purchase = await _context.Purchases.FindAsync(id);

        if (purchase == null)
        { 
            return NotFound();   
        }
        
        if (updatedPurchaseDto.Name != null) 
        {
            purchase.Name = updatedPurchaseDto.Name;
        }
        if (updatedPurchaseDto.Description != null) 
        {
            purchase.Description = updatedPurchaseDto.Description;
        }
        if (updatedPurchaseDto.Quantity.HasValue) 
        {
            purchase.Quantity = updatedPurchaseDto.Quantity.Value;
        }
        if (updatedPurchaseDto.UnitCost.HasValue) 
        {
            purchase.UnitCost = updatedPurchaseDto.UnitCost.Value;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var purchase = await _context.Purchases.FindAsync(id);

        if (purchase == null)
        {
            return NotFound();
        }

        _context.Purchases.Remove(purchase);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
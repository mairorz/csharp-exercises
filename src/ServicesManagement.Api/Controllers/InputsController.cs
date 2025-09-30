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
public class InputsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public InputsController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<GetInputsDto>>> GetAll()
    {
        var result = await _context.Inputs.Include(i => i.Category).ToListAsync();
        var resultDto = _mapper.Map<IEnumerable<GetInputsDto>>(result);
        return Ok(resultDto);
    }
    
    [HttpGet("getAll/byCategory/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<GetInputsDto>>> GetAllByCategory(int categoryId)
    {
        var categoryExist = await _context.InputsCategories.AnyAsync(ic => ic.Id == categoryId);
        if (!categoryExist)
        {
            return NotFound("La categoría de insumos especificada no existe.");
        }

        var result = await _context.Inputs
            .Include(i => i.Category)
            .Where(i => i.CategoryId == categoryId)
            .ToListAsync();
            
        var resultDto = _mapper.Map<IEnumerable<GetInputsDto>>(result);
        return Ok(resultDto);
    }

    [HttpGet("getById/{id:int}")]
    public async Task<ActionResult<GetInputsDto>> GetById(int id)
    {
        var result = await _context.Inputs
            .Include(i => i.Category)
            .Where(i => i.Id == id)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return NotFound();
        }

        var resultDto = _mapper.Map<GetInputsDto>(result);
        return Ok(resultDto);
    }

    [HttpPost("add")]
    public async Task<ActionResult<Inputs>> Add(AddInputsDto inputDto)
    {
        var categoryExist = await _context.InputsCategories.AnyAsync(ic => ic.Id == inputDto.CategoryId);
        if (!categoryExist)
        {
            return NotFound
            (
                new
                {
                    field = "categoryId",
                    msg = "La categoría de insumos especificada no existe"
                }
            );
        }

        var nameExist = await _context.Inputs.AnyAsync(i => i.Name == inputDto.Name && i.CategoryId == inputDto.CategoryId);
        if (nameExist)
        {
            return Conflict
            (
                new
                {
                    field = "name",
                    msg = "Ya existe un insumo con este nombre en esta categoría"
                }
            );
        }

        var newInput = _mapper.Map<Inputs>(inputDto);
        _context.Inputs.Add(newInput);
        await _context.SaveChangesAsync();
        
        var inputResponse = await _context.Inputs
            .Include(i => i.Category)
            .Where(i => i.Id == newInput.Id)
            .FirstOrDefaultAsync();

        var inputResponseDto = _mapper.Map<GetInputsDto>(inputResponse);

        return CreatedAtAction
        (
            nameof(Add),
            new { id = newInput.Id },
            inputResponseDto
        );
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateInputsDto updatedInputDto)
    {
        var input = await _context.Inputs.FindAsync(id);

        if (input == null)
        { 
            return NotFound();   
        }

        if (updatedInputDto.CategoryId.HasValue)
        {
            var categoryExist = await _context.InputsCategories.AnyAsync(ic => ic.Id == updatedInputDto.CategoryId.Value);
            if (!categoryExist)
            {
                return NotFound
                (
                    new
                    {
                        field = "categoryId",
                        msg = "La categoría de insumos especificada no existe"
                    }
                );
            }
        }
        
        if (updatedInputDto.Name != null && updatedInputDto.Name != input.Name)
        {
            var categoryId = updatedInputDto.CategoryId ?? input.CategoryId;
            var nameExist = await _context.Inputs.AnyAsync(i => i.Name == updatedInputDto.Name && i.CategoryId == categoryId && i.Id != id);
            if (nameExist)
            {
                return Conflict
                (
                    new
                    {
                        field = "name",
                        msg = "Ya existe un insumo con este nombre en esta categoría"
                    }
                );
            }
        }
        
        if (updatedInputDto.CategoryId.HasValue) 
        {
            input.CategoryId = updatedInputDto.CategoryId.Value;
        }
        if (updatedInputDto.Name != null) 
        {
            input.Name = updatedInputDto.Name;
        }
        if (updatedInputDto.Unit != null) 
        {
            input.Unit = updatedInputDto.Unit;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var input = await _context.Inputs.FindAsync(id);

        if (input == null)
        {
            return NotFound();
        }

        _context.Inputs.Remove(input);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
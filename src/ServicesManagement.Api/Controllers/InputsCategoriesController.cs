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
public class InputsCategoriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public InputsCategoriesController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<GetInputsCategoriesDto>>> GetAll()
    {
        var result = await _context.InputsCategories.ToListAsync();
        var resultDto = _mapper.Map<IEnumerable<GetInputsCategoriesDto>>(result);
        return Ok(resultDto);
    }

    [HttpGet("getById/{id:int}")]
    public async Task<ActionResult<GetInputsCategoriesDto>> GetById(int id)
    {
        var result = await _context.InputsCategories
            .Where(ic => ic.Id == id)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return NotFound();
        }

        var resultDto = _mapper.Map<GetInputsCategoriesDto>(result);
        return Ok(resultDto);
    }

    [HttpPost("add")]
    public async Task<ActionResult<InputsCategories>> Add(AddInputsCategoriesDto categoryDto)
    {
        var nameExist = await _context.InputsCategories.AnyAsync(ic => ic.Name == categoryDto.Name);
        if (nameExist)
        {
            return Conflict
            (
                new
                {
                    field = "name",
                    msg = "Ya existe una categoría con este nombre"
                }
            );
        }

        var newCategory = _mapper.Map<InputsCategories>(categoryDto);
        _context.InputsCategories.Add(newCategory);
        await _context.SaveChangesAsync();

        var categoryResponseDto = _mapper.Map<GetInputsCategoriesDto>(newCategory);

        return CreatedAtAction
        (
            nameof(Add),
            new { id = newCategory.Id },
            categoryResponseDto
        );
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateInputsCategoriesDto updatedCategoryDto)
    {
        var category = await _context.InputsCategories.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        if (updatedCategoryDto.Name != category.Name)
        {
            var nameExist = await _context.InputsCategories.AnyAsync(ic => ic.Name == updatedCategoryDto.Name && ic.Id != id);
            if (nameExist)
            {
                return Conflict
                (
                    new
                    {
                        field = "name",
                        msg = "Ya existe una categoría con este nombre"
                    }
                );
            }
        }

        category.Name = updatedCategoryDto.Name;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.InputsCategories.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        _context.InputsCategories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
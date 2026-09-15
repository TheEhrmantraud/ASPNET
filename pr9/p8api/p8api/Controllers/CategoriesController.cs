using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using p8api.DTOs;
using p8api.Mappers;
using p8api.Models;

namespace p8api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        private readonly ShopSystemContext _context;

        public CategoriesController(ShopSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CategoryDTO>> GetAll(int page = 1, int pageSize = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1 || pageSize > 100)
            {
                pageSize = 10;
            }

            var items = _context.Categories
                .AsNoTracking()
                .OrderBy(x => x.IdCategory)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(CategoryMapper.ToDto)
                .ToList();

            return Ok(items);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CategoryDTO> GetById(int id)
        {
            var category = _context.Categories
                .AsNoTracking()
                .FirstOrDefault(x => x.IdCategory == id);

            if (category is null)
            {
                return NotFound();
            }

            return Ok(CategoryMapper.ToDto(category));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<CategoryDTO> Create(CreateCategoryDTO dto)
        {
            if (dto.ParentCategoryId.HasValue && !_context.Categories.Any(x => x.IdCategory == dto.ParentCategoryId.Value))
            {
                return BadRequest($"Родительская категория с идентификатором {dto.ParentCategoryId} не существует.");
            }

            var category = CategoryMapper.ToEntity(dto);
            _context.Categories.Add(category);

            var error = TrySaveChanges();
            if (error is not null)
            {
                return error;
            }

            return CreatedAtAction(nameof(GetById), new { id = category.IdCategory }, CategoryMapper.ToDto(category));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Update(int id, UpdateCategoryDTO dto)
        {
            var category = _context.Categories.FirstOrDefault(x => x.IdCategory == id);

            if (category is null)
            {
                return NotFound();
            }

            if (dto.ParentCategoryId.HasValue && !_context.Categories.Any(x => x.IdCategory == dto.ParentCategoryId.Value))
            {
                return BadRequest($"Родительская категория с идентификатором {dto.ParentCategoryId} не существует.");
            }

            CategoryMapper.ApplyUpdate(category, dto);

            var error = TrySaveChanges();
            if (error is not null)
            {
                return error;
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Patch(int id, PatchCategoryDTO dto)
        {
            var category = _context.Categories.FirstOrDefault(x => x.IdCategory == id);

            if (category is null)
            {
                return NotFound();
            }

            if (dto.ParentCategoryId.HasValue && !_context.Categories.Any(x => x.IdCategory == dto.ParentCategoryId.Value))
            {
                return BadRequest($"Родительская категория с идентификатором {dto.ParentCategoryId} не существует.");
            }

            CategoryMapper.ApplyPatch(category, dto);

            var error = TrySaveChanges();
            if (error is not null)
            {
                return error;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.IdCategory == id);

            if (category is null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);

            var error = TrySaveChanges();
            if (error is not null)
            {
                return error;
            }

            return NoContent();
        }

        private ActionResult? TrySaveChanges()
        {
            try
            {
                _context.SaveChanges();
                return null;
            }
            catch (DbUpdateException)
            {
                return Conflict("Операция нарушает ограничения целостности данных.");
            }
        }
    }
}

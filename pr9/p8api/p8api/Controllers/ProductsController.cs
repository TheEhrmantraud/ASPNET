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
    public class ProductsController : ControllerBase
    {
        private readonly ShopSystemContext _context;

        public ProductsController(ShopSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ProductDTO>> GetAll(int page = 1, int pageSize = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1 || pageSize > 100)
            {
                pageSize = 10;
            }

            var items = _context.Products
                .AsNoTracking()
                .OrderBy(x => x.IdProduct)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ProductMapper.ToDto)
                .ToList();

            return Ok(items);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductDTO> GetById(int id)
        {
            var product = _context.Products
                .AsNoTracking()
                .FirstOrDefault(x => x.IdProduct == id);

            if (product is null)
            {
                return NotFound();
            }

            return Ok(ProductMapper.ToDto(product));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<ProductDTO> Create(CreateProductDTO dto)
        {
            if (!_context.Categories.Any(x => x.IdCategory == dto.CategoryId))
            {
                return BadRequest($"Категория с идентификатором {dto.CategoryId} не существует.");
            }

            var product = ProductMapper.ToEntity(dto);
            _context.Products.Add(product);

            var error = TrySaveChanges();
            if (error is not null)
            {
                return error;
            }

            return CreatedAtAction(nameof(GetById), new { id = product.IdProduct }, ProductMapper.ToDto(product));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Update(int id, UpdateProductDTO dto)
        {
            var product = _context.Products.FirstOrDefault(x => x.IdProduct == id);

            if (product is null)
            {
                return NotFound();
            }

            if (!_context.Categories.Any(x => x.IdCategory == dto.CategoryId))
            {
                return BadRequest($"Категория с идентификатором {dto.CategoryId} не существует.");
            }

            ProductMapper.ApplyUpdate(product, dto);

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
        public IActionResult Patch(int id, PatchProductDTO dto)
        {
            var product = _context.Products.FirstOrDefault(x => x.IdProduct == id);

            if (product is null)
            {
                return NotFound();
            }

            if (dto.CategoryId.HasValue && !_context.Categories.Any(x => x.IdCategory == dto.CategoryId.Value))
            {
                return BadRequest($"Категория с идентификатором {dto.CategoryId} не существует.");
            }

            ProductMapper.ApplyPatch(product, dto);

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
            var product = _context.Products.FirstOrDefault(x => x.IdProduct == id);

            if (product is null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);

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

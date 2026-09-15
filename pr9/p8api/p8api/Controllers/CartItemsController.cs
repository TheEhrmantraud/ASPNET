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
    public class CartItemsController : ControllerBase
    {
        private readonly ShopSystemContext _context;

        public CartItemsController(ShopSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CartItemDTO>> GetAll(int page = 1, int pageSize = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1 || pageSize > 100)
            {
                pageSize = 10;
            }

            var items = _context.CartItems
                .AsNoTracking()
                .OrderBy(x => x.IdCartItem)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(CartItemMapper.ToDto)
                .ToList();

            return Ok(items);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CartItemDTO> GetById(int id)
        {
            var cartItem = _context.CartItems
                .AsNoTracking()
                .FirstOrDefault(x => x.IdCartItem == id);

            if (cartItem is null)
            {
                return NotFound();
            }

            return Ok(CartItemMapper.ToDto(cartItem));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<CartItemDTO> Create(CreateCartItemDTO dto)
        {
            if (!_context.Carts.Any(x => x.IdCart == dto.CartId))
            {
                return BadRequest($"Корзина с идентификатором {dto.CartId} не существует.");
            }

            if (!_context.Products.Any(x => x.IdProduct == dto.ProductId))
            {
                return BadRequest($"Товар с идентификатором {dto.ProductId} не существует.");
            }

            var cartItem = CartItemMapper.ToEntity(dto);
            _context.CartItems.Add(cartItem);

            var error = TrySaveChanges();
            if (error is not null)
            {
                return error;
            }

            return CreatedAtAction(nameof(GetById), new { id = cartItem.IdCartItem }, CartItemMapper.ToDto(cartItem));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Update(int id, UpdateCartItemDTO dto)
        {
            var cartItem = _context.CartItems.FirstOrDefault(x => x.IdCartItem == id);

            if (cartItem is null)
            {
                return NotFound();
            }

            if (!_context.Carts.Any(x => x.IdCart == dto.CartId))
            {
                return BadRequest($"Корзина с идентификатором {dto.CartId} не существует.");
            }

            if (!_context.Products.Any(x => x.IdProduct == dto.ProductId))
            {
                return BadRequest($"Товар с идентификатором {dto.ProductId} не существует.");
            }

            CartItemMapper.ApplyUpdate(cartItem, dto);

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
        public IActionResult Patch(int id, PatchCartItemDTO dto)
        {
            var cartItem = _context.CartItems.FirstOrDefault(x => x.IdCartItem == id);

            if (cartItem is null)
            {
                return NotFound();
            }

            if (dto.CartId.HasValue && !_context.Carts.Any(x => x.IdCart == dto.CartId.Value))
            {
                return BadRequest($"Корзина с идентификатором {dto.CartId} не существует.");
            }

            if (dto.ProductId.HasValue && !_context.Products.Any(x => x.IdProduct == dto.ProductId.Value))
            {
                return BadRequest($"Товар с идентификатором {dto.ProductId} не существует.");
            }

            CartItemMapper.ApplyPatch(cartItem, dto);

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
            var cartItem = _context.CartItems.FirstOrDefault(x => x.IdCartItem == id);

            if (cartItem is null)
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItem);

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

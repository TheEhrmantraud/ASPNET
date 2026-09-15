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
    public class CartsController : ControllerBase
    {
        private readonly ShopSystemContext _context;

        public CartsController(ShopSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CartDTO>> GetAll(int page = 1, int pageSize = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1 || pageSize > 100)
            {
                pageSize = 10;
            }

            var items = _context.Carts
                .AsNoTracking()
                .OrderBy(x => x.IdCart)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(CartMapper.ToDto)
                .ToList();

            return Ok(items);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CartDTO> GetById(int id)
        {
            var cart = _context.Carts
                .AsNoTracking()
                .FirstOrDefault(x => x.IdCart == id);

            if (cart is null)
            {
                return NotFound();
            }

            return Ok(CartMapper.ToDto(cart));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<CartDTO> Create(CreateCartDTO dto)
        {
            if (!_context.Users.Any(x => x.IdUser == dto.UserId))
            {
                return BadRequest($"Пользователь с идентификатором {dto.UserId} не существует.");
            }

            var cart = CartMapper.ToEntity(dto);
            _context.Carts.Add(cart);

            var error = TrySaveChanges();
            if (error is not null)
            {
                return error;
            }

            return CreatedAtAction(nameof(GetById), new { id = cart.IdCart }, CartMapper.ToDto(cart));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Update(int id, UpdateCartDTO dto)
        {
            var cart = _context.Carts.FirstOrDefault(x => x.IdCart == id);

            if (cart is null)
            {
                return NotFound();
            }

            if (!_context.Users.Any(x => x.IdUser == dto.UserId))
            {
                return BadRequest($"Пользователь с идентификатором {dto.UserId} не существует.");
            }

            CartMapper.ApplyUpdate(cart, dto);

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
        public IActionResult Patch(int id, PatchCartDTO dto)
        {
            var cart = _context.Carts.FirstOrDefault(x => x.IdCart == id);

            if (cart is null)
            {
                return NotFound();
            }

            if (dto.UserId.HasValue && !_context.Users.Any(x => x.IdUser == dto.UserId.Value))
            {
                return BadRequest($"Пользователь с идентификатором {dto.UserId} не существует.");
            }

            CartMapper.ApplyPatch(cart, dto);

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
            var cart = _context.Carts.FirstOrDefault(x => x.IdCart == id);

            if (cart is null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);

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

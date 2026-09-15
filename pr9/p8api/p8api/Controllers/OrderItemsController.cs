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
    public class OrderItemsController : ControllerBase
    {
        private readonly ShopSystemContext _context;

        public OrderItemsController(ShopSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<OrderItemDTO>> GetAll(int page = 1, int pageSize = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1 || pageSize > 100)
            {
                pageSize = 10;
            }

            var items = _context.OrderItems
                .AsNoTracking()
                .OrderBy(x => x.IdOrderItem)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(OrderItemMapper.ToDto)
                .ToList();

            return Ok(items);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<OrderItemDTO> GetById(int id)
        {
            var orderItem = _context.OrderItems
                .AsNoTracking()
                .FirstOrDefault(x => x.IdOrderItem == id);

            if (orderItem is null)
            {
                return NotFound();
            }

            return Ok(OrderItemMapper.ToDto(orderItem));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<OrderItemDTO> Create(CreateOrderItemDTO dto)
        {
            if (!_context.Orders.Any(x => x.IdOrder == dto.OrderId))
            {
                return BadRequest($"Заказ с идентификатором {dto.OrderId} не существует.");
            }

            if (!_context.Products.Any(x => x.IdProduct == dto.ProductId))
            {
                return BadRequest($"Товар с идентификатором {dto.ProductId} не существует.");
            }

            var orderItem = OrderItemMapper.ToEntity(dto);
            _context.OrderItems.Add(orderItem);

            var error = TrySaveChanges();
            if (error is not null)
            {
                return error;
            }

            return CreatedAtAction(nameof(GetById), new { id = orderItem.IdOrderItem }, OrderItemMapper.ToDto(orderItem));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Update(int id, UpdateOrderItemDTO dto)
        {
            var orderItem = _context.OrderItems.FirstOrDefault(x => x.IdOrderItem == id);

            if (orderItem is null)
            {
                return NotFound();
            }

            if (!_context.Orders.Any(x => x.IdOrder == dto.OrderId))
            {
                return BadRequest($"Заказ с идентификатором {dto.OrderId} не существует.");
            }

            if (!_context.Products.Any(x => x.IdProduct == dto.ProductId))
            {
                return BadRequest($"Товар с идентификатором {dto.ProductId} не существует.");
            }

            OrderItemMapper.ApplyUpdate(orderItem, dto);

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
        public IActionResult Patch(int id, PatchOrderItemDTO dto)
        {
            var orderItem = _context.OrderItems.FirstOrDefault(x => x.IdOrderItem == id);

            if (orderItem is null)
            {
                return NotFound();
            }

            if (dto.OrderId.HasValue && !_context.Orders.Any(x => x.IdOrder == dto.OrderId.Value))
            {
                return BadRequest($"Заказ с идентификатором {dto.OrderId} не существует.");
            }

            if (dto.ProductId.HasValue && !_context.Products.Any(x => x.IdProduct == dto.ProductId.Value))
            {
                return BadRequest($"Товар с идентификатором {dto.ProductId} не существует.");
            }

            OrderItemMapper.ApplyPatch(orderItem, dto);

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
            var orderItem = _context.OrderItems.FirstOrDefault(x => x.IdOrderItem == id);

            if (orderItem is null)
            {
                return NotFound();
            }

            _context.OrderItems.Remove(orderItem);

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

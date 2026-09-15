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
    public class OrdersController : ControllerBase
    {
        private readonly ShopSystemContext _context;

        public OrdersController(ShopSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<OrderDTO>> GetAll(int page = 1, int pageSize = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1 || pageSize > 100)
            {
                pageSize = 10;
            }

            var items = _context.Orders
                .AsNoTracking()
                .OrderBy(x => x.IdOrder)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(OrderMapper.ToDto)
                .ToList();

            return Ok(items);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<OrderDTO> GetById(int id)
        {
            var order = _context.Orders
                .AsNoTracking()
                .FirstOrDefault(x => x.IdOrder == id);

            if (order is null)
            {
                return NotFound();
            }

            return Ok(OrderMapper.ToDto(order));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<OrderDTO> Create(CreateOrderDTO dto)
        {
            if (!_context.Users.Any(x => x.IdUser == dto.UserId))
            {
                return BadRequest($"Пользователь с идентификатором {dto.UserId} не существует.");
            }

            if (!_context.OrderStatuses.Any(x => x.IdOrderStatus == dto.OrderStatusId))
            {
                return BadRequest($"Статус заказа с идентификатором {dto.OrderStatusId} не существует.");
            }

            var order = OrderMapper.ToEntity(dto);
            _context.Orders.Add(order);

            var error = TrySaveChanges();
            if (error is not null)
            {
                return error;
            }

            return CreatedAtAction(nameof(GetById), new { id = order.IdOrder }, OrderMapper.ToDto(order));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Update(int id, UpdateOrderDTO dto)
        {
            var order = _context.Orders.FirstOrDefault(x => x.IdOrder == id);

            if (order is null)
            {
                return NotFound();
            }

            if (!_context.Users.Any(x => x.IdUser == dto.UserId))
            {
                return BadRequest($"Пользователь с идентификатором {dto.UserId} не существует.");
            }

            if (!_context.OrderStatuses.Any(x => x.IdOrderStatus == dto.OrderStatusId))
            {
                return BadRequest($"Статус заказа с идентификатором {dto.OrderStatusId} не существует.");
            }

            OrderMapper.ApplyUpdate(order, dto);

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
        public IActionResult Patch(int id, PatchOrderDTO dto)
        {
            var order = _context.Orders.FirstOrDefault(x => x.IdOrder == id);

            if (order is null)
            {
                return NotFound();
            }

            if (dto.UserId.HasValue && !_context.Users.Any(x => x.IdUser == dto.UserId.Value))
            {
                return BadRequest($"Пользователь с идентификатором {dto.UserId} не существует.");
            }

            if (dto.OrderStatusId.HasValue && !_context.OrderStatuses.Any(x => x.IdOrderStatus == dto.OrderStatusId.Value))
            {
                return BadRequest($"Статус заказа с идентификатором {dto.OrderStatusId} не существует.");
            }

            OrderMapper.ApplyPatch(order, dto);

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
            var order = _context.Orders.FirstOrDefault(x => x.IdOrder == id);

            if (order is null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);

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

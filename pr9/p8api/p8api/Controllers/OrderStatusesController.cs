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
    public class OrderStatusesController : ControllerBase
    {
        private readonly ShopSystemContext _context;

        public OrderStatusesController(ShopSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<OrderStatusDTO>> GetAll(int page = 1, int pageSize = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1 || pageSize > 100)
            {
                pageSize = 10;
            }

            var items = _context.OrderStatuses
                .AsNoTracking()
                .OrderBy(x => x.IdOrderStatus)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(OrderStatusMapper.ToDto)
                .ToList();

            return Ok(items);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<OrderStatusDTO> GetById(int id)
        {
            var orderStatus = _context.OrderStatuses
                .AsNoTracking()
                .FirstOrDefault(x => x.IdOrderStatus == id);

            if (orderStatus is null)
            {
                return NotFound();
            }

            return Ok(OrderStatusMapper.ToDto(orderStatus));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<OrderStatusDTO> Create(CreateOrderStatusDTO dto)
        {
            var orderStatus = OrderStatusMapper.ToEntity(dto);
            _context.OrderStatuses.Add(orderStatus);

            var error = TrySaveChanges();
            if (error is not null)
            {
                return error;
            }

            return CreatedAtAction(nameof(GetById), new { id = orderStatus.IdOrderStatus }, OrderStatusMapper.ToDto(orderStatus));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Update(int id, UpdateOrderStatusDTO dto)
        {
            var orderStatus = _context.OrderStatuses.FirstOrDefault(x => x.IdOrderStatus == id);

            if (orderStatus is null)
            {
                return NotFound();
            }

            OrderStatusMapper.ApplyUpdate(orderStatus, dto);

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
        public IActionResult Patch(int id, PatchOrderStatusDTO dto)
        {
            var orderStatus = _context.OrderStatuses.FirstOrDefault(x => x.IdOrderStatus == id);

            if (orderStatus is null)
            {
                return NotFound();
            }

            OrderStatusMapper.ApplyPatch(orderStatus, dto);

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
            var orderStatus = _context.OrderStatuses.FirstOrDefault(x => x.IdOrderStatus == id);

            if (orderStatus is null)
            {
                return NotFound();
            }

            _context.OrderStatuses.Remove(orderStatus);

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

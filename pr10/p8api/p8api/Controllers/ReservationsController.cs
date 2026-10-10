using Microsoft.AspNetCore.Mvc;
using p8api.DTOs;
using p8api.Services;

namespace p8api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationsController(IReservationService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ReservationDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ReservationDTO>>> GetAll(int page = 1, int pageSize = 10, CancellationToken ct = default)
        {
            var result = await _service.GetPageAsync(page, pageSize, ct);
            Response.Headers["X-Total-Count"] = result.TotalCount.ToString();
            return Ok(result.Items);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ReservationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReservationDTO>> GetById(int id, CancellationToken ct)
        {
            var item = await _service.GetByIdAsync(id, ct);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ReservationDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ReservationDTO>> Reserve(CreateReservationDTO dto, CancellationToken ct)
        {
            var reservation = await _service.ReserveAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = reservation.IdReservation }, reservation);
        }

        [HttpPost("{id:int}/cancel")]
        [ProducesResponseType(typeof(ReservationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReservationDTO>> Cancel(int id, CancellationToken ct)
        {
            var reservation = await _service.CancelAsync(id, ct);
            return reservation is null
                ? NotFound(new { error = $"Активная бронь с Id {id} не найдена." })
                : Ok(reservation);
        }
    }
}

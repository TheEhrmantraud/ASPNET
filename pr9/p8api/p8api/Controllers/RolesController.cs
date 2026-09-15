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
    public class RolesController : ControllerBase
    {
        private readonly ShopSystemContext _context;

        public RolesController(ShopSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<RoleDTO>> GetAll(int page = 1, int pageSize = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1 || pageSize > 100)
            {
                pageSize = 10;
            }

            var items = _context.Roles
                .AsNoTracking()
                .OrderBy(x => x.IdRole)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(RoleMapper.ToDto)
                .ToList();

            return Ok(items);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<RoleDTO> GetById(int id)
        {
            var role = _context.Roles
                .AsNoTracking()
                .FirstOrDefault(x => x.IdRole == id);

            if (role is null)
            {
                return NotFound();
            }

            return Ok(RoleMapper.ToDto(role));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<RoleDTO> Create(CreateRoleDTO dto)
        {
            var role = RoleMapper.ToEntity(dto);
            _context.Roles.Add(role);

            var error = TrySaveChanges();
            if (error is not null)
            {
                return error;
            }

            return CreatedAtAction(nameof(GetById), new { id = role.IdRole }, RoleMapper.ToDto(role));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Update(int id, UpdateRoleDTO dto)
        {
            var role = _context.Roles.FirstOrDefault(x => x.IdRole == id);

            if (role is null)
            {
                return NotFound();
            }

            RoleMapper.ApplyUpdate(role, dto);

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
        public IActionResult Patch(int id, PatchRoleDTO dto)
        {
            var role = _context.Roles.FirstOrDefault(x => x.IdRole == id);

            if (role is null)
            {
                return NotFound();
            }

            RoleMapper.ApplyPatch(role, dto);

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
            var role = _context.Roles.FirstOrDefault(x => x.IdRole == id);

            if (role is null)
            {
                return NotFound();
            }

            _context.Roles.Remove(role);

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

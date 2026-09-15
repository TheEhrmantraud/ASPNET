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
    public class UsersController : ControllerBase
    {
        private readonly ShopSystemContext _context;

        public UsersController(ShopSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<UserDTO>> GetAll(int page = 1, int pageSize = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1 || pageSize > 100)
            {
                pageSize = 10;
            }

            var items = _context.Users
                .AsNoTracking()
                .OrderBy(x => x.IdUser)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(UserMapper.ToDto)
                .ToList();

            return Ok(items);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> GetById(int id)
        {
            var user = _context.Users
                .AsNoTracking()
                .FirstOrDefault(x => x.IdUser == id);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(UserMapper.ToDto(user));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<UserDTO> Create(CreateUserDTO dto)
        {
            if (!_context.Roles.Any(x => x.IdRole == dto.RoleId))
            {
                return BadRequest($"Роль с идентификатором {dto.RoleId} не существует.");
            }

            var user = UserMapper.ToEntity(dto);
            _context.Users.Add(user);

            var error = TrySaveChanges();
            if (error is not null)
            {
                return error;
            }

            return CreatedAtAction(nameof(GetById), new { id = user.IdUser }, UserMapper.ToDto(user));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Update(int id, UpdateUserDTO dto)
        {
            var user = _context.Users.FirstOrDefault(x => x.IdUser == id);

            if (user is null)
            {
                return NotFound();
            }

            if (!_context.Roles.Any(x => x.IdRole == dto.RoleId))
            {
                return BadRequest($"Роль с идентификатором {dto.RoleId} не существует.");
            }

            UserMapper.ApplyUpdate(user, dto);

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
        public IActionResult Patch(int id, PatchUserDTO dto)
        {
            var user = _context.Users.FirstOrDefault(x => x.IdUser == id);

            if (user is null)
            {
                return NotFound();
            }

            if (dto.RoleId.HasValue && !_context.Roles.Any(x => x.IdRole == dto.RoleId.Value))
            {
                return BadRequest($"Роль с идентификатором {dto.RoleId} не существует.");
            }

            UserMapper.ApplyPatch(user, dto);

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
            var user = _context.Users.FirstOrDefault(x => x.IdUser == id);

            if (user is null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);

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

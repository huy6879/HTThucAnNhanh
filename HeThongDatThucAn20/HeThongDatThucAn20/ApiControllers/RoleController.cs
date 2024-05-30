using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HeThongDatThucAn20.Data;
using AutoMapper.QueryableExtensions;
using HeThongDatThucAn20.ViewModels;
using AutoMapper;

namespace HeThongDatThucAn20.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly HeThongDatDoAnContext _db;
        private readonly IMapper _mapper;

        public RoleController(HeThongDatDoAnContext db
            , IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleVM>>> GetRoles()
        {
            return await _db.Roles
                .ProjectTo<RoleVM>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // GET: api/Role/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleVM>> GetRole(int id)
        {
            var role = await _db.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return _mapper.Map<RoleVM>(role);
        }

        // PUT: api/Role/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RoleVM role)
        {
            if (id != role.RoleId)
            {
                return BadRequest();
            }

            // Tìm role cũ
            var oldRole = await _db.Roles.FindAsync(id);

            if (oldRole == null)
            {
                return NotFound();
            }

            // Convert role mới sang role cũ
            _mapper.Map(role, oldRole);

            // Chuyển trạng thái role cũ thành Modified
            _db.Entry(oldRole).State = EntityState.Modified;

            try
            {
                // Lưu lại thay đổi
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Role
        [HttpPost]
        public async Task<ActionResult<RoleVM>> Create(RoleVM role)
        {
            var newRole = _mapper.Map<Role>(role);
            newRole.RoleId = 0;
            _db.Roles.Add(newRole);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetRole", new { id = role.RoleId }, role);
        }

        // DELETE: api/Role/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _db.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            _db.Roles.Remove(role);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool Exists(int id)
        {
            return _db.Roles.Any(e => e.RoleId == id);
        }
    }
}

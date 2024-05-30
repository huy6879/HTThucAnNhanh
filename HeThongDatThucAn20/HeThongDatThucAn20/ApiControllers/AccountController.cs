using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HeThongDatThucAn20.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HeThongDatThucAn20.ViewModels;
using HeThongDatThucAn20.Helpers;
using System.Security.Principal;

namespace HeThongDatThucAn20.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly HeThongDatDoAnContext _db;
        private readonly IMapper _mapper;

        public AccountController(HeThongDatDoAnContext db
            , IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountVM>>> GetAccounts()
        {
            return await _db
                .Accounts
                .Include(x => x.Role)
                .ProjectTo<AccountVM>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountVM>> GetAccount(int id)
        {
            var account = await _db.Accounts
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.AccountId == id);

            if (account == null)
            {
                return NotFound();
            }

            return _mapper.Map<AccountVM>(account);
        }

        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AccountCreateVM account)
        {
            if (id != account.AccountId)
            {
                return BadRequest();
            }

            var oldAccount = await _db.Accounts.FindAsync(id);
            
            if (oldAccount == null)
            {
                return NotFound();
            }

            _mapper.Map(account, oldAccount);

            if (!string.IsNullOrEmpty(account.Password))
            {
                oldAccount.RandomKey = ChuanHoa.GenerateRandomKey();
                oldAccount.Password = account.Password.ToMd5Hash(oldAccount.RandomKey);
            }

            _db.Entry(oldAccount).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }

            return NoContent();
        }

        // POST: api/Accounts

        [HttpPost]
        public async Task<ActionResult<Account>> Create(AccountCreateVM account)
        {
            var newAccount = _mapper.Map<Account>(account);
            newAccount.AccountId = 0;
            newAccount.RandomKey = ChuanHoa.GenerateRandomKey();
            newAccount.Password = account.Password.ToMd5Hash(newAccount.RandomKey);

            _db.Accounts.Add(newAccount);

            try
            {
                await _db.SaveChangesAsync();
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction("GetAccount", new { id = account.AccountId }, account);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var account = await _db.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            _db.Accounts.Remove(account);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        private bool Exists(int id)
        {
            return _db.Accounts.Any(e => e.AccountId == id);
        }
    }
}

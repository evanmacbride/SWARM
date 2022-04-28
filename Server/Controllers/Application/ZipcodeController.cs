using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWARM.Server.Controllers.Application
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZipcodeController : BaseController, iBaseController<Zipcode>
    {
        public ZipcodeController(SWARMOracleContext context, 
                                 IHttpContextAccessor httpContextAccessor) 
            : base(context, httpContextAccessor)
        {
        }

        // Unused
        public Task<IActionResult> Delete(int KeyValue)
        {
            throw new NotImplementedException();
        }

        // Overload with string KeyValue
        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(string KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Zipcode itmZipcode = await _context.Zipcodes.Where(x => x.Zip == KeyValue).FirstOrDefaultAsync();
                _context.Zipcodes.Remove(itmZipcode);
                await trans.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Zipcode> lstZipcodes = await _context.Zipcodes.OrderBy(x => x.Zip).ToListAsync();
            return Ok(lstZipcodes);
        }

        // Unused
        public Task<IActionResult> Get(int KeyValue)
        {
            throw new NotImplementedException();
        }

        // Overload with string KeyValue
        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(string KeyValue)
        {
            Zipcode itmZipcode = await _context.Zipcodes.Where(x => x.Zip == KeyValue).FirstOrDefaultAsync();
            return Ok(itmZipcode);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Zipcode _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _zipc = await _context.Zipcodes.Where(x => x.Zip == _Item.Zip).FirstOrDefaultAsync();

                if (_zipc != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Zipcode already exists.");
                }

                _zipc = new Zipcode();

                _zipc.City = _Item.City;
                _zipc.State = _Item.State;
                _zipc.Zip = _Item.Zip;
                _context.Zipcodes.Add(_zipc);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Zipcode _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _zipc = await _context.Zipcodes.Where(x => x.Zip == _Item.Zip).FirstOrDefaultAsync();

                if (_zipc == null)
                {
                    await this.Post(_Item);
                    return Ok();
                }

                _zipc = new Zipcode();

                _zipc.City = _Item.City;
                _zipc.State = _Item.State;
                _zipc.Zip = _Item.Zip;
                _context.Zipcodes.Update(_zipc);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

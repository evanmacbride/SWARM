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
    public class GradeTypeController : BaseController, iBaseController<GradeType>
    {
        public GradeTypeController(SWARMOracleContext context, 
                                    IHttpContextAccessor httpContextAccessor) 
            : base(context, httpContextAccessor)
        {
        }

        // unused
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
                GradeType itmGradeType = await _context.GradeTypes.Where(x => x.GradeTypeCode == KeyValue).FirstOrDefaultAsync();
                _context.GradeTypes.Remove(itmGradeType);
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
            List<GradeType> lstGradeTypes = await _context.GradeTypes.OrderBy(x => x.GradeTypeCode).ToListAsync();
            return Ok(lstGradeTypes);
        }

        // unused
        public Task<IActionResult> Get(int KeyValue)
        {
            throw new NotImplementedException();
        }

        // Overload Get() to accept a string. Route HttpGet to this version.
        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(string KeyValue)
        {
            GradeType itmGradeType = await _context.GradeTypes.Where(x => x.GradeTypeCode == KeyValue).FirstOrDefaultAsync();
            return Ok(itmGradeType);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeType _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _grdtyp = await _context.GradeTypes.Where(x => x.GradeTypeCode == _Item.GradeTypeCode).FirstOrDefaultAsync();

                if (_grdtyp != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Grade type already exists.");
                }

                _grdtyp = new GradeType();

                _grdtyp.Description = _Item.Description;
                _grdtyp.GradeTypeCode = _Item.GradeTypeCode;
                _grdtyp.GradeTypeWeights = _Item.GradeTypeWeights;
                _grdtyp.SchoolId = _Item.SchoolId;
                _context.GradeTypes.Add(_grdtyp);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeType _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _grdtyp = await _context.GradeTypes.Where(x => x.GradeTypeCode == _Item.GradeTypeCode).FirstOrDefaultAsync();

                if (_grdtyp == null)
                {
                    await this.Post(_Item);
                    return Ok();
                }

                _grdtyp = new GradeType();

                _grdtyp.Description = _Item.Description;
                _grdtyp.GradeTypeCode = _Item.GradeTypeCode;
                _grdtyp.GradeTypeWeights = _Item.GradeTypeWeights;
                _grdtyp.SchoolId = _Item.SchoolId;
                _context.GradeTypes.Update(_grdtyp);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

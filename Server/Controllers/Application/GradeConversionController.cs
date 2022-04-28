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
    public class GradeConversionController : BaseController, iBaseController<GradeConversion>
    {
        public GradeConversionController(SWARMOracleContext context, 
                                         IHttpContextAccessor httpContextAccessor) 
            : base(context, httpContextAccessor)
        {
        }

        // Unused
        public Task<IActionResult> Delete(int KeyValue)
        {
            throw new NotImplementedException();
        }

        // Overload Delete() to accept a string. Route HttpDelete to this version.
        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(string KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                GradeConversion itmGradeConversion = await _context.GradeConversions.Where(x => x.LetterGrade == KeyValue).FirstOrDefaultAsync();
                _context.GradeConversions.Remove(itmGradeConversion);
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
            List<GradeConversion> lstGradeConversions = await _context.GradeConversions.OrderBy(x => x.LetterGrade).ToListAsync();
            return Ok(lstGradeConversions);
        }

        // Unused
        public Task<IActionResult> Get(int KeyValue)
        {
            throw new NotImplementedException();
        }

        // Overload Get() to accept a string. Route HttpGet to this version.
        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(string KeyValue)
        {
            GradeConversion itmGradeConversion = await _context.GradeConversions.Where(x => x.LetterGrade == KeyValue).FirstOrDefaultAsync();
            return Ok(itmGradeConversion);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeConversion _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _grdcon = await _context.GradeConversions.Where(x => x.LetterGrade == _Item.LetterGrade).FirstOrDefaultAsync();

                if (_grdcon != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Grade already exists.");
                }

                _grdcon = new GradeConversion();

                _grdcon.GradePoint = _Item.GradePoint;
                _grdcon.LetterGrade = _Item.LetterGrade;
                _grdcon.MaxGrade = _Item.MaxGrade;
                _grdcon.MinGrade = _Item.MinGrade;
                _grdcon.SchoolId = _Item.SchoolId;
                _context.GradeConversions.Add(_grdcon);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.LetterGrade);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeConversion _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _grdcon = await _context.GradeConversions.Where(x => x.LetterGrade == _Item.LetterGrade).FirstOrDefaultAsync();

                if (_grdcon == null)
                {
                    await this.Post(_Item);
                    return Ok();
                }

                _grdcon = new GradeConversion();

                _grdcon.GradePoint = _Item.GradePoint;
                _grdcon.LetterGrade = _Item.LetterGrade;
                _grdcon.MaxGrade = _Item.MaxGrade;
                _grdcon.MinGrade = _Item.MinGrade;
                _grdcon.SchoolId = _Item.SchoolId;
                _context.GradeConversions.Update(_grdcon);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.LetterGrade);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

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
    public class GradeTypeWeightController : BaseController, iBaseController<GradeTypeWeight>
    {
        public GradeTypeWeightController(SWARMOracleContext context, 
                                            IHttpContextAccessor httpContextAccessor) 
            : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                // Delete first record with StudentId
                GradeTypeWeight itmGradeTypeWeight = await _context.GradeTypeWeights.Where(x => x.SectionId == KeyValue).FirstOrDefaultAsync();
                _context.GradeTypeWeights.Remove(itmGradeTypeWeight);
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
            List<GradeTypeWeight> lstGradeTypeWeights = await _context.GradeTypeWeights.OrderBy(x => x.SectionId).ToListAsync();
            return Ok(lstGradeTypeWeights);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            // Get first record with StudentId
            GradeTypeWeight itmGradeTypeWeight = await _context.GradeTypeWeights.Where(x => x.SectionId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmGradeTypeWeight);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeTypeWeight _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _grdtw = await _context.GradeTypeWeights.Where(x => x.SectionId == _Item.SectionId &&
                                                                        x.GradeTypeCode == _Item.GradeTypeCode).FirstOrDefaultAsync();

                if (_grdtw != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Grade type weight already exists.");
                }

                _grdtw = new GradeTypeWeight();

                _grdtw.DropLowest = _Item.DropLowest;
                _grdtw.GradeTypeCode = _Item.GradeTypeCode;
                _grdtw.NumberPerSection = _Item.NumberPerSection;
                _grdtw.PercentOfFinalGrade = _Item.PercentOfFinalGrade;
                _grdtw.SchoolId = _Item.SchoolId;
                _grdtw.SectionId = _Item.SectionId;
                _context.GradeTypeWeights.Add(_grdtw);
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
        public async Task<IActionResult> Put([FromBody] GradeTypeWeight _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _grdtw = await _context.GradeTypeWeights.Where(x => x.SectionId == _Item.SectionId &&
                                                                        x.GradeTypeCode == _Item.GradeTypeCode).FirstOrDefaultAsync();

                if (_grdtw == null)
                {
                    await this.Post(_Item);
                    return Ok();
                }

                _grdtw = new GradeTypeWeight();

                _grdtw.DropLowest = _Item.DropLowest;
                _grdtw.GradeTypeCode = _Item.GradeTypeCode;
                _grdtw.NumberPerSection = _Item.NumberPerSection;
                _grdtw.PercentOfFinalGrade = _Item.PercentOfFinalGrade;
                _grdtw.SchoolId = _Item.SchoolId;
                _grdtw.SectionId = _Item.SectionId;
                _context.GradeTypeWeights.Update(_grdtw);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

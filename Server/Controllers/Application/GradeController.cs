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
    public class GradeController : BaseController, iBaseController<Grade>
    {
        public GradeController(SWARMOracleContext context, 
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
                Grade itmGrade = await _context.Grades.Where(x => x.StudentId == KeyValue).FirstOrDefaultAsync();
                _context.Grades.Remove(itmGrade);
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
            List<Grade> lstGrades = await _context.Grades.OrderBy(x => x.StudentId).ToListAsync();
            return Ok(lstGrades);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            // Get first record with StudentId
            Grade itmGrade = await _context.Grades.Where(x => x.StudentId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmGrade);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Grade _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _grd = await _context.Grades.Where(x => x.StudentId == _Item.StudentId &&
                                                            x.SectionId == _Item.SectionId &&
                                                            x.GradeTypeCode == _Item.GradeTypeCode &&
                                                            x.GradeCodeOccurrence == _Item.GradeCodeOccurrence).FirstOrDefaultAsync();

                if (_grd != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Grade already exists.");
                }

                _grd = new Grade();

                _grd.Comments = _Item.Comments;
                _grd.GradeCodeOccurrence = _Item.GradeCodeOccurrence;
                _grd.GradeTypeCode = _Item.GradeTypeCode;
                _grd.GradeTypeWeight = _Item.GradeTypeWeight;
                _grd.NumericGrade = _Item.NumericGrade;
                _grd.SchoolId = _Item.SchoolId;
                _grd.SectionId = _Item.SectionId;
                _grd.StudentId = _Item.StudentId;
                _context.Grades.Add(_grd);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Grade _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _grd = await _context.Grades.Where(x => x.StudentId == _Item.StudentId &&
                                                            x.SectionId == _Item.SectionId &&
                                                            x.GradeTypeCode == _Item.GradeTypeCode &&
                                                            x.GradeCodeOccurrence == _Item.GradeCodeOccurrence).FirstOrDefaultAsync();

                if (_grd == null)
                {
                    await this.Post(_Item);
                    return Ok();
                }

                _grd = new Grade();

                _grd.Comments = _Item.Comments;
                _grd.GradeCodeOccurrence = _Item.GradeCodeOccurrence;
                _grd.GradeTypeCode = _Item.GradeTypeCode;
                _grd.GradeTypeWeight = _Item.GradeTypeWeight;
                _grd.NumericGrade = _Item.NumericGrade;
                _grd.SchoolId = _Item.SchoolId;
                _grd.SectionId = _Item.SectionId;
                _grd.StudentId = _Item.StudentId;
                _context.Grades.Update(_grd);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

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
    public class EnrollmentController : BaseController, iBaseController<Enrollment>
    {
        public EnrollmentController(SWARMOracleContext context, 
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
                Enrollment itmEnrollment = await _context.Enrollments.Where(x => x.StudentId == KeyValue).FirstOrDefaultAsync();
                _context.Enrollments.Remove(itmEnrollment);
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
            List<Enrollment> lstEnrollments = await _context.Enrollments.OrderBy(x => x.StudentId).ToListAsync();
            return Ok(lstEnrollments);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            // Get first record with StudentId
            Enrollment itmEnrollment = await _context.Enrollments.Where(x => x.StudentId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmEnrollment);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Enrollment _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _enrl = await _context.Enrollments.Where(x => x.StudentId == _Item.StudentId &&
                                                             x.SectionId == _Item.SectionId).FirstOrDefaultAsync();

                if (_enrl != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Enrollment already exists.");
                }

                _enrl = new Enrollment();

                _enrl.EnrollDate = _Item.EnrollDate;
                _enrl.FinalGrade = _Item.FinalGrade;
                _enrl.SchoolId = _Item.SchoolId;
                _enrl.SectionId = _Item.SectionId;
                _enrl.StudentId = _Item.StudentId;
                _context.Enrollments.Add(_enrl);
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
        public async Task<IActionResult> Put([FromBody] Enrollment _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _enrl = await _context.Enrollments.Where(x => x.StudentId == _Item.StudentId &&
                                                             x.SectionId == _Item.SectionId).FirstOrDefaultAsync();

                if (_enrl == null)
                {
                    await this.Post(_Item);
                    return Ok();
                }

                _enrl = new Enrollment();

                _enrl.EnrollDate = _Item.EnrollDate;
                _enrl.FinalGrade = _Item.FinalGrade;
                _enrl.SchoolId = _Item.SchoolId;
                _enrl.SectionId = _Item.SectionId;
                _enrl.StudentId = _Item.StudentId;
                _context.Enrollments.Update(_enrl);
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

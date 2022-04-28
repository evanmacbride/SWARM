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
    public class InstructorController : BaseController, iBaseController<Instructor>
    {
        public InstructorController(SWARMOracleContext context, 
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
                Instructor itmInstructor = await _context.Instructors.Where(x => x.InstructorId == KeyValue).FirstOrDefaultAsync();
                _context.Instructors.Remove(itmInstructor);
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
            List<Instructor> lstInstructors = await _context.Instructors.OrderBy(x => x.InstructorId).ToListAsync();
            return Ok(lstInstructors);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Instructor itmInstructor = await _context.Instructors.Where(x => x.InstructorId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmInstructor);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Instructor _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _instr = await _context.Instructors.Where(x => x.InstructorId == _Item.InstructorId).FirstOrDefaultAsync();

                if (_instr != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Instructor already exists.");
                }

                _instr = new Instructor();

                _instr.FirstName = _instr.FirstName;
                _instr.InstructorId = _instr.InstructorId;
                _instr.LastName = _instr.LastName;
                _instr.Phone = _instr.Phone;
                _instr.Salutation = _instr.Salutation;
                _instr.SchoolId = _Item.SchoolId;
                _instr.StreetAddress = _Item.StreetAddress;
                _instr.Zip = _Item.Zip;
                _context.Instructors.Add(_instr);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.InstructorId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Instructor _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _instr = await _context.Instructors.Where(x => x.InstructorId == _Item.InstructorId).FirstOrDefaultAsync();

                if (_instr == null)
                {
                    await this.Post(_Item);
                    return Ok();
                }

                _instr = new Instructor();

                _instr.FirstName = _instr.FirstName;
                _instr.InstructorId = _instr.InstructorId;
                _instr.LastName = _instr.LastName;
                _instr.Phone = _instr.Phone;
                _instr.Salutation = _instr.Salutation;
                _instr.SchoolId = _Item.SchoolId;
                _instr.StreetAddress = _Item.StreetAddress;
                _instr.Zip = _Item.Zip;
                _context.Instructors.Update(_instr);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.InstructorId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

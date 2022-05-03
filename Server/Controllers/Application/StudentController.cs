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
    public class StudentController : BaseController, iBaseController<Student>
    {
        public StudentController(SWARMOracleContext context, 
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
                Student itmStudent = await _context.Students.Where(x => x.StudentId == KeyValue).FirstOrDefaultAsync();
                _context.Students.Remove(itmStudent);
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
            List<Student> lstStudents = await _context.Students.OrderBy(x => x.StudentId).ToListAsync();
            return Ok(lstStudents);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Student itmStudent = await _context.Students.Where(x => x.StudentId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmStudent);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _stdn = await _context.Students.Where(x => x.StudentId == _Item.StudentId).FirstOrDefaultAsync();

                if (_stdn != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Student already exists.");
                }

                _stdn = new Student();

                _stdn.Employer = _Item.Employer;
                _stdn.FirstName = _Item.FirstName;
                _stdn.LastName = _Item.LastName;
                _stdn.Phone = _Item.Phone;
                _stdn.RegistrationDate = _Item.RegistrationDate;
                _stdn.Salutation = _Item.Salutation;
                _stdn.SchoolId = _Item.SchoolId;
                _stdn.Zip = _Item.Zip;
                _context.Students.Add(_stdn);
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
        public async Task<IActionResult> Put([FromBody] Student _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _stdn = await _context.Students.Where(x => x.StudentId == _Item.StudentId).FirstOrDefaultAsync();

                if (_stdn == null)
                {
                    await this.Post(_Item);
                    return Ok();
                }

                _stdn = new Student();

                _stdn.Employer = _Item.Employer;
                _stdn.FirstName = _Item.FirstName;
                _stdn.LastName = _Item.LastName;
                _stdn.Phone = _Item.Phone;
                _stdn.RegistrationDate = _Item.RegistrationDate;
                _stdn.Salutation = _Item.Salutation;
                _stdn.SchoolId = _Item.SchoolId;
                _stdn.Zip = _Item.Zip;
                _context.Students.Update(_stdn);
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

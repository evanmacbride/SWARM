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
    public class SectionController : BaseController, iBaseController<Section>
    {
        public SectionController(SWARMOracleContext context,
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
                Section itmSection = await _context.Sections.Where(x => x.SectionId == KeyValue).FirstOrDefaultAsync();
                _context.Sections.Remove(itmSection);
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
            List<Section> lstSections = await _context.Sections.OrderBy(x => x.SectionId).ToListAsync();
            return Ok(lstSections);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Section itmSection = await _context.Sections.Where(x => x.SectionId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmSection);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Section _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _sect = await _context.Sections.Where(x => x.SectionId == _Item.SectionId).FirstOrDefaultAsync();

                if (_sect != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Section already exists.");
                }

                _sect = new Section();

                _sect.Capacity = _Item.Capacity;
                _sect.CourseNo = _Item.CourseNo;
                _sect.InstructorId = _Item.InstructorId;
                _sect.Location = _Item.Location;
                _sect.SchoolId = _Item.SchoolId;
                _sect.SectionId = _Item.SectionId;
                _sect.SectionNo = _Item.SectionNo;
                _context.Sections.Add(_sect);
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
        public async Task<IActionResult> Put([FromBody] Section _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _sect = await _context.Sections.Where(x => x.SectionId == _Item.SectionId).FirstOrDefaultAsync();

                if (_sect == null)
                {
                    await this.Post(_Item);
                    return Ok();
                }

                _sect = new Section();

                _sect.Capacity = _Item.Capacity;
                _sect.CourseNo = _Item.CourseNo;
                _sect.InstructorId = _Item.InstructorId;
                _sect.Location = _Item.Location;
                _sect.SchoolId = _Item.SchoolId;
                _sect.SectionId = _Item.SectionId;
                _sect.SectionNo = _Item.SectionNo;
                _context.Sections.Update(_sect);
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

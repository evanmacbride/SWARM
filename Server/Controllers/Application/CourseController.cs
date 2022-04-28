using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using SWARM.Server.Models;
using SWARM.Shared;
using SWARM.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.Application
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : BaseController, iBaseController<Course>
    {
        public CourseController(SWARMOracleContext context,
                                IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Course> lstCourses = await _context.Courses.OrderBy(x => x.CourseNo).ToListAsync();
            return Ok(lstCourses);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Course itmCourse = await _context.Courses.Where(x => x.CourseNo == KeyValue).FirstOrDefaultAsync();
            return Ok(itmCourse);
        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Course itmCourse = await _context.Courses.Where(x => x.CourseNo == KeyValue).FirstOrDefaultAsync();
                _context.Courses.Remove(itmCourse);
                await trans.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Course _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _crse = await _context.Courses.Where(x => x.CourseNo == _Item.CourseNo).FirstOrDefaultAsync();

                if (_crse != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Course already exists.");
                }

                _crse = new Course();

                _crse.Cost = _Item.Cost;
                _crse.Description = _Item.Description;
                _crse.Prerequisite = _Item.Prerequisite;
                _crse.PrerequisiteSchoolId = _Item.PrerequisiteSchoolId;
                _crse.SchoolId = _Item.SchoolId;
                _context.Courses.Add(_crse);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.CourseNo);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Course _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _crse = await _context.Courses.Where(x => x.CourseNo == _Item.CourseNo).FirstOrDefaultAsync();

                if (_crse == null)
                {
                    await this.Post(_Item);
                    return Ok();
                }

                _crse = new Course();

                _crse.Cost = _Item.Cost;
                _crse.Description = _Item.Description;
                _crse.Prerequisite = _Item.Prerequisite;
                _crse.PrerequisiteSchoolId = _Item.PrerequisiteSchoolId;
                _crse.SchoolId = _Item.SchoolId;
                _context.Courses.Update(_crse);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.CourseNo);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /*
        [HttpPost]
        [Route("GetCourses")]
        public async Task<DataEnvelope<CourseDTO>> GetCoursesPost([FromBody] DataSourceRequest gridRequest)
        {
            DataEnvelope<CourseDTO> dataToReturn = null;
            IQueryable<CourseDTO> queriableStates = _context.Courses
                    .Select(sp => new CourseDTO
                    {
                        Cost = sp.Cost,
                        CourseNo = sp.CourseNo,
                        CreatedBy = sp.CreatedBy,
                        CreatedDate = sp.CreatedDate,
                        Description = sp.Description,
                        ModifiedBy = sp.ModifiedBy,
                        ModifiedDate = sp.ModifiedDate,
                        Prerequisite = sp.Prerequisite,
                        PrerequisiteSchoolId = sp.PrerequisiteSchoolId,
                        SchoolId = sp.SchoolId
                    });

            // use the Telerik DataSource Extensions to perform the query on the data
            // the Telerik extension methods can also work on "regular" collections like List<T> and IQueriable<T>
            try
            {

                DataSourceResult processedData = await queriableStates.ToDataSourceResultAsync(gridRequest);

                if (gridRequest.Groups.Count > 0)
                {
                    // If there is grouping, use the field for grouped data
                    // The app must be able to serialize and deserialize it
                    // Example helper methods for this are available in this project
                    // See the GroupDataHelper.DeserializeGroups and JsonExtensions.Deserialize methods
                    dataToReturn = new DataEnvelope<CourseDTO>
                    {
                        GroupedData = processedData.Data.Cast<AggregateFunctionsGroup>().ToList(),
                        TotalItemCount = processedData.Total
                    };
                }
                else
                {
                    // When there is no grouping, the simplistic approach of 
                    // just serializing and deserializing the flat data is enough
                    dataToReturn = new DataEnvelope<CourseDTO>
                    {
                        CurrentPageData = processedData.Data.Cast<CourseDTO>().ToList(),
                        TotalItemCount = processedData.Total
                    };
                }
            }
            catch (Exception e)
            {
                //fixme add decent exception handling
            }
            return dataToReturn;
        }
        */

    }
}

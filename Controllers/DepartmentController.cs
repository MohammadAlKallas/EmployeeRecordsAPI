using EmployeeRecordsAPI.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {

        private readonly IDepartment _IDepartment;
        public DepartmentController(IDepartment _IDepartment)
        {
           this._IDepartment = _IDepartment;
        }

        [HttpGet("GetAllDept")]
        public async Task<ActionResult> GetAllDept()
        {
            try
            {
                var Result = await _IDepartment.GetAllDept();
                if (Result == null)
                {
                    return NotFound("There are no Departments!");
                }

                return Ok(Result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error retrieving data from the database");
            }
        }
        [HttpGet("GetDept")]
        public async Task<ActionResult> GetDept(int DeptId)
        {
            try
            {
                var Result = await _IDepartment.GetDept(DeptId);
                if (Result == null)
                {
                    return NotFound();
                }

                return Ok(Result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error retrieving data from the database");
            }
        }
    }
}

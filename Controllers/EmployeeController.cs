using EmployeeRecordsAPI.Interface;
using EmployeeRecordsAPI.Models;
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
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _IEmployee;
        public EmployeeController(IEmployee _IEmployee)
        {
            this._IEmployee = _IEmployee;
        }
        [HttpGet("GetAllEmployees")]
        public async Task<ActionResult> GetAllEmployees()
        {
            try
            {
                var Result = await _IEmployee.GetAllEmployees();
                if (Result == null)
                {
                    return NotFound("There are no employees!");
                }

                return Ok(Result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error retrieving data from the database");
            }
        }
        [HttpGet("GetEmployee/{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var Result = await _IEmployee.GetEmployee(id);
                if (Result == null)
                {
                    return NotFound();
                }
                return Result;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       "Error retrieving data from the database");
            }
        }
        [HttpPost("CreateEmployee")]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee Employee)
        {
            try
            {
                if (Employee == null)
                {
                    return BadRequest();
                }
                var Result = await _IEmployee.CreateEmployee(Employee);
                return CreatedAtAction(nameof(GetEmployee),
                new { id =Result.ID }, Result);
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                               "Error retrieving data from the database");
            }
        }
        [HttpPut("UpdateEmployee/{id}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee Employee)
        {
            try
            {
                if (id != Employee.ID)
                {
                    return BadRequest("Employee ID Mismatchg");
                }
                var Result = await _IEmployee.UpdateEmployee(id, Employee);
                if (Result == null)
                {
                    return NotFound($"Employee With ID ={id} not Found");
                }
                return Result;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error Updating Data");
            }
        }
        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            try
            {
                var Result = await _IEmployee.GetEmployee(id);
                if (Result == null)
                {
                    return NotFound($"Employee With ID ={id} not  Found");
                }
                return await _IEmployee.DeleteEmployee(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error Deleteing Data");
            }
        }
        [HttpGet("FindEmployee/{value}")]
        public async Task<ActionResult> FindEmployee(string value)
        {
            try
            {
                var Result = await _IEmployee.FindEmployee(value);
                if (Result == null)
                {
                    return NotFound(value+" IS Not Found");
                }
                return Ok(Result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error Deleteing Data");
            }
        }
    }
}

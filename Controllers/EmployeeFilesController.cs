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
    public class EmployeeFilesController : ControllerBase
    {
        private readonly IEmployeeFiles _IEmployeeFiles;
        public EmployeeFilesController(IEmployeeFiles _IEmployeeFiles)
        {
            this._IEmployeeFiles = _IEmployeeFiles;
        }
        [HttpPost("SetImage/{id}")]
        public async Task<ActionResult> SetImage(int id ,IFormFile file)
        {
            try
            {
                bool Result =await _IEmployeeFiles.EditImage(file, id);
                if (Result == false)
                {
                    return NotFound("Problem in Update Image !");
                }

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error retrieving data from the database");
            }
        }

        [HttpPost("CreateImage/{id}")]
        public async Task<ActionResult> CreateImage(int id,IFormFile file)
        {
            try
            {
                bool Result =await _IEmployeeFiles.CreateImage(file, id);
                if (Result == false)
                {
                    return NotFound("Problem in Create Image!");
                }

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error retrieving data from the database");
            }
        }

        [HttpGet("GetAllFile/{id}")]
        public async Task<ActionResult> GetAllFile(int id)
        {
            try
            {
                var Result = await _IEmployeeFiles.GetFile(id);
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

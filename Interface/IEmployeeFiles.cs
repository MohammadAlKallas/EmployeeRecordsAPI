using EmployeeRecordsAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsAPI.Interface
{
   public interface IEmployeeFiles
    {
        Task<bool> EditImage(IFormFile file,int employeeID);
        Task<bool> CreateImage(IFormFile file, int employeeID);
        Task<EmployeeFiles> GetFile(int employeeID);
    }
}

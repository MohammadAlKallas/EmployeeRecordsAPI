using EmployeeRecordsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsAPI.Interface
{
     public interface IDepartment
    {
        Task<IEnumerable<Department>> GetAllDept();
        Task<Department> GetDept(int DeptId);
    }
}

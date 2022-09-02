using EmployeeRecordsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsAPI.Interface
{
     public interface IEmployee
    {
        Task<IEnumerable<Employee>> GetAllEmployees();
        Task<Employee> GetEmployee(int EmployeeId);
        Task<Employee> CreateEmployee(Employee Employee);
        Task<Employee> UpdateEmployee(int Id,Employee Employee);
        Task<Employee> DeleteEmployee(int EmployeeId);
        Task<IEnumerable<Employee>> FindEmployee(string Value);

    }
}

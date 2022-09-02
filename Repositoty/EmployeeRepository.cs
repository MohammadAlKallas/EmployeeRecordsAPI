using EmployeeRecordsAPI.Interface;
using EmployeeRecordsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace EmployeeRecordsAPI.Repositoty
{
    public class EmployeeRepository : IEmployee
    {

        private readonly IConnection _connection;
        private readonly IDepartment department;
        private readonly IEmployeeFiles files;

        public EmployeeRepository(
            IDepartment department,
            IEmployeeFiles files,
            IConnection _connection)
        {
            this.department = department;
            this.files = files;
            this._connection = _connection;
        }
     
        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            string query = "select * from Employee";
            SqlCommand sqlCommand = new SqlCommand(query, _connection.OpenConnect());

            var EmployeesList = new List<Employee>();
            var Reader = sqlCommand.ExecuteReader();
            while (await Reader.ReadAsync())
            {
                EmployeesList.Add(new Employee()
                {
                    ID = Reader.GetInt32("ID"),
                    Name = Reader.GetString("Name"),
                    Address = Reader.GetString("Address"),
                    DateOfBirth = Reader.GetDateTime("DateOfBirth"),
                    DepartmentID = Reader.GetInt32("DepartmentID"),
                    Departments = await department
                                .GetDept(Reader.GetInt32("DepartmentID")),
                    File=await files.GetFile(Reader.GetInt32("ID"))
                });
            }

            _connection.CloseConnect();
            if (EmployeesList.Count() != 0)
                return EmployeesList;

            else return null;
        }
        public async Task<Employee> GetEmployee(int EmployeeId)
        {
            string query = "Select * From Employee where ID=@Id";
            SqlCommand sqlCommand = new SqlCommand(query, _connection.OpenConnect());
            sqlCommand.Parameters.AddWithValue("@Id", EmployeeId);

            Employee Employee = null;
            var Reader = sqlCommand.ExecuteReader();
            while (await Reader.ReadAsync())
            {
                Employee = new Employee()
                {
                    ID = Reader.GetInt32("ID"),
                    Name = Reader.GetString("Name"),
                    Address = Reader.GetString("Address"),
                    DateOfBirth = Reader.GetDateTime("DateOfBirth"),
                    DepartmentID = Reader.GetInt32("DepartmentID"),
                    Departments = await department
                                .GetDept(Reader.GetInt32("DepartmentID")),
                    File = await files.GetFile(Reader.GetInt32("ID"))
                };
            }

            _connection.CloseConnect();
            return Employee;
        }
        public async Task<Employee> CreateEmployee(Employee Employee)
        {
            string query = "insert into Employee(Name,DateOfBirth,DepartmentID,Address) " +
                "Values(@name,@dateofbirth,@departmentid,@address)" +
                " SELECT SCOPE_IDENTITY() AS SCOPEIDENTITYOUTPUT   ";
            SqlCommand sqlCommand = new SqlCommand(query, _connection.OpenConnect());
            sqlCommand.Parameters.AddWithValue("@name", Employee.Name);
            sqlCommand.Parameters.AddWithValue("@dateofbirth", Employee.DateOfBirth);
            sqlCommand.Parameters.AddWithValue("@departmentid", Employee.DepartmentID);
            sqlCommand.Parameters.AddWithValue("@address", Employee.Address);

            var returnID =Convert.ToInt32(sqlCommand.ExecuteScalar());


            _connection.CloseConnect();
            return await GetEmployee(returnID);
        }
        public async Task<Employee> UpdateEmployee(int Id, Employee Employee)
        {
            var Result = await GetEmployee(Id);
            if (Result != null)
            {
                string query =
                    "update Employee " +
                    "set Name=@name," +
                    "DateOfBirth=@dateofbirth," +
                    "DepartmentID=@departmentid," +
                    "Address=@address " +
                    "where ID=@id";
                SqlCommand sqlCommand = new SqlCommand(query, _connection.OpenConnect());
                sqlCommand.Parameters.AddWithValue("@name", Employee.Name);
                sqlCommand.Parameters.AddWithValue("@dateofbirth", Employee.DateOfBirth);
                sqlCommand.Parameters.AddWithValue("@departmentid", Employee.DepartmentID);
                sqlCommand.Parameters.AddWithValue("@address", Employee.Address);
                sqlCommand.Parameters.AddWithValue("@id", Id);
                await sqlCommand.ExecuteNonQueryAsync();

                _connection.CloseConnect();
                return await GetEmployee(Id);
            }
            return null;

        }
        public async Task<Employee> DeleteEmployee(int EmployeeId)
        {
            var Result =await GetEmployee(EmployeeId);
            if (Result != null)
            {
                string query = "Delete From Employee where ID=@Id";
                SqlCommand sqlCommand = new SqlCommand(query, _connection.OpenConnect());
                sqlCommand.Parameters.AddWithValue("@Id", EmployeeId);

                await sqlCommand.ExecuteNonQueryAsync();

                _connection.CloseConnect();
                return Result;
            }
            return null;
        }
        public async Task<IEnumerable<Employee>> FindEmployee(string value)
        {
            string query = "Select * From Employee where Name like '"+value+"%' ";
            SqlCommand sqlCommand = new SqlCommand(query, _connection.OpenConnect());
            await sqlCommand.ExecuteNonQueryAsync();

            var EmployeesList = new List<Employee>();
            var Reader = sqlCommand.ExecuteReader();
            while (await Reader.ReadAsync())
            {
                EmployeesList.Add(new Employee()
                {
                    ID = Reader.GetInt32("ID"),
                    Name = Reader.GetString("Name"),
                    Address = Reader.GetString("Address"),
                    DateOfBirth = Reader.GetDateTime("DateOfBirth"),
                    DepartmentID = Reader.GetInt32("DepartmentID"),
                    Departments = await department
                                .GetDept(Reader.GetInt32("DepartmentID")),
                    File = await files.GetFile(Reader.GetInt32("ID"))
                });
            }

            _connection.CloseConnect();
            if (EmployeesList.Count() != 0)
                return EmployeesList;
            else return null;
        }
    }
}

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
    public class DepartmentRepository : IDepartment
    {

        private readonly IConnection _connection;
        public DepartmentRepository(IConnection _connection)
        {
            this._connection = _connection;
        }
        public async Task<IEnumerable<Department>> GetAllDept()
        {
            string query = "select * from Department";
            SqlCommand sqlCommand = 
                new SqlCommand(query, _connection.OpenConnect());

            var DepartmentList = new List<Department>();
            var Reader = sqlCommand.ExecuteReader();
            while (await Reader.ReadAsync())
            {
                DepartmentList.Add(new Department()
                {
                    ID = Reader.GetInt32("ID"),
                    DeptName = Reader.GetString("DeptName"),
                });
            }

            _connection.CloseConnect();
            if (DepartmentList.Count() != 0)
                return DepartmentList;

            else return null;
        }
        public async Task<Department> GetDept(int DeptId)
        {
            var Dept = new Department();
            string query = "Select * From Department Where ID = @Id";
            SqlCommand sqlCommand = new SqlCommand(query, _connection.OpenConnect());
            sqlCommand.Parameters.AddWithValue("@Id", DeptId);

            var Reader = sqlCommand.ExecuteReader();
            while (await Reader.ReadAsync())
            {
                Dept.ID = Reader.GetInt32("ID");
                Dept.DeptName = Reader.GetString("DeptName");
            }

            return Dept;
        }
    }
}

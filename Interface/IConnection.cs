using EmployeeRecordsAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRecordsAPI.Interface
{
    public interface IConnection
    {
        public SqlConnection OpenConnect();
        public void CloseConnect();
    }
}

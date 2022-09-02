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
    public class ConnectionDataBase :IConnection
    {
        public IConfiguration configuration;
        public SqlConnection sqlconnection;

        public ConnectionDataBase(IConfiguration configuration) {
            this.configuration = configuration;
            sqlconnection = new SqlConnection(
            configuration.GetConnectionString("EmployeeData"));
        }

        public SqlConnection OpenConnect()
        {
            if(sqlconnection.State==ConnectionState.Closed)
            sqlconnection.Open();
            return sqlconnection;
        }

        public void CloseConnect()
        {
            sqlconnection.Close();
        }

    }
}

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
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace EmployeeRecordsAPI.Repositoty
{
    public class EmployeeFilesRepository : IEmployeeFiles
    {
        private readonly IConnection _connection;
        private readonly IWebHostEnvironment appEnvironment;
        public EmployeeFilesRepository(IConnection _connection, IWebHostEnvironment appEnvironment)
        {
            this._connection = _connection;
            this.appEnvironment = appEnvironment;
        }
        public async Task<EmployeeFiles> GetFile(int employeeID)
        {
            string query = "Select * From EmployeeFiles where ID=@Id";
            SqlCommand sqlCommand = new SqlCommand(query, _connection.OpenConnect());
            sqlCommand.Parameters.AddWithValue("@Id", employeeID);


            var employeeFile = new EmployeeFiles();
            var Reader = sqlCommand.ExecuteReader();

        
            while (await Reader.ReadAsync())
            {
                employeeFile = new EmployeeFiles
                {
                    ID = Reader.GetInt32("ID"),
                    Image=Reader[1] as byte[],
                    FileName = Reader.GetString("FileName"),
                    FileSize = Reader.GetDouble("FileSize"),
                };
            }
            return employeeFile;
        }
        public async Task<bool> CreateImage(IFormFile file, int employeeID)
        {
            byte[] ST = null;
            string FileName=null;
            long Filelength;

            if (file == null)
            {
                ST = System.IO.File
                .ReadAllBytes(appEnvironment.WebRootPath + "/Images/UserImage.png");
                FileName = "UserImage";
                Filelength = 9219;
            }
            else {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    ST = stream.ToArray();
                }
                FileName = file.FileName;
                Filelength = file.Length;
            }
            
            string  query = "insert into EmployeeFiles(ID,Image,FileName,FileSize) " +
                  "Values(@id,@image,@fileName,@fileSize)";
            SqlCommand sqlCommand = new SqlCommand(query, _connection.OpenConnect());
            sqlCommand.Parameters.AddWithValue("@image", ST);
            sqlCommand.Parameters.AddWithValue("@id", employeeID);
            sqlCommand.Parameters.AddWithValue("@fileName",FileName);
            sqlCommand.Parameters.AddWithValue("@fileSize", Filelength);
            var Result = await sqlCommand.ExecuteNonQueryAsync();
            _connection.CloseConnect();
            if (Result == 0)
                return false;

            return true;
        }
        public async Task<bool> EditImage(IFormFile file, int employeeID)
        {

            byte[] ST = null;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                ST = stream.ToArray();
            }

            string query = "update EmployeeFiles " +
                   "set Image=@image," +
                   "FileName=@fileName," +
                   "FileSize=@fileSize " +
                   "where ID=@id";

            SqlCommand sqlCommand = new SqlCommand(query, _connection.OpenConnect());
            sqlCommand.Parameters.AddWithValue("@image", ST);
            sqlCommand.Parameters.AddWithValue("@id", employeeID);
            sqlCommand.Parameters.AddWithValue("@fileName", file.FileName);
            sqlCommand.Parameters.AddWithValue("@fileSize", file.Length);
            var Result= await sqlCommand.ExecuteNonQueryAsync();
            _connection.CloseConnect();
            if (Result == 0)
                return false;
            return true;
        }
    }
}

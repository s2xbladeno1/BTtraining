using AppTool.Dtos;
using AppTool.Model;
using AppTool.Services.ToolServices;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AppTool.Services
{
    public class ToolService: IToolService
    {
        private string connectionString;
        public ToolService()
        {
            connectionString = "Server=S2XNGOCLINH\\SQLEXPRESS;Database=TestUser;Trusted_Connection=True;MultipleActiveResultSets=true";
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }
        public async Task<List<Tool>> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sql = @"select * from [Tool]";
                dbConnection.Open();
                return dbConnection.Query<Tool>(sql).ToList(); ;
            }
        }

        public async Task<List<Tool>> Search(FilterDto dto)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sql =  @"select * from [Tool] where (Tags like '%" + dto.Tags + "%') and (Creator like '%" + dto.Creator + "%') and (Title like '%" + dto.Title + "%') and (Description like '%" + dto.Description + "%') ";
                 dbConnection.Open();
                return dbConnection.Query<Tool>(sql).ToList();
            }
        }
    }
}

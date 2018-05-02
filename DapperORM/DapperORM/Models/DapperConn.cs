using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DapperORM.Models
{
    public class DapperConn
    {
        public static IDbConnection GetConnection()
        {
            string connStr = ConfigurationManager.AppSettings["SQLConn"];
            return new SqlConnection(connStr);
        }
    }
}
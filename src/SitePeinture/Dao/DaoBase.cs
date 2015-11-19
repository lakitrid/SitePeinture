using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Configuration;
using SitePeinture.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture.Dao
{
    public class DaoBase
    {
        private string connectionString;

        public IConfiguration configuration { get; set; }

        public DaoBase(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionString = configuration.Get<string>("connectionStrings:painting-local");
        }

        protected void Execute(Action<SqlCommand> action)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        action.Invoke(command);
                    }

                    connection.Close();
                }
            }
            catch (Exception exc)
            {
            }
        }
    }
}

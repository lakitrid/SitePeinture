using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace SitePeinture.Dao
{
    public class DaoBase
    {
        private string connectionString;

        public IConfiguration configuration { get; set; }

        public DaoBase(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionString = configuration.GetSection("connectionStrings:painting-local").Value;
        }

        protected void Execute(Action<DbCommand> action)
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
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal void Init()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT version From Version";

                        try
                        {
                            using (IDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    connection.Close();
                                    return;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    // Here we have to create the database
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"CREATE TABLE Event(
                                            Id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                                            Title text NOT NULL,
                                            Description text NULL,
                                            Modification text NOT NULL,
                                            Expiration text NOT NULL)";
                        command.ExecuteNonQuery();
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"CREATE TABLE Painting(
                                            Id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                                            Title text NOT NULL,
                                            ThemeId integer NOT NULL,
                                            Description text NULL,
                                            Filename text NOT NULL,
                                            OnSlider integer NULL)";
                        command.ExecuteNonQuery();
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"CREATE TABLE Theme(
                                            Id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                                            ParentId integer NULL,
                                            Title text NOT NULL,
                                            Description text NULL)";
                        command.ExecuteNonQuery();
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"CREATE TABLE User(
                                            Id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                                            Login text NOT NULL,
                                            Password text NOT NULL)";
                        command.ExecuteNonQuery();

                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"CREATE TABLE Version(
                                            version integer)";
                        command.ExecuteNonQuery();

                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"INSERT INTO Version(version) VALUES(1)";
                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                }
            }
            catch (Exception exc)
            {
                throw;
            }
        }
    }
}

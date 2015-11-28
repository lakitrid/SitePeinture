using Microsoft.AspNet.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using SitePeinture.Models;
using System;
using System.Collections.Generic;
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
            this.connectionString = configuration.GetSection("connectionStrings:painting-local").Value;
        }

        protected void Execute(Action<SqliteCommand> action)
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(this.connectionString))
                {
                    connection.Open();

                    using (SqliteCommand command = new SqliteCommand())
                    {
                        command.Connection = connection;
                        action.Invoke(command);
                    }

                    connection.Close();
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
                using (SqliteConnection connection = new SqliteConnection(this.connectionString))
                {
                    connection.Open();

                    SqliteCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT version From Version";

                    try
                    {
                        SqliteDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            connection.Close();
                            return;
                        }
                    }
                    catch (SqliteException)
                    {
                    }

                    // Here we have to create the database
                    command = connection.CreateCommand();
                    command.CommandText = @"CREATE TABLE Event(
                                            Id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                                            Title text NOT NULL,
                                            Description text NULL,
                                            Modification text NOT NULL,
                                            Expiration text NOT NULL)";
                    command.ExecuteNonQuery();

                    command = connection.CreateCommand();
                    command.CommandText = @"CREATE TABLE Painting(
                                            Id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                                            Title text NOT NULL,
                                            ThemeId integer NOT NULL,
                                            Description text NULL,
                                            Filename text NOT NULL,
                                            OnSlider integer NULL)";
                    command.ExecuteNonQuery();

                    command = connection.CreateCommand();
                    command.CommandText = @"CREATE TABLE Theme(
                                            Id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                                            ParentId integer NULL,
                                            Title text NOT NULL,
                                            Description text NULL)";
                    command.ExecuteNonQuery();

                    command = connection.CreateCommand();
                    command.CommandText = @"CREATE TABLE User(
                                            Id integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                                            Login text NOT NULL,
                                            Password text NOT NULL)";
                    command.ExecuteNonQuery();

                    command = connection.CreateCommand();
                    command.CommandText = @"CREATE TABLE Version(
                                            version integer)";
                    command.ExecuteNonQuery();

                    command = connection.CreateCommand();
                    command.CommandText = @"INSERT INTO Version(version) VALUES(1)";
                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

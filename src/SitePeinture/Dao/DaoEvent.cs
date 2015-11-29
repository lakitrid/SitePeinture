﻿using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using SitePeinture.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture.Dao
{
    public class DaoEvent : DaoBase
    {
        private const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";

        public DaoEvent(IConfiguration configuration) : base(configuration)
        {
        }

        public IEnumerable<Event> GetAll()
        {
            List<Event> result = new List<Event>();

            this.Execute((command) =>
            {
                command.CommandText = @"SELECT Id, Title, Description, Modification, Expiration FROM [Event]";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(this.FillEvent(reader));
                    }
                }
            });

            return result.OrderBy(d => d.ExpirationDate);
        }

        public void Edit(Event @event)
        {
            if (@event.IsNew)
            {
                this.Execute((command) =>
                {
                    command.CommandText = @"INSERT INTO [Event] (Title, Description, Modification, Expiration)
VALUES(@title, @description,@modification, @expiration)";

                    command.Parameters.Add("@title", SqliteType.Text);
                    command.Parameters.Add("@description", SqliteType.Text);
                    command.Parameters.Add("@modification", SqliteType.Text);
                    command.Parameters.Add("@expiration", SqliteType.Text);

                    command.Parameters["@title"].Value = @event.Title;
                    command.Parameters["@description"].Value = @event.Description;
                    command.Parameters["@modification"].Value = DateTime.Now.ToString(DateTimeFormat, CultureInfo.InvariantCulture);
                    command.Parameters["@expiration"].Value = @event.ExpirationDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture);

                    command.ExecuteNonQuery();
                });
            }
            else
            {
                this.Execute((command) =>
                {
                    command.CommandText = @"UPDATE [Event]
SET Title = @title,
	Description = @description,
	Modification = @modification,
    Expiration = @expiration
WHERE Id = @id";
                    command.Parameters.Add("@title", SqliteType.Text);
                    command.Parameters.Add("@description", SqliteType.Text);
                    command.Parameters.Add("@modification", SqliteType.Text);
                    command.Parameters.Add("@expiration", SqliteType.Text);
                    command.Parameters.Add("@id", SqliteType.Integer);

                    command.Parameters["@id"].Value = @event.Id;
                    command.Parameters["@title"].Value = @event.Title;
                    command.Parameters["@description"].Value = @event.Description;
                    command.Parameters["@modification"].Value = DateTime.Now.ToString(DateTimeFormat, CultureInfo.InvariantCulture);
                    command.Parameters["@expiration"].Value = @event.ExpirationDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture);

                    command.ExecuteNonQuery();
                });
            }
        }

        public void Delete(int id)
        {
            this.Execute((command) =>
            {
                command.CommandText = "DELETE FROM [Event] WHERE Id = @id";

                command.Parameters.Add("@id", SqliteType.Integer);
                command.Parameters["@id"].Value = id;
                command.ExecuteNonQuery();
            });
        }

        private Event FillEvent(SqliteDataReader reader)
        {
            Event value = new Event();

            int index = reader.GetOrdinal("Id");
            if (!reader.IsDBNull(index))
            {
                value.Id = reader.GetInt32(index);
            }

            index = reader.GetOrdinal("Title");
            if (!reader.IsDBNull(index))
            {
                value.Title = reader.GetString(index);
            }

            index = reader.GetOrdinal("Description");
            if (!reader.IsDBNull(index))
            {
                value.Description = reader.GetString(index);
            }

            index = reader.GetOrdinal("Modification");
            if (!reader.IsDBNull(index))
            {
                string modification = reader.GetString(index);

                value.ModificationDate = DateTime.Parse(modification);
            }

            index = reader.GetOrdinal("Expiration");
            if (!reader.IsDBNull(index))
            {
                string expiration = reader.GetString(index);
                value.ExpirationDate = DateTime.Parse(expiration);
            }

            return value;
        }
    }
}

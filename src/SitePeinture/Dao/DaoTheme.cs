using Microsoft.Extensions.Configuration;
using SitePeinture.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture.Dao
{
    public class DaoTheme : DaoBase
    {
        public DaoTheme(IConfiguration configuration) : base(configuration)
        {
        }

        public List<Theme> GetAll()
        {
            List<Theme> result = new List<Theme>();

            this.Execute((command) =>
            {
                command.CommandText = "Select * from Theme";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(this.FillTheme(reader));
                }
            });

            return result;

        }

        private Theme FillTheme(SqlDataReader reader)
        {
            Theme value = new Theme();

            int index = reader.GetOrdinal("Id");
            if (!reader.IsDBNull(index))
            {
                value.Id = reader.GetDecimal(index);
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

            return value;
        }
    }
}

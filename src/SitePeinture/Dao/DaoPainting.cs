using Microsoft.Framework.Configuration;
using SitePeinture.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture.Dao
{
    public class DaoPainting : DaoBase
    {
        public DaoPainting(IConfiguration configuration) : base(configuration)
        {
        }

        public List<Painting> GetAll()
        {
            List<Painting> result = new List<Painting>();

            this.Execute((command) =>
            {
                command.CommandText = "Select * from Painting";

                SqlDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    result.Add(this.FillPainting(reader));
                }
            });

            return result;
        }

        private Painting FillPainting(SqlDataReader reader)
        {
            Painting value = new Painting();

            int index = reader.GetOrdinal("Id");
            if(!reader.IsDBNull(index))
            {
                value.Id = reader.GetDecimal(index);
            }

            index = reader.GetOrdinal("Title");
            if (!reader.IsDBNull(index))
            {
                value.Title = reader.GetString(index);
            }
            index = reader.GetOrdinal("ThemeId");
            if (!reader.IsDBNull(index))
            {
                value.ThemeId = reader.GetDecimal(index);
            }

            index = reader.GetOrdinal("Description");
            if (!reader.IsDBNull(index))
            {
                value.Description = reader.GetString(index);
            }

            index = reader.GetOrdinal("Filename");
            if (!reader.IsDBNull(index))
            {
                value.Filename = reader.GetString(index);
            }


            return value;
        }
    }
}

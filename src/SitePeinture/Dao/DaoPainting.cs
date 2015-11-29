using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using SitePeinture.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
                command.CommandText = "Select p.Id, p.Title, p.ThemeId, p.Filename, p.Description, p.OnSlider, t.Title ThemeTitle from Painting p inner join Theme t on t.Id = p.ThemeId";

                using (SqliteDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        result.Add(this.FillPainting(reader));
                    }
                }
            });

            return result;
        }

        public void Edit(Painting painting)
        {
            if (painting.IsNew)
            {
                // Create the file
                string path = Path.Combine("images\\tableaux", painting.Filename);
                if (File.Exists(path) == false)
                {
                    string data = painting.Data;
                    string imageDatas = data.Split(',')[1];

                    File.WriteAllBytes(path, Convert.FromBase64String(imageDatas));
                }

                this.Execute((command) =>
                {
                    command.CommandText = @"INSERT INTO Painting (Title, ThemeId, Description, Filename, OnSlider)
VALUES(@title, @themeId,@description, @fileName, @onslider)";

                    command.Parameters.Add("@title", SqliteType.Text);
                    command.Parameters.Add("@themeId", SqliteType.Integer);
                    command.Parameters.Add("@description", SqliteType.Text);
                    command.Parameters.Add("@fileName", SqliteType.Text);
                    command.Parameters.Add("@onslider", SqliteType.Integer);

                    command.Parameters["@title"].Value = painting.Title;
                    command.Parameters["@themeId"].Value = painting.ThemeId;
                    command.Parameters["@fileName"].Value = painting.Filename;
                    command.Parameters["@onslider"].Value = painting.OnSlider;
                    if (string.IsNullOrWhiteSpace(painting.Description))
                    {
                        command.Parameters["@description"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@description"].Value = painting.Description;
                    }

                    command.ExecuteNonQuery();
                });
            }
            else
            {
                this.Execute((command) =>
                {
                    command.CommandText = @"UPDATE Painting
SET Title =@title,
	ThemeId =@themeId,
	Description = @description,
	OnSlider = @onslider
WHERE Id = @id";
                    command.Parameters.Add("@id", SqliteType.Integer);
                    command.Parameters.Add("@title", SqliteType.Text);
                    command.Parameters.Add("@themeId", SqliteType.Integer);
                    command.Parameters.Add("@description", SqliteType.Text);
                    command.Parameters.Add("@onslider", SqliteType.Integer);

                    command.Parameters["@id"].Value = painting.Id;
                    command.Parameters["@title"].Value = painting.Title;
                    command.Parameters["@themeId"].Value = painting.ThemeId;
                    command.Parameters["@onslider"].Value = painting.OnSlider;
                    if (string.IsNullOrWhiteSpace(painting.Description))
                    {
                        command.Parameters["@description"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@description"].Value = painting.Description;
                    }

                    command.ExecuteNonQuery();
                });
            }
        }

        public void Delete(int id)
        {
            this.Execute((command) =>
            {
                command.CommandText = "DELETE FROM [Painting] WHERE Id = @id";

                command.Parameters.Add("@id", SqliteType.Integer);
                command.Parameters["@id"].Value = id;
                command.ExecuteNonQuery();
            });
        }

        private Painting FillPainting(SqliteDataReader reader)
        {
            Painting value = new Painting();

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

            index = reader.GetOrdinal("ThemeId");
            if (!reader.IsDBNull(index))
            {
                value.ThemeId = reader.GetInt32(index);
            }

            index = reader.GetOrdinal("ThemeTitle");
            if (!reader.IsDBNull(index))
            {
                value.ThemeTitle = reader.GetString(index);
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

            index = reader.GetOrdinal("OnSlider");
            if (!reader.IsDBNull(index))
            {
                value.OnSlider = reader.GetBoolean(index);
            }


            return value;
        }
    }
}

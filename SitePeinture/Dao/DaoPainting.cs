using Microsoft.Extensions.Configuration;
using SitePeinture.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

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
                command.CommandText = @"Select p.Id, p.Title, p.ThemeId, p.Filename, p.Description, p.OnSlider, t.Title ThemeTitle, p.Price, p.Available 
from Painting p 
inner join Theme t on t.Id = p.ThemeId";

                using (IDataReader reader = command.ExecuteReader())
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
                    command.CommandText = @"INSERT INTO Painting (Title, ThemeId, Description, Filename, OnSlider, Price, Available)
VALUES(@title, @themeId,@description, @fileName, @onslider, @price, @available)";

                    command.Parameters["@title"].Value = painting.Title;
                    command.Parameters["@themeId"].Value = painting.ThemeId;
                    command.Parameters["@fileName"].Value = painting.Filename;
                    command.Parameters["@onslider"].Value = painting.OnSlider;
                    command.Parameters["@price"].Value = painting.Price;
                    command.Parameters["@available"].Value = painting.Available;

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
	OnSlider = @onslider,
    Price = @price, 
    Available = @available
WHERE Id = @id";

                    command.Parameters["@id"].Value = painting.Id;
                    command.Parameters["@title"].Value = painting.Title;
                    command.Parameters["@themeId"].Value = painting.ThemeId;
                    command.Parameters["@onslider"].Value = painting.OnSlider;
                    command.Parameters["@price"].Value = painting.Price;
                    command.Parameters["@available"].Value = painting.Available;

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
                
                command.Parameters["@id"].Value = id;
                command.ExecuteNonQuery();
            });
        }

        public Painting Get(int id)
        {
            Painting result = null;

            this.Execute((command) =>
            {
                command.CommandText = @"SELECT p.Id, p.Title, p.ThemeId, p.Filename, p.Description, p.OnSlider, t.Title ThemeTitle, p.Price, p.Available 
FROM Painting p inner join Theme t on t.Id = p.ThemeId
WHERE p.Id = @id ";
                
                command.Parameters["@id"].Value = id;

                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = this.FillPainting(reader);
                    }
                }
            });

            return result;
        }


        private Painting FillPainting(IDataReader reader)
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

            index = reader.GetOrdinal("Price");
            if (!reader.IsDBNull(index))
            {
                value.Price = reader.GetInt32(index);
            }

            index = reader.GetOrdinal("Available");
            if (!reader.IsDBNull(index))
            {
                value.Available = reader.GetBoolean(index);
            }

            return value;
        }
    }
}

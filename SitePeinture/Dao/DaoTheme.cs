using Microsoft.Extensions.Configuration;
using SitePeinture.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
                command.CommandText = @"SELECT t1.Id, t1.ParentId,t1.Title, t1.Description, t2.Title AS ParentTitle, t1.WithText
FROM Theme AS t1
LEFT JOIN Theme AS t2 ON t1.ParentId = t2.Id";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(this.FillTheme(reader));
                    }
                }
            });

            return result;
        }

        public void Edit(Theme theme)
        {
            if (theme.IsNew)
            {
                // Add the theme 
                this.Execute((command) =>
                {
                    command.CommandText = "INSERT INTO [Theme] ([ParentId], [Title], [Description], [WithText]) VALUES(@parentId, @title,@description, @withText)";

                    command.Parameters["@title"].Value = theme.Title;

                    if (theme.HasParent)
                    {
                        command.Parameters["@parentId"].Value = theme.ParentId;
                    }
                    else
                    {
                        command.Parameters["@parentId"].Value = DBNull.Value;
                    }

                    if (string.IsNullOrWhiteSpace(theme.Description))
                    {
                        command.Parameters["@description"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@description"].Value = theme.Description;
                    }

                    command.Parameters["@withText"].Value = theme.WithText;

                    command.ExecuteNonQuery();
                });
            }
            else
            {
                // Update the exsiting theme
                this.Execute((command) =>
                {
                    command.CommandText = "UPDATE [Theme] SET [ParentId]=@parentId, [Title] = @title, [Description]= @description, [WithText]= @withText WHERE [Id]= @id";

                    command.Parameters["@id"].Value = theme.Id;
                    command.Parameters["@title"].Value = theme.Title;

                    if (theme.HasParent)
                    {
                        command.Parameters["@parentId"].Value = theme.ParentId;
                    }
                    else
                    {
                        command.Parameters["@parentId"].Value = DBNull.Value;
                    }

                    if (string.IsNullOrWhiteSpace(theme.Description))
                    {
                        command.Parameters["@description"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@description"].Value = theme.Description;
                    }

                    command.Parameters["@withText"].Value = theme.WithText;

                    command.ExecuteNonQuery();
                });
            }
        }

        public void Delete(int id)
        {
            this.Execute((command) =>
            {
                command.CommandText = "DELETE FROM [Theme] WHERE Id = @id";
                
                command.Parameters["@id"].Value = id;
                command.ExecuteNonQuery();
            });
        }

        private Theme FillTheme(IDataReader reader)
        {
            Theme value = new Theme();

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

            index = reader.GetOrdinal("ParentId");
            if (!reader.IsDBNull(index))
            {
                value.ParentId = reader.GetInt32(index);
            }

            index = reader.GetOrdinal("ParentTitle");
            if (!reader.IsDBNull(index))
            {
                value.ParentTitle = reader.GetString(index);
            }

            index = reader.GetOrdinal("WithText");
            if(!reader.IsDBNull(index))
            {
                value.WithText = reader.GetBoolean(index);
            }

            return value;
        }
    }
}

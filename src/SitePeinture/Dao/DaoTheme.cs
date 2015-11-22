using Microsoft.Extensions.Configuration;
using SitePeinture.Models;
using System;
using System.Collections.Generic;
using System.Data;
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

        public void Edit(Theme theme)
        {
            if (theme.IsNew)
            {
                // Add the theme 
                this.Execute((command) =>
                {
                    command.CommandText = "INSERT INTO [Theme] ([ParentId], [Title], [Description]) VALUES(@parentId, @title,@description)";
                    command.Parameters.Add("@parentId",  SqlDbType.Decimal);
                    command.Parameters.Add("@title", SqlDbType.Text);
                    command.Parameters.Add("@description", SqlDbType.Text);

                    command.Parameters["@title"].Value = theme.Title;

                    if (theme.HasParent)
                    {
                        command.Parameters["@parentId"].Value = theme.ParentId;
                    }
                    else
                    {
                        command.Parameters["@parentId"].Value = DBNull.Value;
                    }

                    if(string.IsNullOrWhiteSpace(theme.Description))
                    {
                        command.Parameters["@description"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters["@description"].Value = theme.Description;
                    }

                    command.ExecuteNonQuery();
                });
            }
            else
            {
                // Update the exsiting theme
                this.Execute((command) =>
                {
                    command.CommandText = "UPDATE [Theme] SET [ParentId]=@parentId, [Title] = @title, [Description]=@description WHERE [Id]= @id";
                    command.Parameters.Add("@id", SqlDbType.Decimal);
                    command.Parameters.Add("@parentId", SqlDbType.Decimal);
                    command.Parameters.Add("@title", SqlDbType.Text);
                    command.Parameters.Add("@description", SqlDbType.Text);
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

                    command.ExecuteNonQuery();
                });
            }
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

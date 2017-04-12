using Microsoft.Extensions.Configuration;
using SitePeinture.Dao;
using SitePeinture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture.Services
{
    public class ThemeServices
    {
        private DaoPainting paintingTable;
        private DaoTheme themeTable;

        public ThemeServices(DaoTheme themeTable)
        {
            this.themeTable = themeTable;
            this.paintingTable = new DaoPainting(themeTable.configuration);
        }

        public IEnumerable<Theme> GetAll()
        {
            // Gets all themes
            var allThemes = this.themeTable.GetAll();
            
            // Gets all painting
            List<Painting> allPainting = this.paintingTable.GetAll();

            // Change the value of HasChildren
            foreach (var theme in allThemes)
            {
                if (allPainting.Any(p => p.ThemeId == theme.Id))
                {
                    theme.HasChildrenPainting = true;
                }

                if (allThemes.Any(t => t.ParentId == theme.Id))
                {
                    theme.HasChildrenTheme = true;
                }
            }

            return allThemes;
        }

        public void Edit(Theme theme)
        {
            this.themeTable.Edit(theme);
        }

        public void Delete(int id)
        {
            this.themeTable.Delete(id);
        }

        public IEnumerable<Theme> GetParents(int id)
        {
            return this.GetAll().Where(e => e.HasParent == false && e.Id != id).ToArray();
        }
    }
}

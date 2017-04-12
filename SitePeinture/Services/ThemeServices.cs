using SitePeinture.Data;
using SitePeinture.Models;
using System.Collections.Generic;
using System.Linq;

namespace SitePeinture.Services
{
    public class ThemeServices
    {
        private ApplicationDbContext _context;

        public ThemeServices(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<Theme> GetAll()
        {
            // Gets all themes
            var allThemes = this._context.Themes.ToArray();
            
            // Gets all painting
            List<Painting> allPainting = this._context.Paintings.ToList();

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
            //this.themeTable.Edit(theme);
        }

        public void Delete(int id)
        {
            //this.themeTable.Delete(id);
        }

        public IEnumerable<Theme> GetParents(int id)
        {
            return this.GetAll().Where(e => e.HasParent == false && e.Id != id).ToArray();
        }
    }
}

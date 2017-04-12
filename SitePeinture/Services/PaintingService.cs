using SitePeinture.Data;
using SitePeinture.Models;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;

namespace SitePeinture.Services
{
    public class PaintingService
    {
        private readonly ApplicationDbContext _context;

        public PaintingService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Delete(int id)
        {
            Painting painting = this._context.Paintings.Where(p=> p.Id.Equals(id)).FirstOrDefault();
            this._context.Paintings.Remove(painting);
            this._context.SaveChanges();

            if (painting != null)
            {
                string path = Path.Combine("images", "tableaux", painting.Filename);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        internal Painting GetById(int id)
        {
            return this._context.Paintings.Where(p => p.Id.Equals(id)).FirstOrDefault();
        }

        internal void Edit(Painting value)
        {
        }

        internal IEnumerable<Painting> GetSliderPaints()
        {
            return this._context.Paintings.Where(p => p.OnSlider).ToArray();
        }

        internal IEnumerable<Painting> GetAll()
        {
            return this._context.Paintings.ToArray();
        }

        internal IEnumerable<Painting> GetPaintingByThemeId(int id)
        {
            return this._context.Paintings.Where(p => p.ThemeId.Equals(id)).ToArray();
        }
    }
}

using SitePeinture.Dao;
using SitePeinture.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture.Services
{
    public class PaintingService
    {
        private readonly DaoPainting paintingTable;

        public PaintingService(DaoPainting paintingTable)
        {
            this.paintingTable = paintingTable;
        }

        public void Delete(int id)
        {
            Painting painting = this.paintingTable.Get(id);
            this.paintingTable.Delete(id);

            if (paintingTable != null)
            {
                string path = Path.Combine("images", "tableaux", painting.Filename);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.DataAccess.Contracts.Domain
{
    public class Images
    {
        public int Img_Id { get; set; }
        public string Img_Title { get; set; }
        public byte[] Img_Data { get; set; }
        public string Img_Type { get; set; }
        public bool Img_Profil { get; set; }
        public bool Img_Offical { get; set; }
        public bool Img_sponsor { get; set; }

    }
}

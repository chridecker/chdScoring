using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace chdScoring.DataAccess.Contracts.Domain
{
    public class Country_Images
    {
        public int Img_Id { get; set; }
        public string Short { get; set; }
        public string Name { get; set; }
        public string Img_Type { get; set; }
        public byte[] Img_Data { get; set; }
    }
}

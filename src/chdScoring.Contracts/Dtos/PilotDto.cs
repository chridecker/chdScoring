using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class PilotDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Club { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public byte[] Image{ get; set; }
        public byte[] CountryImage { get; set; }
    }
}

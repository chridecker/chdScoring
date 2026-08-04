using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class ImportFileDto
    {
        public int Pilot { get; set; }
        public int Round { get; set; }
        public byte[] File { get; set; }
        public string Type { get; set; }
    }
}

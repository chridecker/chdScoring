using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class PrintPdfDto
    {
        public string Name { get; set; }
        public string Directory { get; set; }
        public DateTime CreationTime { get; set; }
    }
}

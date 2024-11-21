using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.PrintService.Dtos
{
    public class PrintDto
    {
        public string FilePath { get; set; }
        public string Printer { get; set; }
        public bool Landscape { get; set; }
    }
}

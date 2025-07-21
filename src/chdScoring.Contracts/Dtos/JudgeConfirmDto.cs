using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class JudgeConfirmDto
    {
        public int Teilnehmer { get; set; }
        public int Durchgang { get; set; }
        public int Judge { get; set; }
    }
}

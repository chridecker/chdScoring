﻿using chdScoring.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.DataAccess.Contracts.Domain
{
    public class Wettkampf_Leitung
    {
        public int Start { get; set; }
        public int Teilnehmer { get; set; }
        public int Durchgang { get; set; }
        public int Status { get; set; }
        public DateTime Start_Time { get; set; }
    }
}

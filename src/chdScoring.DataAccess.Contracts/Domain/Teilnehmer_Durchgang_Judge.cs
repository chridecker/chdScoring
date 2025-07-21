using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.DataAccess.Contracts.Domain
{
    public class Teilnehmer_Durchgang_Judge
    {
        public int Teilnehmer { get; set; }
        public int Durchgang { get; set; }
        public int Judge { get; set; }
        public DateTime Time { get; set; }
    }
}


//CREATE TABLE "teilnehmer_durchgang_judge" 
//( `teilnehmer` int(11) NOT NULL, `durchgang` int(11) NOT NULL, `judge` int(11) NOT NULL,  `time` datetime NOT NULL, PRIMARY KEY(`teilnehmer`,`durchgang`,`judge`,`time`) )
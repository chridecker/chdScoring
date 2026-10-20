using System;
using System.Collections.Generic;
using System.Text;
using chdScoring.Contracts.Enums;

namespace chdScoring.Contracts.Dtos
{
    public class StammdatenDto
    {
        public string TournamentName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Federation { get; set; }
        public int Jury { get; set; }
        public int Wettkampf_Leiter { get; set; }
        public int Org_Leiter { get; set; }
        public string Host { get; set; }
        public string HostWeb { get; set; }
        public int Img_Id { get; set; }
        public int Urkunde_Id { get; set; }
        public int Fed_Id { get; set; }
        public string Location { get; set; }
        public string Number { get; set; }
        public int Finale { get; set; }
        public int Final_Durchgang { get; set; }
        public int Final_Teilnehmer { get; set; }
        public EScoreMode Score_Mode { get; set; }
        public bool Judge_Pin { get; set; }
        public bool Edit { get; set; }
    }
}

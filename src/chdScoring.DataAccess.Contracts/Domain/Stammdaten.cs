using System;

namespace chdScoring.DataAccess.Contracts.Domain
{
    public class Stammdaten
    {
        public int Id { get; set; }
        public string Turnier { get; set; }
        public int Klasse { get; set; }
        public int Durchgaenge { get; set; }
        public string Federation { get; set; }
        public int Jury { get; set; }
        public int Wettkampf_Leiter { get; set; }
        public int Org_Leiter { get; set; }
        public string Veranstalter { get; set; }
        public string Veranstalter_web { get; set; }
        public int Img_Id { get; set; }
        public int Gruppe_Id { get; set; }
        public int Urkunde_Id { get; set; }
        public int Fed_Id { get; set; }
        public string Ort{ get; set; }
        public string Number{ get; set; }
        public DateTime Datum{ get; set; }
        public DateTime End_datum{ get; set; }
        public int Finale { get; set; }
        public int Final_Durchgang { get; set; }
        public int Final_Teilnehmer { get; set; }
        public bool Three_Panel_Final { get; set; }
        public int End_Finale { get; set; }
        public int End_Final_Teilnehmer { get; set; }
        public int End_Final_Durchgang{ get; set; }
        public int Score_Mode{ get; set; }
        public bool Judge_Pin{ get; set; }
        public bool Del_On{ get; set; }
        public bool Edit{ get; set; }
        public int Airfields{ get; set; }
    }
}

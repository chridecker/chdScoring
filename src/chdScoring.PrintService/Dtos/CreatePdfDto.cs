namespace chdScoring.PrintService.Dtos
{
    public class CreatePdfDto
    {
        public bool Landscape{ get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
    }
}

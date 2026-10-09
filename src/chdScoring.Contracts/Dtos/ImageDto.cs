using System;

namespace chdScoring.Contracts.Dtos
{
    public class ImageDto
    {
        public string Type { get; set; }
        public byte[] Data { get; set; }
        public string Src => string.IsNullOrWhiteSpace(Type) ? string.Empty : $"data:{this.Type};base64,{Convert.ToBase64String(this.Data)}";
    }
}

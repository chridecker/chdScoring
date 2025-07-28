using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Base64 { get; set; }
        public string ImageUrl => $"data:{this.Type};base64,{this.Base64}";
    }
}

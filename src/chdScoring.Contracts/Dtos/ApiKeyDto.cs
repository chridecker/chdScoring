using System;
using System.Collections.Generic;
using System.Text;
using chdScoring.Contracts.Enums;

namespace chdScoring.Contracts.Dtos
{
    public class ApiKeyDto
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public EUserRole Role { get; set; }
        public int? JudgeId { get; set; }
        public string Surname { get; set; }
        public string Lastname { get; set; }
    }
}

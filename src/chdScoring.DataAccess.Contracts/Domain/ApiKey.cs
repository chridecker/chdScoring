using System;
using System.Collections.Generic;
using System.Text;
using chdScoring.Contracts.Enums;

namespace chdScoring.DataAccess.Contracts.Domain
{
    public class ApiKey
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public EUserRole Role { get; set; }
        public int? JudgeId { get; set; }
        public string Surname { get; set; }
        public string Lastname { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class RoundDataDto
    {
        public RoundDto Round { get; set; }
        public PilotDto Pilot { get; set; }
        public IEnumerable<JudgeDto> Judges { get; set; }
        public IDictionary<int, IEnumerable<ManeouvreDto>> ManeouvreLst { get; set; } = new Dictionary<int, IEnumerable<ManeouvreDto>>();

    }
}

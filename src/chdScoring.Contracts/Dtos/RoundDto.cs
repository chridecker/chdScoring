﻿using System;

namespace chdScoring.Contracts.Dtos
{
    public class RoundDto
    {
        public int Id { get; set; }
        public string Program { get; set; }
        public TimeSpan Time { get; set; }
    }
}

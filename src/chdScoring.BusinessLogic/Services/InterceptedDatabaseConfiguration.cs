using chdScoring.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.BusinessLogic.Services
{
    public class InterceptedDatabaseConfiguration : IInterceptedDatabaseConfiguration
    {
        public string Name { get; set; }
    }
}

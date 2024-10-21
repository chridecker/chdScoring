using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Interfaces
{
    public interface IContextFactory<TContext>
    {
        public TContext Create();
    }
}

using chdScoring.Main.Contracts.Interfaces;

namespace chdScoring.Main.Contracts
{
    public interface IchdScoringService
    {
        IJudgeService Judge { get; }
        IScoringService Scoring { get; }
    }
}

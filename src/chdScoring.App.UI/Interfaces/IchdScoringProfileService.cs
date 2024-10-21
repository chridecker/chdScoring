using chd.UI.Base.Contracts.Interfaces.Authentication;
using chdScoring.Contracts.Dtos;

namespace chdScoring.App.UI.Interfaces
{
    public interface IchdScoringProfileService : IProfileService<int, int>
    {
        csUserDto? CsUser { get; }
    }

}

using chdScoring.Contracts.Enums;
using chd.UI.Base.Contracts.Dtos.Authentication;

namespace chdScoring.Contracts.Dtos
{
    public class csUserDto : UserDto<int, int>
    {
        public EUserRole Role { get; set; }
    }
}

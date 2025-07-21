using chd.UI.Base.Contracts.Enum;

namespace chdScoring.Web.Models
{
    public class CustomIconModel
    {
        public bool Show { get; set; } = true;
        public string FAClass { get; set; }
        public EIconStyle? Style { get; set; }
        public string StyleClass => Style switch
        {

            EIconStyle.Solid => "fa-solid",
            EIconStyle.Regular => "fa-regular",
            EIconStyle.Thin => "fa-thin",
            _ => string.Empty,
        };
    }
}

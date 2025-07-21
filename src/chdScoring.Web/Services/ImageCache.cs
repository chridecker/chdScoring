using chdScoring.Contracts.Dtos;
using System.Collections.Concurrent;

namespace chdScoring.Web.Services
{
    public class ImageCache
    {
        public ConcurrentDictionary<int, ImageDto> CountryImageCache { get; set; } = [];
    }
}

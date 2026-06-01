using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace chdScoring.Web.Pages
{
    public class ImagesModel : PageModel
    {
        public List<string> ImageUrls { get; set; } = new List<string>();

        public void OnGet()
        {
            var images = Directory.GetFiles("images").Select(s => new FileInfo(s)).Select(s => s.Name);
            this.ImageUrls = images.ToList();
        }
        public ContentResult OnGetBase64(string file)
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "images", file);

            // Read image bytes
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);

            // Convert to Base64
            return this.Content(Convert.ToBase64String(imageBytes));

            // Create HTML img tag
            //string imgTag = $"<img src=\"data:image/png;base64,{base64String}\" alt=\"Image\" />";
        }
    }
}

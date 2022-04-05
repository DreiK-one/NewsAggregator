using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewsAggregator.App.Models
{
    public class CreateOrEditArticleViewModel : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public string SourceUrl { get; set; }
        public string Image { get; set; }
        public DateTime CreationDate { get; set; }
        public float Coefficient { get; set; }

        public CategoryModel Category { get; set; }
        public SourceModel Source { get; set; }

        public IEnumerable<SelectListItem> Sources { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.App.Models
{
    public class SourceModel : BaseModel
    {
        [Required(AllowEmptyStrings = false)]
        [MinLength(4)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string BaseUrl { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string RssUrl { get; set; }
    }
}

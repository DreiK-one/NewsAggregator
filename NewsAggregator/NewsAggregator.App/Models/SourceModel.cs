using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.App.Models
{
    public class SourceModel : BaseModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field isn't be null")]
        [MinLength(4, ErrorMessage = "Minimum length of this field is 4")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This field isn't be null")]
        public string BaseUrl { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This field isn't be null")]
        public string RssUrl { get; set; }
    }
}

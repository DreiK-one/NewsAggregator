using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.App.Models
{
    public class CategoryModel : BaseModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field isn't be null")]
        [MinLength(4, ErrorMessage = "Minimum length of this field is 4")]
        public string Name { get; set;}
    }
}

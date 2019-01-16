using System.ComponentModel.DataAnnotations;

namespace API.Model.Classes
{
    public class ProductModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}

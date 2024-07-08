using System.ComponentModel.DataAnnotations;
namespace BRAND.Models
{
    public class Brand
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [Display(Name="Brand Name")]
        public string Name { get; set; }
        [Display(Name = "Estabilished year")]
        public int EstabilishedYear { get; set; }

        public string BrandLogo { get; set; }
    }
}

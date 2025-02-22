using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebStore9Domain.Entities;

namespace WebStore9Domain.ViewModels
{

    public class ProductViewModel : IValidatableObject
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Название не указано")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 200 символов")]
        public string Name { get; set; }

        [Display(Name = "Цена")]
        [Required(ErrorMessage = "Цена не указана")]
        public decimal Price { get; set; }

        [Display(Name = "Путь к картинке")]
        [Required(ErrorMessage = "Путь не указан")]
        public string ImageUrl { get; set; }

        [Display(Name = "Бренд")]
        public string BrandName { get; set; }

        public int? BrandId { get; set; }

        [Display(Name = "Секция")]
        [Required(ErrorMessage = "Секция не указана")]
        public string SectionName { get; set; }

        public int SectionId { get; set; }
        public int Order { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            switch (validationContext.MemberName)
            {
                default: return new[] { ValidationResult.Success };

                case nameof(Price):
                    if (Price <= 0)
                        return new[] { new ValidationResult("Некорректная цена", new[] { nameof(Price) }) };
                    return [ValidationResult.Success];
            }
        }
    }
}

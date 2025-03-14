using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebStore9Domain.ViewModels
{
    public class EmployeeViewModel : IValidatableObject
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Имя не указано")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 200 символов")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Неверный формат имени")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Фамилия не указана")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 200 символов")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Неверный формат фамилии")]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Возраст")]
        [Required(ErrorMessage = "Возраст не указан")]
        [Range(18, 80, ErrorMessage = "Возраст должен быть в пределах от 18 до 80 лет")]
        public int Age { get; set; }

        [Display(Name = "Стаж")]
        [Required(ErrorMessage = "Стаж не указан")]
        [Range(0, 10, ErrorMessage = "Стаж должен быть в пределах от 0 до 10")]
        public int Seniority { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            switch (validationContext.MemberName)
            {
                default: return new[] { ValidationResult.Success };

                case nameof(Age):
                    if (Age < 18 || Age > 80)
                        return new[] { new ValidationResult("Странный возраст", new[] { nameof(Age) }) };
                    return [ValidationResult.Success];
            }
        }
    }
}

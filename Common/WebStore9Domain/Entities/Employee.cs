using Microsoft.EntityFrameworkCore;
using WebStore9Domain.Entities.Base.Interfaces;

namespace WebStore9Domain.Entities
{
    /// <summary> Информация о сотруднике </summary>
    [Index(nameof(FirstName), nameof(LastName))]
    public class Employee : IEntity
    {
        /// <summary> Идентификатор сотрудника </summary>
        public int Id { get; set; }
        /// <summary> Имя сотрудника </summary>
        public string FirstName { get; set; }
        /// <summary> Фамилия сотрудника </summary>
        public string LastName { get; set; }
        /// <summary> Отчество сотрудника </summary>
        public string? Patronymic { get; set; }
        /// <summary> Возраст сотрудника </summary>
        public int Age { get; set; }
        /// <summary> Опыт сотрудника </summary>
        public int Seniority { get; set; }

        public override string ToString()
        {
            return $"[{Id}]{LastName} {FirstName} {Patronymic} ({Age}, {Seniority})";
        }
    }
}

using Microsoft.EntityFrameworkCore;
using WebStore9Domain.Entities.Base.Interfaces;

namespace WebStore9Domain.Entities
{
    [Index(nameof(FirstName), nameof(LastName))]
    public class Employee : IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Patronymic { get; set; }
        public int Age { get; set; }
        public int Seniority { get; set; }

    }
}

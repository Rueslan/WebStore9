using System.ComponentModel.DataAnnotations;
using WebStore9Domain.Entities.Base.Interfaces;

namespace WebStore9Domain.Entities.Base
{
    public abstract class NamedEntity : Entity, INamedEntity
    {
        [Required]
        public string Name { get; set; }
    }

}

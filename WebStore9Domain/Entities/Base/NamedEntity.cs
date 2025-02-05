using WebStore9Domain.Entities.Base.Interfaces;

namespace WebStore9Domain.Entities.Base
{
    public abstract class NamedEntity : Entity, INamedEntity
    {
        public string Name { get; set; }
    }

}

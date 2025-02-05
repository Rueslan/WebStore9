using WebStore9Domain.Entities.Base.Interfaces;

namespace WebStore9Domain.Entities.Base
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
    }

}

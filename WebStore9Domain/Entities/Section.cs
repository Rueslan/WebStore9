using WebStore9Domain.Entities.Base;
using WebStore9Domain.Entities.Base.Interfaces;

namespace WebStore9Domain.Entities
{
    public class Section : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }

        public int? ParentId { get; set; }
    }
}

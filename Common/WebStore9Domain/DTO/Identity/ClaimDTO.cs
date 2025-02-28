using System.Security.Claims;

namespace WebStore9Domain.DTO.Identity
{
    public abstract class ClaimDTO : UserDTO
    {
        public IEnumerable<Claim> Claims { get; set; }
    }

    public class AddClaimDTO : ClaimDTO { }

    public class RemoveClaimDTO : ClaimDTO { }

    public class ReplaceClaimDTO : UserDTO
    {
        public Claim Claim { get; set; }

        public Claim NewClaim { get; set; }
    }
}

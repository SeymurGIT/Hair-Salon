using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace HairSalon.Infrastructure
{
    public class UserValidator : IUserValidator<IdentityUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            if (user.Email.ToLower().EndsWith("@gmail.com") || user.Email.ToLower().EndsWith("@mail.ru"))
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError()
                {
                    Code = "EmailDomainError",
                    Description = "Sadəcə gmail və email icazə verilir"
                }));
            }
        }
    }
}

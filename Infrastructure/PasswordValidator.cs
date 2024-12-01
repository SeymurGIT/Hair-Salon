using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HairSalon.Infrastructure
{
    public class PasswordValidator : IPasswordValidator<IdentityUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user, string password)
        {
            List<IdentityError> identity_errors = new List<IdentityError>();

            if(password.ToLower().Contains(user.UserName.ToLower()))
            {
                identity_errors.Add(new IdentityError()
                {
                    Code="Password_Identical_Username",
                    Description="Şifrə istifadəçi adı ilə eynidir"
                });
            }
            if(password.Contains("123"))
            {
                identity_errors.Add(new IdentityError()
                {
                    Code="Password_Has_Simple_Sequence",
                    Description="Şifrə sadə və sıralanmış ədədləri təşkil edir"
                });
            }
                return Task.FromResult(identity_errors.Count==0 ?
                IdentityResult.Success : IdentityResult.Failed(identity_errors.ToArray() ));
        }
    }
}

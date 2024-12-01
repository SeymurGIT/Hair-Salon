using HairSalon.Data;
using HairSalon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HairSalon.TagHelpers
{
    [HtmlTargetElement("td", Attributes="asp-role-users")]
    public class RoleUsersTagHelper : TagHelper
    {   
        private readonly AppDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RoleUsersTagHelper(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }
        [HtmlAttributeName("asp-role-users")]
        public string RoleId { get; set;} = null!; //boş olmur

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            
                var userNames = new List<string>();
                var role = await _roleManager.FindByIdAsync(RoleId);

                if (role != null && role.Name != null)
                {
                        var users = await _userManager.Users.ToListAsync();

                    foreach (var user in users)
                    {
                        if (await _userManager.IsInRoleAsync(user, role.Name))
                        {
                            userNames.Add(user.UserName ?? "");
                        }
                    }

                    output.Content.SetHtmlContent(userNames.Count == 0 ? "İstifadəçi yoxdur" : SetHtml(userNames));
                }
        }

        private string SetHtml(List<string> userNames)
        {
            var html = "<ul>";

            foreach (var item in userNames)
            {
                html += "<li>" + item + "</li>"; // Fixed the missing closing angle bracket here
            }
            html += "</ul>";

            return html;
        }
    }   
}

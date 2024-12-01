using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable enable

namespace HairSalon.ViewModel
{
    public class EditViewModel
    {

            public IList<string>? SelectedRoles { get; set; }
            public string? Id { get; set; }
            public string? FullName { get; set; }
      
            [EmailAddress(ErrorMessage = "Doğru e-poçt ünvanı qeyd edin")]
            public string? Email { get; set; }

            [DataType(DataType.Password)]
            public string? Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Şifrə uyğun gəlmir!")]
            public string? ConfirmPassword { get; set; }

            

        }
    }

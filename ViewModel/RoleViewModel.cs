using System.ComponentModel.DataAnnotations;

namespace HairSalon.ViewModel
{
    public class RoleViewModel
    {
        public string RoleId { get; set; }
        [Required(ErrorMessage = "Bu sahə boş ola bilməz")]
        [StringLength(20)]
        public string RoleName { get; set; }
    }
}

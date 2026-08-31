using System.ComponentModel.DataAnnotations;

namespace Pathway.ViewModels.Admin
{
    public class EditUserViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "الاسم لازم يكون بين 2 و100 حرف")]
        [Display(Name = "الاسم")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "الإيميل مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة الإيميل غير صحيحة")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "الدور مطلوب")]
        [Display(Name = "الدور")]
        public string Role { get; set; } = string.Empty;
    }
}
using System.ComponentModel.DataAnnotations;

namespace Pathway.ViewModels
{
    public class UpdateProgressViewModel
    {
        [Required]
        [Range(0, 100, ErrorMessage = "Progress must be between 0 and 100")]
        public int Progress { get; set; }
    }
}

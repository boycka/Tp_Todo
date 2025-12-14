using System.ComponentModel.DataAnnotations;

namespace Tp_TODO.ViewModels
{
    public class UserVM
    {
        [Required(ErrorMessage ="email est obligatoire")]
        [EmailAddress]
        public string Login { get; set; }

        [Required(ErrorMessage ="Password obligatoire")]
        [DataType(DataType.Password)]

        public string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using Tp_TODO.Enums;

namespace Tp_TODO.ViewModels
{
    public class TodoVm
    {
        [Required(ErrorMessage = "Libelle est obligatoir")]
        public string Libelle { get; set; }
        [Required(ErrorMessage = "Description est obligatoir")]

        public string Description { get; set; }
        [Required(ErrorMessage = "state est obligatoir")]

        public State State { get; set; }
        [Required(ErrorMessage = "dateLimite est obligatoir")]
        [DataType(DataType.Date)]

        public DateOnly DateLimite { get; set; }
    }
}

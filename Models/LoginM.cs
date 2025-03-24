using System.ComponentModel.DataAnnotations;

namespace API_Patient_Managerment.Models
{
    public class LoginM
    {
            [Required(ErrorMessage = "*")]
            [MaxLength(20)]
            public string UserName { get; set; } = null!;

            [Required(ErrorMessage = "*")]
            //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[\S]{8,50}$", ErrorMessage = "*")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = null!;
    }
}

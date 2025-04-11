using System.ComponentModel.DataAnnotations;

namespace API_Patient_Managerment.Models
{
    public class PatientCreateDTO
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string gender { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        public string phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string email { get; set; }

        public string address { get; set; }

        public bool deleted { get; set; } = false;
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API_Patient_Managerment.Models
{
    public partial class PatientDTO
    {
        [BindNever]
        public string _id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; } 
        public string gender { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        [BindNever]
        public string createdBy { get; set; }
        [BindNever]
        public DateTime createdAt { get; set; }
        public Boolean deleted { get; set; }
    }
}

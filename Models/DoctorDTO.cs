namespace API_Patient_Managerment.Models
{
    public partial class DoctorDTO
    {
        public string _id { get; set; }
        public string firstname { get; set; } = string.Empty;
        public string lastname { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public DateTime dob { get; set; }
        public string specialty { get; set; } = string.Empty;
        public string phone { get; set; }
        public string address { get; set; }
        public string image { get; set; } = string.Empty;
    }
}

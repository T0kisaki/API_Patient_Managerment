namespace API_Patient_Managerment.Models
{
    public partial class PatientDTO
    {
        public string _id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; } 
        public string gender { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public DateTime createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public Boolean deleted { get; set; }
    }
}

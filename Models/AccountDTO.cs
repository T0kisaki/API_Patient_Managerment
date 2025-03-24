namespace API_Patient_Managerment.Models
{
    public partial class AccountDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        //public string? Password { get; set; }
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int Status { get; set; }
    }
}

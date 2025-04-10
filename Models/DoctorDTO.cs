namespace API_Patient_Managerment.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

public partial class DoctorDTO
{
    public string? _id { get; set; }
    public string firstname { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public DateTime dob { get; set; }
    public string specialty { get; set; } = string.Empty;
    public string phone { get; set; }
    public string address { get; set; }

    [NotMapped] // tránh lưu trường này nếu bạn dùng Entity Framework
    public IFormFile? image { get; set; }

    // Nếu bạn vẫn cần lưu đường dẫn image sau khi upload
    public string imagePath { get; set; } = string.Empty;
}

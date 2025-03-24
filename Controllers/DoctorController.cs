using API_Patient_Managerment.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API_Patient_Managerment.Controllers
{
    public class DoctorController : Controller
    {
        Uri baseAddress = new Uri("https://patient-records-management-api.vercel.app");
        private readonly HttpClient _client;

        public DoctorController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        // Lấy danh sách bác sĩ
        private async Task<List<DoctorDTO>> GetDoctorList()
        {
            List<DoctorDTO> doclist = new List<DoctorDTO>();
            using (var response = await _client.GetAsync("/api/doctor/getlist?page=1&limit=99"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    doclist = JsonConvert.DeserializeObject<List<DoctorDTO>>(data);
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to retrieve doctor list!";
                }
            }
            return doclist;
        }

        // Lấy bác sĩ theo ID
        private async Task<DoctorDTO?> GetDocById(string id)
        {
            using (var response = await _client.GetAsync($"/api/doctor/getById/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var postData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<DoctorDTO>(postData);
                }
            }
            return null;
        }

        // Danh sách bác sĩ
        public async Task<IActionResult> List_Doctor()
        {
            var doclist = await GetDoctorList();
            return View(doclist);
        }

        // Tạo hoặc cập nhật bác sĩ (GET)
        [HttpGet]
        public async Task<IActionResult> Upsert(string id = null)
        {
            // Lấy danh sách bác sĩ cho dropdown (nếu cần)
            ViewBag.CategoryList = await GetDoctorList();

            if (!string.IsNullOrEmpty(id)) // Chỉnh sửa bác sĩ
            {
                var doctor = await GetDocById(id);
                if (doctor == null)
                {
                    TempData["ErrorMessage"] = "Failed to retrieve doctor!";
                    return RedirectToAction("List_Doctor");
                }
                return View(doctor);
            }

            return View(); // Tạo bác sĩ mới
        }

        // Tạo hoặc cập nhật bác sĩ (POST)
        [HttpPost]
        public async Task<IActionResult> Upsert(DoctorDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryList = await GetDoctorList();
                return View(model);
            }

            bool isCreate = string.IsNullOrEmpty(model._id);
            HttpResponseMessage response;

            if (isCreate)
            {
                response = await _client.PostAsJsonAsync("/api/doctor/create", model);
            }
            else
            {
                response = await _client.PutAsJsonAsync("/api/doctor/edit", model);
            }

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = isCreate ? "Failed to create doctor!" : "Failed to update doctor!";
                ViewBag.CategoryList = await GetDoctorList();
                return View(model);
            }

            TempData["SuccessMessage"] = isCreate ? "Doctor created successfully!" : "Doctor updated successfully!";
            return RedirectToAction("List_Doctor");
        }
    }
}

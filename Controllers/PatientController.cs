using API_Patient_Managerment.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API_Patient_Managerment.Controllers
{
    public class PatientController : Controller
    {
        Uri baseAddress = new Uri("https://patient-records-management-api.vercel.app");
        private readonly HttpClient _client;

        public PatientController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<PatientDTO> data = new List<PatientDTO>();
            HttpResponseMessage response = await _client.GetAsync("/api/patient/getlist?page=1&limit=100");

            if (response.IsSuccessStatusCode)
            {
                data = await response.Content.ReadFromJsonAsync<List<PatientDTO>>() ?? new List<PatientDTO>();
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to retrieve patient list!";
            }

            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Invalid patient id.";
                return RedirectToAction("Index");
            }

            // Gọi API để lấy thông tin bệnh nhân
            HttpResponseMessage response = await _client.GetAsync($"/api/patient/getbyid/{id}");
            if (response.IsSuccessStatusCode)
            {
                var patient = await response.Content.ReadFromJsonAsync<PatientDTO>();
                if (patient != null)
                {
                    return View(patient);
                }
                else
                {
                    TempData["ErrorMessage"] = "Patient not found.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to retrieve patient details!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, PatientDTO updatedPatient)
        {
            if (string.IsNullOrEmpty(id) || updatedPatient == null)
            {
                TempData["ErrorMessage"] = "Invalid data submitted.";
                return RedirectToAction("Index");
            }

            // Tạo PATCH request tới API
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/patient/edit/{id}")
            {
                Content = JsonContent.Create(updatedPatient)
            };

            HttpResponseMessage response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Patient updated successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update patient.";
                return View(updatedPatient);
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new PatientDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientDTO newPatient)
        {
            // Kiểm tra ModelState trước khi gửi dữ liệu
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data submitted.";
                return View(newPatient);
            }

            try
            {
                // Gửi request POST đến API NodeJS để tạo mới patient
                HttpResponseMessage response = await _client.PostAsJsonAsync("/api/patient/create", newPatient);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Patient created successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Đọc lỗi từ API để debug
                    string errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Failed to create patient. Error: {errorContent}";
                    return View(newPatient);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Exception occurred: " + ex.Message;
                return View(newPatient);
            }
        }

    }
}

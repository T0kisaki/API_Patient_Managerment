using System.Net.Http.Json;
using API_Patient_Managerment.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Patient_Managerment.Controllers
{
    public class PatientController : Controller
    {
        // Cấu hình BaseAddress của API NodeJS
        private readonly HttpClient _client;
        private readonly Uri _baseAddress = new Uri("https://patient-records-management-api.up.railway.app");

        public PatientController()
        {
            _client = new HttpClient { BaseAddress = _baseAddress };
        }

        // GET: Patient/Index
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

        // GET: Patient/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Invalid patient id.";
                return RedirectToAction("Index");
            }

            HttpResponseMessage response = await _client.GetAsync($"/api/patient/getbyid/{id}");
            if (response.IsSuccessStatusCode)
            {
                var patient = await response.Content.ReadFromJsonAsync<PatientDTO>();
                if (patient != null)
                    return View(patient);
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

        // POST: Patient/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, PatientDTO updatedPatient)
        {
            if (string.IsNullOrEmpty(id) || updatedPatient == null)
            {
                TempData["ErrorMessage"] = "Invalid data submitted.";
                return RedirectToAction("Index");
            }

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

        // GET: Patient/Create
        [HttpGet]
        public IActionResult Create()
        {
            // Trả về view với model PatientDTO (chỉ các field cần nhập)
            return View(new PatientDTO());
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientDTO newPatient)
        {
            // Bỏ qua lỗi của _id nếu có
            if (ModelState.ContainsKey("_id"))
            {
                ModelState.Remove("_id");
            }
            // Kiểm tra tính hợp lệ của model
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data submitted.";
                return View(newPatient);
            }

            // Bỏ đi các trường không cần thiết khi tạo mới (API tự tạo _id, createdAt, createdBy)
            newPatient._id = null;
            newPatient.createdAt = default;
            newPatient.createdBy = null;

            try
            {
                // Gửi request POST đến API tạo mới patient
                HttpResponseMessage response = await _client.PostAsJsonAsync("/api/patient/create", newPatient);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Patient created successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = "API error: " + errorContent;
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

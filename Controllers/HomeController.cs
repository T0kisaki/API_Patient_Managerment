using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using API_Patient_Managerment.Models;
using System.Numerics;

namespace API_Patient_Managerment.Controllers;

public class HomeController : Controller
{
    Uri baseAddress = new Uri("https://patient-records-management-api.up.railway.app");
    private readonly HttpClient _client;
    public HomeController()
    {
        _client = new HttpClient();
        _client.BaseAddress = baseAddress;
    }

    public async Task<IActionResult> Index()
    {
        List<Doctor> doctors = new List<Doctor>();
        HttpResponseMessage response = await _client.GetAsync("/api/doctor/getlist?page=1&limit=4");

        if (response.IsSuccessStatusCode)
        {
            doctors = await response.Content.ReadFromJsonAsync<List<Doctor>>();
        }
        return View(doctors);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

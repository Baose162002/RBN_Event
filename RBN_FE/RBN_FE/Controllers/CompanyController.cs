using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RBN_FE.Controllers
{
    public class CompanyController : Controller
    {
        private static string pathAPI = "http://localhost:5250/api/";
        public async Task<IActionResult> CompanyIndex()
        {
            var data = new List<Company>();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync(pathAPI + "Company"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<List<Company>>(content);

                            if (result != null)
                            {
                                data = result; // Sử dụng dữ liệu đã deserialize
                                return View(data); // Trả về View với Model là danh sách Company
                            }
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Request error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
            }
            // Trả về View rỗng nếu không có dữ liệu
            return View(data);
        }
    }
}

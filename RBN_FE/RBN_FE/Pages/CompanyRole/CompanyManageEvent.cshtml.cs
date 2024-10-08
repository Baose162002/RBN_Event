using BusinessObject.Dto;
using BusinessObject;
using BusinessObject.Dto.ResponseDto;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace RBN_FE.Pages.CompanyRole
{
    public class CompanyManageEventModel : PageModel
    {

        public CompanyDTO Company { get; set; }
        public List<EventDTO> Event { get; set; }
        private static string APIPort = "http://localhost:5250/api/";

        [BindProperty(SupportsGet = true)]

        public int? EventId { get; set; }
        public async Task<IActionResult> OnPostChangeStatusAsync()
        {
            if (EventId == null)
            {
                ModelState.AddModelError("", "EventId không hợp lệ.");
                return Page();
            }

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Gọi API để thay đổi trạng thái booking
                    var apiUrl = $"{APIPort}Event/change-status-event/{EventId.Value}";
                    using (var response = await httpClient.PutAsync(apiUrl, null))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SuccessMessage"] = "Trạng thái booking đã được cập nhật thành công.";
                        }
                        else
                        {
                            ModelState.AddModelError("", "Không thể thay đổi trạng thái booking.");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError("", $"Lỗi khi gửi yêu cầu: {ex.Message}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Lỗi không mong đợi: {ex.Message}");
                }
            }


            return RedirectToPage("/CompanyRole/CompanyManageEvent");
        }

        public async Task OnGetAsync(int companyId)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var userIdStr = HttpContext.Session.GetString("UserId");

                    if (int.TryParse(userIdStr, out int userId))
                    {
                        using (var response = await httpClient.GetAsync(APIPort + "Company/user/" + userId))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var content = await response.Content.ReadAsStringAsync();
                                var result = JsonConvert.DeserializeObject<CompanyDTO>(content);

                                if (result != null)
                                {
                                    Company = result;
                                }
                            }
                        }

                        // Kiểm tra nếu Company không null, sau đó lấy feedback dựa trên Company.Id
                        if (Company != null)
                        {
                            using (var response = await httpClient.GetAsync(APIPort + "Event/get-event-by-company/" + Company.Id))
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    var content = await response.Content.ReadAsStringAsync();
                                    var result = JsonConvert.DeserializeObject<List<EventDTO>>(content);

                                    if (result != null)
                                    {
                                        Event = result;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("UserId không hợp lệ hoặc không tồn tại trong session.");
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
        }
    }
}


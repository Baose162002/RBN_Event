using BusinessObject.Dto;
using BusinessObject;
using BusinessObject.Dto.ResponseDto;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using CloudinaryDotNet;

namespace RBN_FE.Pages.CompanyRole
{
    public class CompanyManageEventModel : PageModel
    {
        //Paging
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 8;
        public int TotalPages { get; set; }

        public CompanyDTO Company { get; set; }
        public List<EventDTO> Event { get; set; }
        private static string APIPort = "http://localhost:5250/api/";

        [BindProperty(SupportsGet = true)]
        public int? EventId { get; set; }


        [BindProperty(SupportsGet = true)]
        public int? SearchId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchEventName { get; set; }
        [BindProperty(SupportsGet = true)]
        public double? SearchPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SearchStatus { get; set; }
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
                        using (var companyResponse = await httpClient.GetAsync(APIPort + "Company/user/" + userId))
                        {
                            if (companyResponse.IsSuccessStatusCode)
                            {
                                var content = await companyResponse.Content.ReadAsStringAsync();
                                var result = JsonConvert.DeserializeObject<CompanyDTO>(content);

                                if (result != null)
                                {
                                    Company = result;
                                }
                            }
                        }
                    }
                    
                        var queryParameters = new List<string>();

                        if (SearchId.HasValue)
                            queryParameters.Add($"id={SearchId.Value}");

                        if (!string.IsNullOrEmpty(SearchEventName))
                            queryParameters.Add($"name={SearchEventName.ToString()}");

                        if (SearchPrice.HasValue)
                            queryParameters.Add($"price={SearchPrice.Value}");

                        if (SearchStatus.HasValue)
                            queryParameters.Add($"status={SearchStatus.Value}");

                        var queryString = queryParameters.Any() ? "?" + string.Join("&", queryParameters) : "";

                        var apiUrl = $"{APIPort}Event/search/{Company.Id}{queryString}";

                        var response = await httpClient.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            Event = JsonConvert.DeserializeObject<List<EventDTO>>(content) ?? new List<EventDTO>();
                            TotalPages = (int)Math.Ceiling((double)Event.Count() / PageSize);
                            Event = Event.OrderByDescending(f => f.CreateAt)
                        .Skip((PageIndex - 1) * PageSize)
                        .Take(PageSize)
                        .ToList();
                        }
                        else
                        {
                            Event = new List<EventDTO>();
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


using BusinessObject.Dto.ResponseDto;
using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using BusinessObject.DTO;
using BusinessObject.Dto;
using System.ComponentModel.Design;

namespace RBN_FE.Pages.CompanyRole
{
    public class CompanyManageFeedbackModel : PageModel
    {
        private static readonly string APIPort = "http://localhost:5250/api/";

        public List<FeedbackDTO> FeedBack { get; set; }
        public CompanyDTO Company { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 8;
        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SearchId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateOnly? SearchDate { get; set; }


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
                    using (var response = await httpClient.GetAsync(APIPort + "Feedback/get-feedback-by-company/" + Company.Id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<List<FeedbackDTO>>(content);

                            if (result != null)
                            {
                                        FeedBack = result;
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


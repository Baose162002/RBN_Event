using BusinessObject;
using BusinessObject.Dto;
using BusinessObject.DTO;
using BusinessObject.DTO.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace RBN_FE.Pages.CompanyPages
{
    public class CompanyDetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CompanyDetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public ListCompanyDTO Company { get; set; }
        public List<FeedbackDTO> FeedBack { get; set; }
        public List<ResponseDTO> Response { get; set; }
        public string ErrorMessage { get; set; } // Thêm thuộc tính để hiển thị lỗi

        public async Task OnGetAsync(int companyId)
        {
            try
            {
                Company = await _httpClient.GetFromJsonAsync<ListCompanyDTO>($"http://localhost:5250/api/Company/{companyId}");
                FeedBack = await _httpClient.GetFromJsonAsync<List<FeedbackDTO>>($"http://localhost:5250/api/Feedback");
                Response = await _httpClient.GetFromJsonAsync<List<ResponseDTO>>($"http://localhost:5250/api/Response");

                if (Company == null)
                {
                    ErrorMessage = "Không có thông tin công ty."; 
                }
                if (FeedBack == null)
                {
                    Console.WriteLine("Chưa có phản hồi");
                }
            }
            catch (HttpRequestException e)
            {
                ErrorMessage = $"Lỗi khi gọi API: {e.Message}";
            }
            catch (JsonException e)
            {
                ErrorMessage = $"Lỗi khi parse JSON: {e.Message}";
            }
            catch (Exception e)
            {
                ErrorMessage = $"Lỗi không xác định: {e.Message}";
            }
        }
    }
}

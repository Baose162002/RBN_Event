using BusinessObject;
using BusinessObject.Dto;
using BusinessObject.DTO;
using BusinessObject.DTO.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RBN_FE.Pages.LogIn_Out;
using System.ComponentModel.Design;
using System.Security.Claims;
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
        // Pagination properties
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        public int TotalPages { get; set; }
        public ListCompanyDTO Company { get; set; }
        public List<FeedbackDTO> FeedBack { get; set; }
        public List<ResponseDTO> Response { get; set; }
        public Dictionary<int, string> UserNames { get; set; } // Để lưu trữ UserId và UserName
        public string ErrorMessage { get; set; } 
        [BindProperty]
        public string FeedbackContent { get; set; }


        public async Task OnGetAsync(int companyId)
        {
            try
            {
                Company = await _httpClient.GetFromJsonAsync<ListCompanyDTO>($"https://rbnapi20241031155156.azurewebsites.net/api/Company/{companyId}"); 
                Response = await _httpClient.GetFromJsonAsync<List<ResponseDTO>>($"https://rbnapi20241031155156.azurewebsites.net/api/Response");
                var allFeedbacks = await _httpClient.GetFromJsonAsync<List<FeedbackDTO>>($"https://rbnapi20241031155156.azurewebsites.net/api/Feedback/get-feedback-by-company/{companyId}");
                
                    // Calculate total pages
                    TotalPages = (int)Math.Ceiling(allFeedbacks.Count / (double)PageSize);

                    // Paginate feedbacks
                    FeedBack = allFeedbacks
                        .OrderByDescending(f => f.FeedbackDate)
                        .Skip((PageIndex - 1) * PageSize)
                        .Take(PageSize)
                        .ToList();
                // Fetch all responses for the company
                Response = await _httpClient.GetFromJsonAsync<List<ResponseDTO>>($"https://rbnapi20241031155156.azurewebsites.net/api/Response?companyId={companyId}");
                if (Response == null)
                {
                    Response = new List<ResponseDTO>();
                }
                if (Company == null)
                {
                    ErrorMessage = "Không có thông tin công ty.";
                }
                if (FeedBack == null)
                {
                    Console.WriteLine("Chưa có phản hồi");
                }
                if (Response == null)
                {
                    Console.WriteLine("Chưa có phản hồi");
                }
                // Khởi tạo dictionary để lưu trữ UserId và UserName
                UserNames = new Dictionary<int, string>();

                if (FeedBack != null && FeedBack.Count > 0)
                {
                    // Lấy danh sách các UserId duy nhất từ phản hồi
                    var userIds = FeedBack.Select(f => f.UserId).Distinct().ToList();

                    foreach (var userId in userIds)
                    {
                        // Gọi API để lấy thông tin người dùng
                        var user = await _httpClient.GetFromJsonAsync<UserDTO>($"https://rbnapi20241031155156.azurewebsites.net/api/User/get-username/{userId}");
                        if (user != null)
                        {
                            UserNames.Add(userId, user.Name);
                        }
                        else
                        {
                            UserNames.Add(userId, "Không xác định");
                        }
                    }
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
        public async Task<IActionResult> OnPostSubmitFeedbackAsync(int companyId)
        {
            if (string.IsNullOrWhiteSpace(FeedbackContent))
            {
                ModelState.AddModelError(string.Empty, "Nội dung đánh giá không được để trống.");
                await OnGetAsync(companyId);
                return Page();
            }
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                ErrorMessage = "Bạn cần đăng nhập để gửi đánh giá.";
                await OnGetAsync(companyId);
                return Page();
            }

            if (!int.TryParse(userIdStr, out int userId))
            {
                ErrorMessage = "Lỗi khi lấy UserId từ session.";
                await OnGetAsync(companyId);
                return Page();
            }

            var newFeedback = new FeedbackDTO
            {
                Comment = FeedbackContent,
                FeedbackDate = DateTime.Now,
                UserId = userId,
                CompanyId = companyId
            };
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://rbnapi20241031155156.azurewebsites.net/api/Feedback", newFeedback);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage(new { companyId = companyId });
                }
                else
                {
                    ErrorMessage = "Gửi đánh giá không thành công.";
                }
            }
            catch (Exception e)
            {
                ErrorMessage = $"Lỗi không xác định: {e.Message}";
            }

            await OnGetAsync(companyId);
            return Page();
        }

        // Phương thức để xử lý POST tạo Response (được gọi qua AJAX)
        public class CreateResponseRequest
        {
            public int CompanyId { get; set; }
            public int FeedbackId { get; set; }
            public string Comment { get; set; }
        }

        public async Task<IActionResult> OnPostCreateResponseAsync()
        {
            try
            {
                // Đọc JSON từ request body
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                var data = JsonSerializer.Deserialize<CreateResponseRequest>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (data == null)
                {
                    return BadRequest("Dữ liệu không hợp lệ.");
                }

                if (string.IsNullOrEmpty(data.Comment))
                {
                    return BadRequest("Phản hồi không được để trống.");
                }

                // Tạo ResponseDTO
                var newResponse = new ResponseDTO
                {
                    Comment = data.Comment,
                    ResponseDate = DateTime.UtcNow,
                    FeedBackId = data.FeedbackId,
                    CompanyId = data.CompanyId
                };

                // Gửi POST request tới API để tạo Response
                var apiResponse = await _httpClient.PostAsJsonAsync("https://rbnapi20241031155156.azurewebsites.net/api/Response", newResponse);

                if (apiResponse.IsSuccessStatusCode)
                {
                    var createdResponse = await apiResponse.Content.ReadFromJsonAsync<ResponseDTO>();
                    // Lấy tên công ty từ Company đối tượng đã được lấy ở OnGetAsync
                    string companyName = Company?.Name ;

                    return new JsonResult(new { companyName, createdResponse });
                }
                else
                {
                    var error = await apiResponse.Content.ReadAsStringAsync();
                    return BadRequest($"Lỗi khi tạo phản hồi: {apiResponse.StatusCode} - {error}");
                }
            }
            catch (HttpRequestException e)
            {
                return StatusCode(500, $"Lỗi khi gọi API: {e.Message}");
            }
            catch (Exception e)
            {
                ErrorMessage = $"Lỗi không xác định: {e.Message}";
            }

            return Page();
        }
    }
}



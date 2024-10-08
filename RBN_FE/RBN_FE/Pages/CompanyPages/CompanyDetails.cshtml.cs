using BusinessObject;
using BusinessObject.Dto;
using BusinessObject.DTO;
using BusinessObject.DTO.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.Design;
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
        public string ErrorMessage { get; set; } // Thêm thuộc tính để hiển thị lỗi
        [BindProperty]
        public string FeedbackContent { get; set; }
        public async Task OnGetAsync(int companyId)
        {
            try
            {
                Company = await _httpClient.GetFromJsonAsync<ListCompanyDTO>($"http://localhost:5250/api/Company/{companyId}"); Response = await _httpClient.GetFromJsonAsync<List<ResponseDTO>>($"http://localhost:5250/api/Response");
                var allFeedbacks = await _httpClient.GetFromJsonAsync<List<FeedbackDTO>>($"http://localhost:5250/api/Feedback?companyId={companyId}");
                if (allFeedbacks == null)
                {
                    FeedBack = new List<FeedbackDTO>();
                }
                else
                {
                    // Calculate total pages
                    TotalPages = (int)Math.Ceiling(allFeedbacks.Count / (double)PageSize);

                    // Paginate feedbacks
                    FeedBack = allFeedbacks
                        .OrderByDescending(f => f.FeedbackDate)
                        .Skip((PageIndex - 1) * PageSize)
                        .Take(PageSize)
                        .ToList();
                }
                // Fetch all responses for the company
                Response = await _httpClient.GetFromJsonAsync<List<ResponseDTO>>($"http://localhost:5250/api/Response?companyId={companyId}");
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
                        var user = await _httpClient.GetFromJsonAsync<UserDTO>($"http://localhost:5250/api/User/get-username/{userId}");
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

            var newFeedback = new FeedbackDTO
            {
                Comment = FeedbackContent,
                FeedbackDate = DateTime.Now,
                UserId = 1, // Giả sử UserId là 1, bạn cần thay đổi theo cách xác thực người dùng
                CompanyId = companyId
            };
            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://localhost:5250/api/Feedback", newFeedback);
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
        public async Task<IActionResult> OnPostCreateResponseAsync(int companyId, int feedbackId, string comment)
        {
            if (string.IsNullOrEmpty(comment))
            {
                ErrorMessage = "Phản hồi không được để trống.";
                return Page();
            }

            try
            {
                // Tạo một đối tượng ResponseDTO mới
                var response = new ResponseDTO
                {
                    Comment = comment,
                    ResponseDate = DateTime.UtcNow,
                    FeedBackId = feedbackId,
                    CompanyId = companyId
                };

                // Gửi POST request tới API để tạo Response
                var responseResult = await _httpClient.PostAsJsonAsync("http://localhost:5250/api/Response", response);

                if (responseResult.IsSuccessStatusCode)
                {
                    // Tải lại thông tin của Company, Feedback, và Response sau khi thêm thành công
                    await OnGetAsync(companyId);
                }
                else
                {
                    ErrorMessage = $"Lỗi khi tạo phản hồi: {responseResult.StatusCode} - {responseResult.ReasonPhrase}";
                }
            }
            catch (HttpRequestException e)
            {
                ErrorMessage = $"Lỗi khi gọi API: {e.Message}";
            }
            catch (Exception e)
            {
                ErrorMessage = $"Lỗi không xác định: {e.Message}";
            }

            return Page();
        }
    }
}



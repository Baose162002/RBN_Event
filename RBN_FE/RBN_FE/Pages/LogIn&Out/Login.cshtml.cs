using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using BusinessObject.Dto.RequestDto;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace RBN_FE.Pages.LogIn_Out
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        public LoginUserRequest LoginRequest { get; set; }

        public string ErrorMessage { get; set; }

        public LoginModel(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<LoginModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var loginUrl = $"{baseUrl}/auth/login";

            _logger.LogInformation($"Attempting to login at URL: {loginUrl}");

            try
            {
                var response = await client.PostAsJsonAsync(loginUrl, LoginRequest);
                var content = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"Response status code: {response.StatusCode}");
                _logger.LogInformation($"Response content: {content}");

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var loginResult = JsonSerializer.Deserialize<LoginResponse>(content, options);
                    if (loginResult != null)
                    {
                        HttpContext.Session.SetString("JWTToken", loginResult.Token);
                        HttpContext.Session.SetString("UserRole", loginResult.Role);
                        HttpContext.Session.SetString("TokenExpiration", loginResult.Expiration.ToString());
                        HttpContext.Session.SetString("UserId", loginResult.Id.ToString());


                        // Extract user name from the token (this is just an example, adjust as needed)
                        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadToken(loginResult.Token) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;
                        var userName = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

                        if (!string.IsNullOrEmpty(userName))
                        {
                            HttpContext.Session.SetString("UserName", userName);
                        }

                        _logger.LogInformation($"User with role {loginResult.Role} logged in successfully");

                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        _logger.LogWarning("Login result is null");
                        ErrorMessage = "Invalid login response from server";
                    }
                }
                else
                {
                    _logger.LogWarning($"Login failed. Status code: {response.StatusCode}");
                    ErrorMessage = $"Login failed. Status code: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login");
                ErrorMessage = "An error occurred during login. Please try again later.";
            }

            ModelState.AddModelError(string.Empty, ErrorMessage ?? "Invalid login attempt.");
            return Page();
        }
    }

    public class LoginResponse
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public DateTime Expiration { get; set; }
    }
}
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BusinessObject.Dto.ResponseDto;

namespace RBN_FE.Pages.Admin
{
    public class UserManagementModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public List<UserResponseDto> Users { get; set; }

        public UserManagementModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var response = await client.GetAsync($"{baseUrl}/User");

            if (response.IsSuccessStatusCode)
            {
                Users = await response.Content.ReadFromJsonAsync<List<UserResponseDto>>();
            }
            else
            {
                // Handle error - maybe set an error message to display on the page
                Users = new List<UserResponseDto>();
            }
        }
    }
}
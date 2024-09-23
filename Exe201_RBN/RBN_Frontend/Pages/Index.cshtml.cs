using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace RBN_Frontend.Pages
{
    public class IndexModel : PageModel
    {
        public List<Event> Events { get; set; }

        public async Task OnGet()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5250");
                var response = await client.GetAsync("api/Event");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    Events = JsonConvert.DeserializeObject<List<Event>>(jsonString);
                }
                else
                {
                    Events = new List<Event>(); // Handle no events found case
                }
            }
        }
    }

    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string EventType { get; set; }
        public string Price { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int CompanyId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
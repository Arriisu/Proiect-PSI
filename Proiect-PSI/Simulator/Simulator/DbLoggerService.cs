using System.Net.Http.Json;

namespace Simulator
{
    public class DbLoggerService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DbLoggerService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task LogActionAsync(StareSistem stare, string actionDescription)
        {
            var client = _httpClientFactory.CreateClient();
            var payload = new
            {
                State = stare,
                ActionDescription = actionDescription,
                AlarmActive = stare.IsAlarmActive
            };

            try
            {
                await client.PostAsJsonAsync("http://localhost:5138/api/statelog", payload);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR] Nu s-a putut salva în baza de date: {ex.Message}");
            }
        }
    }
}
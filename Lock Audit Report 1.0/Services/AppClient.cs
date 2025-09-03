using Lock_Audit_Report_1._0.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lock_Audit_Report_1._0.Services
{
    public class ApiClient
    {
        private readonly AppConfig _config;
        private readonly HttpClient _httpClient;

        public ApiClient(AppConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_config.ServerURL)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Username", _config.Username);
            _httpClient.DefaultRequestHeaders.Add("Password", _config.Password);
        }

        public async Task<List<OnlineEvent>> GetRoomEventsAsync(string roomNumber, DateTime start, DateTime end)
        {
            if (string.IsNullOrWhiteSpace(roomNumber))
                throw new ArgumentException("Room number cannot be empty.", nameof(roomNumber));

            var payload = new
            {
                roomNumber,
                startDate = start.ToString("yyyy-MM-ddTHH:mm:ss"),
                endDate = end.ToString("yyyy-MM-ddTHH:mm:ss")
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            try
            {
                response = await _httpClient.PostAsync("/api/room/events", content);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Could not connect to server. Please check your network or server settings.", ex);
            }

            if (!response.IsSuccessStatusCode)
                throw new Exception($"API returned error: {response.StatusCode} - {response.ReasonPhrase}");

            var json = await response.Content.ReadAsStringAsync();

            // Deserialize into list of OnlineEvent
            var events = JsonConvert.DeserializeObject<List<OnlineEvent>>(json);

            return events ?? new List<OnlineEvent>();
        }
    }
}

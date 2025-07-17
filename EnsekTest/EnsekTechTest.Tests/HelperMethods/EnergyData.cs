using EnsekTechTest.TestData.POCOS;
using System.Text.Json;

namespace EnsekTechTest.Tests.HelperMethods
{
    public class EnergyData
    {
        public static async Task<Energy> GetAllEnergy()
        {
            HttpClient _httpClient = new HttpClient();
            HttpResponseMessage response2 = await _httpClient.GetAsync("https://qacandidatetest.ensek.io/ENSEK/energy");
            response2.EnsureSuccessStatusCode();

            string content = await response2.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Energy>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }

        public static async Task<IList<RecentOrder>> GetAllOrders()
        {
            HttpClient _httpClient = new HttpClient();
            HttpResponseMessage response2 = await _httpClient.GetAsync("https://qacandidatetest.ensek.io/ENSEK/orders");
            response2.EnsureSuccessStatusCode();

            string content = await response2.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IList<RecentOrder>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }
    }
}
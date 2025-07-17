using EnsekTechTest.TestData.POCOS;
using System.Net.Http.Headers;
using System.Text;

namespace EnsekTechTest.Extensions
{
    public static class EnskEnergy
    {
        public static async Task<bool> ResetEnergyRecords(this Login validLogin, string tokenEndpoint, string endpoint)
        {
            var token = await validLogin.GetToken(tokenEndpoint);

            using HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );

            var content = new StringContent(string.Empty);

            HttpResponseMessage response = await client.PostAsync("https://qacandidatetest.ensek.io/ENSEK/reset", content);
            string responseBody = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            return true;
        }

        public static async Task<bool> PurchaseEnergy(this Login validLogin, string tokenEndpoint, string endpoint, Order order)
        {
            var token = await validLogin.GetToken(tokenEndpoint);

            using HttpClient client = new HttpClient();

            // Set headers
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
                );

            // JSON payload
            var json = $@"{{
            ""id"": ""{order.Id}"",
            ""quantity"": {order.Quantity},
            ""energy_id"": {order.EnergyId}
        }}";

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send PUT request
            HttpResponseMessage response = await client.PutAsync($"https://qacandidatetest.ensek.io/ENSEK/orders/{order.EnergyId}", content);

            // Read response
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
            response.EnsureSuccessStatusCode();

            return true;
        }
    }
}

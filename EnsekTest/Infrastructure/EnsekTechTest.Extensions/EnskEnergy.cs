using EnsekTechTest.TestData.POCOS;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

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

        public static async Task<(string PurchaseId, string Quantity)> PurchaseEnergy(this Login validLogin, string tokenEndpoint, string endpoint, Order order)
        {
            var token = await validLogin.GetToken(tokenEndpoint);
            var url = $"https://qacandidatetest.ensek.io/ENSEK/buy/{order.EnergyId}/{order.Quantity}";

            using HttpClient client = new HttpClient();

            // Set headers
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
                );

            // Send PUT request
            HttpResponseMessage response = await client.PutAsync(url, null);
            response.EnsureSuccessStatusCode();

            // Read response
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(responseBody);
            var message = json["message"].ToString() ?? string.Empty;

            //this should be an extension method
            var splitMessage = message.Split(' ');
            var purchaseId = splitMessage[^1];
            var quantity = splitMessage[3];
            return (purchaseId.Replace(".", ""), quantity);
        }
    }
}

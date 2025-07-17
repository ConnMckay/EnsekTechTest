using EnsekTechTest.Abstractions;
using EnsekTechTest.Abstractions.Errors;
using EnsekTechTest.Extensions;
using EnsekTechTest.TestData;
using EnsekTechTest.TestData.POCOS;
using EnsekTechTest.Tests.HelperMethods;
using FluentAssertions;
using Xunit;

namespace EnsekTechTest.Tests
{
    public class ApiTests
    {
        const string valid_Username = "test";
        const string invalid_Username = "invalid";
        const string password = "testing";

        [Fact]
        public async Task Test_login_is_invalid()
        {
            Login invalidLogin = new()
            {
                Username = invalid_Username,
                Password = password
            };
            string tokenEndpoint = "https://qacandidatetest.ensek.io/ENSEK/login";
            (OutcomeResult Outcome, HttpResponseMessage httpResponse) response = await invalidLogin.ValidateLogin(tokenEndpoint);

            response.Outcome.Should().Be(HttpCodeErrors.UnauthorisedLogin);
        }

        [Fact]
        public async Task Test_login_is_valid()
        {
            Login validLogin = new()
            {
                Username = valid_Username,
                Password = password
            };
            string tokenEndpoint = "https://qacandidatetest.ensek.io/ENSEK/login";
            (OutcomeResult Outcome, HttpResponseMessage httpResponse) response = await validLogin.ValidateLogin(tokenEndpoint);

            response.Outcome.Should().Be(OutcomeResult.Success());
        }

        [Fact]
        public async Task Validate_energy_records()
        {
            Energy expectedResults = AllEnergyData.GetEnergyData();
            Energy energyOnRecord = await EnergyData.GetAllEnergy();

            energyOnRecord.Should().BeEquivalentTo(expectedResults);
        }
        [Fact]
        public async Task Reset_energy_records()
        {
            Login validLogin = new()
            {
                Username = valid_Username,
                Password = password
            };
            string resetEndpoint = "https://qacandidatetest.ensek.io/ENSEK/reset";
            string tokenEndpoint = "https://qacandidatetest.ensek.io/ENSEK/login";
            Energy expectedResults = AllEnergyData.GetEnergyData();

            bool success = await validLogin.ResetEnergyRecords(tokenEndpoint, resetEndpoint);
            success.Should().BeTrue();

            Energy energyOnRecord = await EnergyData.GetAllEnergy();
            energyOnRecord.Should().BeEquivalentTo(expectedResults);
        }
        [Fact]
        public async Task Validate_list_of_recent_orders()
        {
            IList<RecentOrder> orders = await EnergyData.GetAllOrders();
            orders.Should().NotBeNull();
        }

        [Theory]
        [InlineData("Electricity_1", 5, 3)]
        [InlineData("Gas_1", 1, 1)]
        [InlineData("Oil_1", 2, 4)]
        public async Task Purchase_energy_and_validate_purchase_has_been_debited(string id, int quantity, int energyId)
        {
            Order order = new Order(id, quantity, energyId);

            Login validLogin = new()
            {
                Username = valid_Username,
                Password = password
            };
            string ordersEndpoint = "https://qacandidatetest.ensek.io/ENSEK/orders";
            string tokenEndpoint = "https://qacandidatetest.ensek.io/ENSEK/login";
            Energy expectedResults = AllEnergyData.GetEnergyData();

            bool success = await validLogin.PurchaseEnergy(tokenEndpoint, ordersEndpoint, order);
            success.Should().BeTrue();

            Energy energyOnRecord = await EnergyData.GetAllEnergy();

            var baseElectricity = expectedResults.Electric;
            var newElectricrty = energyOnRecord.Electric;

            baseElectricity.quantity_of_units.Should().Be(newElectricrty.quantity_of_units + order.Quantity);

            IList<RecentOrder> orders = await EnergyData.GetAllOrders();

            orders.Any(x => x.Equals(order)).Should().BeTrue();

            string resetEndpoint = "https://qacandidatetest.ensek.io/ENSEK/reset";
            success = await validLogin.ResetEnergyRecords(tokenEndpoint, resetEndpoint);
            success.Should().BeTrue();

            energyOnRecord = await EnergyData.GetAllEnergy();
            energyOnRecord.Should().BeEquivalentTo(expectedResults);
        }
    }
}

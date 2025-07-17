using EnsektechTest.Fixtures;
using EnsekTechTest.Abstractions;
using EnsekTechTest.Abstractions.Errors;
using EnsekTechTest.Extensions;
using EnsekTechTest.TestData;
using EnsekTechTest.TestData.POCOS;
using EnsekTechTest.Tests.HelperMethods;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Xunit;

namespace EnsekTechTest.Tests
{
    public class ApiTests : IClassFixture<ConfigurationFixture>
    {
        public ApiTests(ConfigurationFixture configurationFixture)
        {
            _logger = ConfigurationFixture.Logger($"{GetType().Name}");
        }

        // please note i would normally have all usernames, passwords, uri etc.. in an apsettings file or push them in from
        // a runner as environmental variables

        // TODO in the purchase method, Nuclear does not have any quantity so would have a separate test as would need to
        // create quantity to purchase

        // Bug found on the message generated for purchasing of Gas. Asked for quantity of 1 and got 2999
        // This issue looks to be with Gas returning the quantity left instead of quantity purchased

        // There are 5 orders before today, but 1 has an invalid date


        readonly ILogger _logger;
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

            response.Outcome.IsFailure.Should().BeTrue($"Expected {HttpCodeErrors.UnauthorisedLogin} but was successful");
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

            response.Outcome.IsSuccess.Should().BeTrue(response.Outcome.IsError.Description);
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
        [InlineData("Electricity_1", 20, 3)]
        [InlineData("Gas_1", 4, 1)]
        [InlineData("Oil_1", 5, 4)]
        public async Task Purchase_energy_and_validate_purchase_has_been_debited(string id, int quantity, int energyId)
        {
            _logger.LogInformation($"Running Purchase energy tests for Id : {id} with quantity of : {quantity} using Energy Id : {energyId}");
            Order order = new Order(id, quantity, energyId);

            Login validLogin = new()
            {
                Username = valid_Username,
                Password = password
            };
            string ordersEndpoint = "https://qacandidatetest.ensek.io/ENSEK/orders";
            string tokenEndpoint = "https://qacandidatetest.ensek.io/ENSEK/login";
            Energy expectedResults = AllEnergyData.GetEnergyData();

            (string PurchaseId, string Quantity) purchasedEnergy = await validLogin.PurchaseEnergy(tokenEndpoint, ordersEndpoint, order);
            purchasedEnergy.PurchaseId.Should().NotBeNullOrEmpty();

            bool isAnInt = int.TryParse(purchasedEnergy.Quantity, out int intQuantity);

            isAnInt.Should().BeTrue($"returned value {purchasedEnergy.Quantity} is not of type int");


            IList<RecentOrder> orders = await EnergyData.GetAllOrders();

            orders.Any(x => x.Id.Equals(purchasedEnergy.PurchaseId)).Should().BeTrue();

            RecentOrder justPurchased = orders.First(x => x.Id.Equals(purchasedEnergy.PurchaseId));
            using (new AssertionScope())
            {
                justPurchased.Id.Should().Be(purchasedEnergy.PurchaseId);
                justPurchased.Quantity.Should().Be(intQuantity);
            }


            string resetEndpoint = "https://qacandidatetest.ensek.io/ENSEK/reset";
            bool success = await validLogin.ResetEnergyRecords(tokenEndpoint, resetEndpoint);
            success.Should().BeTrue();

            Energy energyOnRecord = await EnergyData.GetAllEnergy();
            energyOnRecord.Should().BeEquivalentTo(expectedResults);
        }

        [Theory]
        [InlineData(4, 1)]
        public async Task Confirm_the_number_of_orders_before_today(int expectedOrders, int invalidOrders)
        {
            IList<RecentOrder> orders = await EnergyData.GetAllOrders();
            DateTime yesterday = DateTime.UtcNow.AddDays(-1);

            int ordersBeforeToday = 0;
            int ordersInvalidOrAfterToday = 0;

            foreach (var order in orders)
            {
                DateTime orderDate = order.Time.ToDateTime();
                if (orderDate < yesterday)
                {
                    ordersBeforeToday++;
                }
                else
                {
                    ordersInvalidOrAfterToday++;
                }
            }

            using (new AssertionScope())
            {
                ordersBeforeToday.Should().Be(expectedOrders);
                ordersInvalidOrAfterToday.Should().Be(invalidOrders);
            }

        }
    }
}

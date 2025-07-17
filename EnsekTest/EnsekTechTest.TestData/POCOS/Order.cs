namespace EnsekTechTest.TestData.POCOS
{
    public class Order
    {
        public Order(string id, int quantity, int energyId)
        {
            Id = id;
            Quantity = quantity;
            EnergyId = energyId;
        }
        public string Id { get; set; }
        public int Quantity { get; set; }
        public int EnergyId { get; set; }
    }
}

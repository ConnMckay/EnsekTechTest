using EnsekTechTest.TestData.POCOS;

namespace EnsekTechTest.TestData
{
    public class AllEnergyData
    {
        public static Energy GetEnergyData()
        {
            Electric electric = new()
            {
                energy_id = 3,
                price_per_unit = 0.47f,
                quantity_of_units = 4322,
                unit_type = "kWh"
            };
            Gas gas = new()
            {
                energy_id = 1,
                price_per_unit = 0.34f,
                quantity_of_units = 3000,
                unit_type = "m³"
            };
            Nuclear nuclear = new()
            {
                energy_id = 2,
                price_per_unit = 0.56f,
                quantity_of_units = 0,
                unit_type = "MW"
            };
            Oil oil = new()
            {
                energy_id = 4,
                price_per_unit = 0.5f,
                quantity_of_units = 20,
                unit_type = "Litres"
            };
            Energy energy = new()
            {
                Electric = electric,
                Gas = gas,
                Nuclear = nuclear,
                Oil = oil
            };
            return energy;
        }
    }
}

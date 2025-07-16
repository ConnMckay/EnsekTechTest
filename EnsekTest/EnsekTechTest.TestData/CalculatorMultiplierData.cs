using Xunit;

namespace EnsekTechTest.TestData;

public class CalculatorMultiplierData : TheoryData<int, int, int>
{
    public CalculatorMultiplierData()
    {
        AddRow(3, 3, 9);
    }
}

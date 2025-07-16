using Xunit;

namespace EnsekTechTest.TestData;

public class CalculatorDivisionData : TheoryData<int, int, int>
{
    public CalculatorDivisionData()
    {
        AddRow(6, 0, 2);
    }
}

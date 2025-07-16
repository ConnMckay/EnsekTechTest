using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsekTechTest.Abstractions.Errors;

public static class CalculatorErrors
{
    public static readonly IsError DivideByZero = 
        new IsError("Invalid Divisor - You attempted to divide by zero");
}

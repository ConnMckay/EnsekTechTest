using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsekTechTest.Abstractions.Errors;

public static class HttpCodeErrors
{
    public static readonly IsError BadRequest = 
        new IsError("Http Bad Request - The Request returned a 400 Bad Request");
}

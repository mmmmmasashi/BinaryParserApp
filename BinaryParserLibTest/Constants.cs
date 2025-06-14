using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserLibTest;

internal static class Constants
{
    internal static readonly string TestRootPath = "./TestData";

    internal static string GetPathOf(string path)
    {
        return System.IO.Path.Combine(TestRootPath, path);
    }
}

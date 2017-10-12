using System;
using System.Collections.Generic;
using System.Linq;

namespace OpCodeGenerator
{
  class Program
  {
    static IEnumerable<string> ReadKnownOpCodes()
    {
      yield break;
    }

    static IEnumerable<string> GenerateOpCodes()
    {
      yield break;
    }

    static void Main(string[] args)
    {
      var known = ReadKnownOpCodes().ToArray();
      var gen = GenerateOpCodes().ToArray();
      int count = Math.Min(known.Length, gen.Length);
      for (int i = 0; i < count; i++)
      {
        Console.WriteLine("  ok: " + gen[i]);
        if (gen[i] != known[i])
        {
          Console.WriteLine();
          Console.WriteLine("Different found:");
          Console.WriteLine("  [known] " + known[i]);
          Console.WriteLine("    [gen] " + gen[i]);
          break;
        }
      }
      Console.WriteLine();
      if (known.Length < gen.Length) Console.WriteLine("Missing known-opcodes: {0:N0}", gen.Length - known.Length);
      if (gen.Length < known.Length) Console.WriteLine("Missing generated-opcodes: {0:N0}", known.Length - gen.Length);

      if (Environment.CommandLine.Contains(".vshost.exe"))
      {
        Console.Write("Press any key to continue . . . ");
        Console.ReadKey(true);
      }
    }
  }
}

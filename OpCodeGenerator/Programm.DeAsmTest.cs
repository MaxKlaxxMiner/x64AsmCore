#region # using *.*
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpCodeGenerator.DeAsm;

// ReSharper disable ConvertToConstant.Local
// ReSharper disable RedundantAssignment
#endregion

namespace OpCodeGenerator
{
  static partial class Program
  {
    static void DeAsmTest()
    {
      byte[] testOpcodes =
      {
        0x06, // ??? (invalid code)
        0xcc, // int3
        0x00, 0xeb, // add bl, ch
      };

      uint ofs = 0;
      while (ofs < testOpcodes.Length)
      {
        var res = DeAsm64.Decode(testOpcodes, ofs);
        Console.WriteLine(res.ToString());
        ofs += res.Length;
      }

      // --- auf Tastendruck warten, falls mit VS im Debug-Modus gestartet wurde (Fenster würde sich sonst sofort schließen) ---
      Console.WriteLine();
      if (Environment.CommandLine.Contains(".vshost.exe"))
      {
        Console.Write("Press any key to continue . . . ");
        Console.ReadKey(true);
      }
    }
  }
}

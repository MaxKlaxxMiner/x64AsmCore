#region # using *.*
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
// ReSharper disable ConvertToConstant.Local
// ReSharper disable RedundantAssignment
#endregion

namespace OpCodeGenerator
{
  class Program
  {
    /// <summary>
    /// liest alle bekannten, von Hand geschriebenen OpCodes aus Textdateien herraus und gibt diese zurück
    /// </summary>
    /// <returns>Enumerable der Zeilen, welche gelesen wurden</returns>
    static IEnumerable<string> ReadKnownOpCodes()
    {
      string[] files = { "00", "01", "02", "03", "04-07", "08", "09", "0a", "0b", "0c-0e", "0f00" };
      foreach (var file in files)
      {
        foreach (var line in File.ReadLines("KnownOpCodes/" + file + ".txt")) if (!string.IsNullOrWhiteSpace(line) && line.Trim()[0] != '#') yield return line.Trim();
      }
    }

    /// <summary>
    /// maximale Anzahl der Vorschau-Zeilen, falls eine Liste länger sein sollte
    /// </summary>
    const int SamplePreview = 10;

    const int StartView = 52500;

    static void Main()
    {
      //TextHelper.SwapOperands("KnownOpCodes/01.txt", "tmp.txt");
      //TextHelper.ReplaceInstrucion("KnownOpCodes/03.txt", "tmp.txt", "or");
      //TextHelper.ReplaceFirstOpcodes("tmp.txt", "tmp2.txt", "0b");
      //TextHelper.ReplaceFirstChars("KnownOpCodes/0f00.txt", "tmp.txt", "0f 00 ", 3);
      //return;

      var known = ReadKnownOpCodes().ToArray();
      var gen = Generator.GenerateOpCodes().ToArray();
      int count = Math.Min(known.Length, gen.Length);

      // --- bekannte OpCodes mit den generierten Version vergleichen und beim ersten Fehler stoppen
      for (int i = 0; i < count; i++)
      {
        if (i >= StartView) Console.WriteLine("  " + i.ToString("N0") + ": " + gen[i]);
        if (gen[i] != known[i])
        {
          Console.WriteLine();
          Console.WriteLine("Different found:");
          Console.WriteLine();
          Console.WriteLine("  [known] " + known[i]);
          Console.WriteLine("    [gen] " + gen[i]);
          break;
        }
      }
      Console.WriteLine();

      // --- Vorschau von eventuell Fehlenden OpCodes ---
      if (known.Length < gen.Length)
      {
        Console.WriteLine("Missing known-opcodes: {0:N0}", gen.Length - known.Length);
        Console.WriteLine();
        for (int i = count; i < Math.Min(count + SamplePreview, gen.Length); i++)
        {
          Console.WriteLine("  gen: " + gen[i]);
        }
        if (gen.Length > SamplePreview) Console.WriteLine("  gen: ...");
        Console.WriteLine();
      }
      if (gen.Length < known.Length)
      {
        Console.WriteLine("Missing generated-opcodes: {0:N0}", known.Length - gen.Length);
        Console.WriteLine();
        for (int i = count; i < Math.Min(count + SamplePreview, known.Length); i++)
        {
          Console.WriteLine("  known: " + known[i]);
        }
        if (known.Length > SamplePreview) Console.WriteLine("  known: ...");
        Console.WriteLine();
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

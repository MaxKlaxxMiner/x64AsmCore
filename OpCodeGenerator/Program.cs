#region # using *.*
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
// ReSharper disable ConvertToConstant.Local
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
      foreach (var line in File.ReadLines("KnownOpCodes/0000-.txt")) if (!string.IsNullOrWhiteSpace(line) && line.Trim()[0] != '#') yield return line.Trim();
    }

    /// <summary>
    /// gibt eine bestimmte Anzahl von Bytes in hexadezimale Schreibweise zurück und endet mit einem Bindestrich (z.B. "00 04 f4 - ")
    /// </summary>
    /// <param name="bytes">Bytes, welche ausgelesen werden sollen</param>
    /// <param name="count">Anzahl der Bytes, welche ausgelesen werden sollen</param>
    /// <returns></returns>
    static string StrB(byte[] bytes, int count)
    {
      return string.Join(" ", bytes.Take(count).Select(b => b.ToString("x2"))) + " - ";
    }

    /// <summary>
    /// gibt eine 64-Bit Adressierung zurück mit Leerzeichen am Anfang (z.B. " [rsi]")
    /// </summary>
    /// <param name="index">Index auf den Register (z.B. 2: "rdx")</param>
    /// <returns>fertige Adressierung</returns>
    static string R64Addr(int index)
    {
      return " [" + Asm.RegistersR64[index] + "]";
    }

    /// <summary>
    /// gibt einen 8-Bit Register zurück mit Leerzeichen am Anfang (z.B. " al")
    /// </summary>
    /// <param name="index">index auf den Register (z.B. 2: "dl")</param>
    /// <returns>fertiger Register</returns>
    static string R8(int index)
    {
      return " " + Asm.RegistersR8[index];
    }

    /// <summary>
    /// generiert alle OpCodes anhand bestimmter Regeln
    /// </summary>
    /// <returns>Enumerable der Zeilen, welche generiert wurden</returns>
    static IEnumerable<string> GenerateOpCodes()
    {
      var opCode = new byte[2];
      int opCodeLen = 2;

      for (int i = 0; i < 4; i++)
      {
        yield return StrB(opCode, opCodeLen) + Asm.Instructions[0] + R64Addr(i) + ',' + R8(0);
        opCode[opCodeLen - 1]++;
      }
    }

    /// <summary>
    /// maximale Anzahl der Vorschau-Zeilen, falls eine Liste länger sein sollte
    /// </summary>
    const int SamplePreview = 10;

    static void Main(string[] args)
    {
      var known = ReadKnownOpCodes().ToArray();
      var gen = GenerateOpCodes().ToArray();
      int count = Math.Min(known.Length, gen.Length);

      // --- bekannte OpCodes mit den generierten Version vergleichen und beim ersten Fehler stoppen
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

      // --- Vorschau von eventuell Fehlenden OpCodes ---
      if (known.Length < gen.Length)
      {
        Console.WriteLine("Missing known-opcodes: {0:N0}", gen.Length - known.Length);
        Console.WriteLine();
        for (int i = count; i < Math.Min(SamplePreview, known.Length); i++)
        {
          Console.WriteLine("  gen: " + known[i]);
        }
        if (known.Length > SamplePreview) Console.WriteLine("  gen: ...");
        Console.WriteLine();
      }
      if (gen.Length < known.Length)
      {
        Console.WriteLine("Missing generated-opcodes: {0:N0}", known.Length - gen.Length);
        Console.WriteLine();
        for (int i = count; i < Math.Min(SamplePreview, known.Length); i++)
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

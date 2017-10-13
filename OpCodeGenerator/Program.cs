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
      foreach (var line in File.ReadLines("KnownOpCodes/0000-.txt")) if (!string.IsNullOrWhiteSpace(line) && line.Trim()[0] != '#') yield return line.Trim();
    }

    #region # // --- Hilfmethoden für die Darstellung ---
    /// <summary>
    /// gibt eine bestimmte Anzahl von Bytes in hexadezimale Schreibweise zurück und endet mit einem Bindestrich (z.B. "00 04 f4 - ")
    /// </summary>
    /// <param name="bytes">Bytes, welche ausgelesen werden sollen</param>
    /// <param name="lastByte">zeig auf das letzte Byte in der Kette, welche ausgelesen werden sollen</param>
    /// <param name="constBytes">optionale zusätzliche Bytes für eine direkte Konstante</param>
    /// <returns>fertige Zeichenkette</returns>
    static string StrB(byte[] bytes, int lastByte, int constBytes = 0)
    {
      return string.Join(" ", bytes.Take(lastByte + 1).Select(b => b.ToString("x2")).Concat(Enumerable.Range(0, constBytes).Select(i => "xx"))) + " - ";
    }

    /// <summary>
    /// gibt eine 64-Bit Adressierung zurück, mit Leerzeichen am Anfang (z.B. " [rsi]")
    /// </summary>
    /// <param name="index">Index auf den Register (z.B. 2: "rdx")</param>
    /// <returns>fertige Adressierung</returns>
    static string R64Addr(int index)
    {
      return " [" + Asm.RegistersR64[index] + "]";
    }

    /// <summary>
    /// Flags für Zusatzinformationen
    /// </summary>
    [Flags]
    enum AddrExt
    {
      /// <summary>
      /// keine Sonderinfos vorhanden
      /// </summary>
      None = 0,
      /// <summary>
      /// fügt eine Int8-Konstante hinzu (-128 bis 127)
      /// </summary>
      C1 = 1,
      /// <summary>
      /// fügt eine Int32-Konstante hinzu (-2147483648 bis 2147483647)
      /// </summary>
      C4 = 2,
      /// <summary>
      /// ersetzt den Register "RBP" an erster Stelle durch eine Int32-Konstante aus (-2147483648 bis 2147483647)
      /// </summary>
      Rbp1ToC4 = 4,
      /// <summary>
      /// entfernt den Register "RSP" an zweiter Stelle 
      /// </summary>
      Rsp2Skip = 8
    }

    /// <summary>
    /// gibt eine mehrteilige 64-Bit Adressierung zurück, mit Leerzeichen am Anfang (z.B. "[rsi + rcx * 4]")
    /// </summary>
    /// <param name="index1">Index auf den ersten Register (z.B. 6: "rsi")</param>
    /// <param name="index2">Index auf den zweiten Register (z.B. 1: "rcx")</param>
    /// <param name="mulShift2">Shiftwert für die Multiplikation des zweiten Registers (z.B. 2: "rcx * 4")</param>
    /// <param name="ext">optionale Zusatzinfos</param>
    /// <returns>fertige Adressierung</returns>
    static string R64Addr(int index1, int index2, int mulShift2, AddrExt ext = AddrExt.None)
    {
      int constBytes = 0;
      bool skip1 = false;
      bool skip2 = false;

      if (ext.HasFlag(AddrExt.C1)) { constBytes = 1; }
      if (ext.HasFlag(AddrExt.C4)) { constBytes = 4; }
      if (ext.HasFlag(AddrExt.Rbp1ToC4) && index1 == 5) { skip1 = true; constBytes = 4; }
      if (ext.HasFlag(AddrExt.Rsp2Skip) && index2 == 4) { skip2 = true; }

      string t1 = skip1 ? "" : Asm.RegistersR64[index1];

      string t2 = skip2 ? "" : Asm.RegistersR64[index2];
      if (mulShift2 != 0 && t2 != "") t2 += " * " + (1 << mulShift2);

      string t3 = constBytes > 0 ? "x" : "";

      string str = t1;
      if (str != "" && t2 != "") str += " + ";
      str += t2;
      if (str != "" && t3 != "") str += " + ";
      str += t3;
      if (str == "") str = "0";

      return " [" + str + "]";
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
    #endregion

    /// <summary>
    /// generiert alle OpCodes anhand bestimmter Regeln
    /// </summary>
    /// <returns>Enumerable der Zeilen, welche generiert wurden</returns>
    static IEnumerable<string> GenerateOpCodes()
    {
      var opCode = new byte[3];
      int pos = 1;

      #region # // 00 00 - 00 3f
      for (int y = 0; y < 64; y++)
      {
        int rr = opCode[pos] / 8;
        switch (opCode[pos] & 7)
        {
          default: yield return StrB(opCode, pos) + Asm.Instructions[0] + R64Addr(opCode[pos] & 7) + ',' + R8(rr); break;

          case 4:
          {
            opCode[++pos] = 0;
            for (int x = 0; x < 256; x++)
            {
              int r1 = opCode[pos] & 7;
              yield return StrB(opCode, pos, r1 == 5 ? 4 : 0) + Asm.Instructions[0] + R64Addr(r1, opCode[pos] / 8 & 7, opCode[pos] / 64, AddrExt.Rbp1ToC4 | AddrExt.Rsp2Skip) + ',' + R8(rr);
              opCode[pos]++;
            }
            pos--;
          } break;

          case 5:
          {
            yield return StrB(opCode, pos, 4) + Asm.Instructions[0] + R64Addr(opCode[pos] & 7, 21, 0, AddrExt.Rbp1ToC4) + ',' + R8(rr);
          } break;
        }
        opCode[pos]++;
      }
      #endregion
      #region # // 00 40 - 00 7f
      for (int y = 0; y < 64; y++)
      {
        int rr = opCode[pos] / 8 & 7;
        switch (opCode[pos] & 7)
        {
          default: yield return StrB(opCode, pos, 1) + Asm.Instructions[0] + R64Addr(opCode[pos] & 7, 4, 0, AddrExt.C1 | AddrExt.Rsp2Skip) + ',' + R8(rr); break;

          case 4:
          {
            opCode[++pos] = 0;
            for (int x = 0; x < 256; x++)
            {
              yield return StrB(opCode, pos, 1) + Asm.Instructions[0] + R64Addr(opCode[pos] & 7, opCode[pos] / 8 & 7, opCode[pos] / 64 & 3, AddrExt.C1 | AddrExt.Rsp2Skip) + ',' + R8(rr);
              opCode[pos]++;
            }
            pos--;
          } break;
        }
        opCode[pos]++;
      }
      #endregion
      #region # // 00 80 - 00 bf
      for (int y = 0; y < 64; y++)
      {
        int rr = opCode[pos] / 8 & 7;
        switch (opCode[pos] & 7)
        {
          default: yield return StrB(opCode, pos, 4) + Asm.Instructions[0] + R64Addr(opCode[pos] & 7, 4, 0, AddrExt.C4 | AddrExt.Rsp2Skip) + ',' + R8(rr); break;

          case 4:
          {
            opCode[++pos] = 0;
            for (int x = 0; x < 256; x++)
            {
              yield return StrB(opCode, pos, 4) + Asm.Instructions[0] + R64Addr(opCode[pos] & 7, opCode[pos] / 8 & 7, opCode[pos] / 64 & 3, AddrExt.C4 | AddrExt.Rsp2Skip) + ',' + R8(rr);
              opCode[pos]++;
            }
            pos--;
          } break;
        }
        opCode[pos]++;
      }
      #endregion
    }

    /// <summary>
    /// maximale Anzahl der Vorschau-Zeilen, falls eine Liste länger sein sollte
    /// </summary>
    const int SamplePreview = 10;

    const int StartView = 4200;

    static void Main(string[] args)
    {
      var known = ReadKnownOpCodes().ToArray();
      var gen = GenerateOpCodes().ToArray();
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

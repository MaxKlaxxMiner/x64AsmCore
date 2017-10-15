#region # using *.*
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable StringIndexOfIsCultureSpecific.1
// ReSharper disable UnusedMember.Global
#endregion

namespace OpCodeGenerator
{
  /// <summary>
  /// Klasse mit Hilfmethoden zum bearbeiten von Textdateien
  /// </summary>
  public static class TextHelper
  {
    static string SwapOperand(string line)
    {
      try
      {
        if (line.Trim() == "" || line.Trim()[0] == '#') return line;
        string t = line;
        int p = t.IndexOf(" - ");
        string r = t.Substring(0, p + 3);
        t = t.Remove(0, p + 3);
        p = t.IndexOf(' ');
        r += t.Substring(0, p + 1);
        t = t.Remove(0, p + 1);
        var sp = t.Split(',');
        r += sp[1].Trim() + ", " + sp[0].Trim();

        return r;
      }
      catch
      {
        Console.WriteLine("err: " + line);
        return line;
      }
    }

    /// <summary>
    /// tauscht die Operanden in allen Opcode-Zeilen
    /// </summary>
    /// <param name="inputFile">Datei, welche gelesen werden soll</param>
    /// <param name="outputFile">Datei, welche mit getauschten Operanden wieder geschrieben werden soll</param>
    public static void SwapOperands(string inputFile, string outputFile)
    {
      using (var w = new StreamWriter(outputFile))
      {
        foreach (var r in File.ReadLines(inputFile))
        {
          w.WriteLine(SwapOperand(r));
        }
      }
    }

    static string ReplaceFirstOpcode(string line, string newFirstCode)
    {
      try
      {
        if (line.Trim() == "" || line.Trim()[0] == '#') return line;
        string t = line;
        int p = t.IndexOf(' ');
        if (p != 2) throw new Exception();
        string r = newFirstCode;
        r += t.Remove(0, p);
        return r;
      }
      catch
      {
        Console.WriteLine("err: " + line);
        return line;
      }
    }

    /// <summary>
    /// ersetzt den ersten Opcode in allen Zeilen
    /// </summary>
    /// <param name="inputFile">Datei, welche gelesen werden soll</param>
    /// <param name="outputFile">Datei, welche mit neuen Opcodes geschrieben werden soll</param>
    /// <param name="newFirstCode">neuer erster Opcode</param>
    public static void ReplaceFirstOpcodes(string inputFile, string outputFile, string newFirstCode)
    {
      using (var w = new StreamWriter(outputFile))
      {
        foreach (var r in File.ReadLines(inputFile))
        {
          w.WriteLine(ReplaceFirstOpcode(r, newFirstCode));
        }
      }
    }

    static string ReplaceInstruction(string line, string newInstruction)
    {
      try
      {
        if (line.Trim() == "" || line.Trim()[0] == '#') return line;
        string t = line;
        int p = t.IndexOf(" - ");
        string r = t.Substring(0, p + 3);
        t = t.Remove(0, p + 3);
        p = t.IndexOf(' ');
        r += newInstruction + " ";
        r += t.Remove(0, p + 1);

        return r;
      }
      catch
      {
        Console.WriteLine("err: " + line);
        return line;
      }
    }

    /// <summary>
    /// ersetzt die Instruktion in allen Zeilen
    /// </summary>
    /// <param name="inputFile">Datei, welche gelesen werden soll</param>
    /// <param name="outputFile">Datei, welche mit neuen Instruktionen geschrieben werden soll</param>
    /// <param name="newInstruction">neue Instruktion</param>
    public static void ReplaceInstrucion(string inputFile, string outputFile, string newInstruction)
    {
      using (var w = new StreamWriter(outputFile))
      {
        foreach (var r in File.ReadLines(inputFile))
        {
          w.WriteLine(ReplaceInstruction(r, newInstruction));
        }
      }
    }

    static string ReplaceFirstChars(string line, string newChars)
    {
      try
      {
        if (line.Trim() == "" || line.Trim()[0] == '#') return line;
        string t = line;
        return newChars + t.Remove(0, newChars.Length);
      }
      catch
      {
        Console.WriteLine("err: " + line);
        return line;
      }
    }

    /// <summary>
    /// erstzt die ersten Zeichen in jeder Zeile (opcodes)
    /// </summary>
    /// <param name="inputFile">Datei, welche gelesen werden soll</param>
    /// <param name="outputFile">Datei, welche mit neuen Instruktionen geschrieben werden soll</param>
    /// <param name="newChars">neue Zeichen, welche mit der gleichen Länge ersetzt werden sollen</param>
    public static void ReplaceFirstChars(string inputFile, string outputFile, string newChars)
    {
      using (var w = new StreamWriter(outputFile))
      {
        foreach (var r in File.ReadLines(inputFile))
        {
          w.WriteLine(ReplaceFirstChars(r, newChars));
        }
      }
    }
  }
}

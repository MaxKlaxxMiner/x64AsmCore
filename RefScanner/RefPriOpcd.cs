#region # using *.*
// ReSharper disable RedundantUsingDirective
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NotAccessedField.Global
#endregion

namespace RefScanner
{
  public sealed class RefPriOpcd
  {
    public readonly byte value;
    public readonly int procStart;
    public readonly RefEntry[] entries;

    public RefPriOpcd(XElement xml)
    {
      value = xml.ParseHex("value", (byte)0);
      procStart = xml.ParseHex("proc_start", 0);
      entries = xml.Descendants("entry").Select(x => new RefEntry(x)).ToArray();
    }

    public override string ToString()
    {
      return (new { value = value.ToString("X2"), procStart, entries = "RefEntry[" + entries.Length + "]" }).ToString();
    }

    /// <summary>
    /// gibt den Pfad zur x86reference.xml zurück oder wirft eine Exception, wenn nicht gefunden
    /// </summary>
    static string GetRefFileName()
    {
      const string SearchFile = "x86reference.xml";
      for (int i = 0; i < 5; i++)
      {
        string fileName = new string('#', i).Replace("#", "../") + SearchFile;
        if (File.Exists(fileName)) return fileName;
      }
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine();
      Console.WriteLine("  File not found: " + SearchFile);
      Console.WriteLine();
      Console.WriteLine("  You can download it from Github:");
      Console.WriteLine();
      Console.WriteLine("  https://raw.githubusercontent.com/Barebit/x86reference/master/x86reference.xml");
      Console.WriteLine();
      throw new FileNotFoundException();
    }

    /// <summary>
    /// liest alle die Elemente der x86reference.xml ein
    /// </summary>
    /// <param name="twoBytes">gibt an, ob statt die 1-Byte Elemente nun die 2-Byte Elemente geladen werden sollen</param>
    /// <returns>fertig gelesene Elemente</returns>
    public static RefPriOpcd[] ReadElements(bool twoBytes = false)
    {
      var xdoc = XDocument.Load(GetRefFileName());
      return xdoc.Root.Element(twoBytes ? "two-byte" : "one-byte").Descendants("pri_opcd").Select(x => new RefPriOpcd(x)).ToArray();
    }
  }
}

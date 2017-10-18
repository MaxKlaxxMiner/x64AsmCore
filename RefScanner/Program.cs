using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RefScanner
{
  class Program
  {
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

    static void Main(string[] args)
    {
      var xdoc = XDocument.Load(GetRefFileName());
      var oneByte = new RefOneByte(xdoc.Root.Element("one-byte"));
      var twoByte = new RefTwoByte(xdoc.Root.Element("two-byte"));
    }
  }
}

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

    static void Main(string[] args)
    {
      var oneByte = RefPriOpcd.ReadElements();
      var twoByte = RefPriOpcd.ReadElements(true);
    }
  }
}

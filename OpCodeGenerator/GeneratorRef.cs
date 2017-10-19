#region # using *.*
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using RefScanner;

// ReSharper disable ConvertToConstant.Local
#endregion

namespace OpCodeGenerator
{
  /// <summary>
  /// Klasse mit den Methoden zum generieren der Opcodes anhand der original x86reference.xml
  /// </summary>
  public static class GeneratorRef
  {
    // .....### --- r0, r1, r2, r3, #, C4|r5, r6, r7
    // .....000 - [rax], ?
    // .....001 - [rcx], ?
    // .....010 - [rdx], ?
    // .....011 - [rbx], ?
    // .....100 - [#], ?
    // .....101 - [C4|rbp], ?
    // .....110 - [rsi], ?
    // .....111 - [rdi], ?

    // ..###... --- r0, r1, r2, r3, r4, r5, r6, r7
    // ..000... - ?, al
    // ..001... - ?, cl
    // ..010... - ?, dl
    // ..011... - ?, bl
    // ..100... - ?, ah
    // ..101... - ?, ch
    // ..110... - ?, dh
    // ..111... - ?, bh

    // ##...... --- [?], [? + C1], [? + C4], R
    // 00...... - [?], ?
    // 01...... - [? + C1], ?
    // 10...... - [? + C4], ?
    // 11...... - ?, ?

    // ........ .....### --- r0, r1, r2, r3, r4, C4|r5, r6, r7
    // ........ .....000 - [rax + ? * ? + ?]
    // ........ .....001 - [rcx + ? * ? + ?]
    // ........ .....010 - [rdx + ? * ? + ?]
    // ........ .....011 - [rbx + ? * ? + ?]
    // ........ .....100 - [rsp + ? * ? + ?]
    // ........ .....101 - [C4|rbp + ? * ?]
    // ........ .....110 - [rsi + ? * ? + ?]
    // ........ .....111 - [rdi + ? * ? + ?]

    // ........ ..###... --- r0, r1, r2, r3, 0|r4, r5, r6, r7
    // ........ ..000... - [? + rax * ? + ?]
    // ........ ..001... - [? + rcx * ? + ?]
    // ........ ..010... - [? + rdx * ? + ?]
    // ........ ..011... - [? + rbx * ? + ?]
    // ........ ..100... - [? + 0 * ? + ?]
    // ........ ..101... - [? + rbp * ? + ?]
    // ........ ..110... - [? + rsi * ? + ?]
    // ........ ..111... - [? + rdi * ? + ?]

    // ........ ##...... --- 1, 2, 4, 8
    // ........ 00...... - [? + ? * 1 + ?]
    // ........ 01...... - [? + ? * 2 + ?]
    // ........ 10...... - [? + ? * 4 + ?]
    // ........ 11...... - [? + ? * 8 + ?]

    static string Para(string p1, string p2, bool swap = false)
    {
      if (swap)
      {
        return " " + p2 + ", " + p1;
      }
      else
      {
        return " " + p1 + ", " + p2;
      }
    }

    static string[] GetRegs(RefParam p)
    {
      if (p.t == "b") return Asm.RegistersR8;
      if (p.t == "vqp") return Asm.RegistersR32;
      if (p.type == "b") return Asm.RegistersR8;
      if (p.type == "vqp") return Asm.RegistersR32;
      return null;
    }

    static IEnumerable<string> Generate(byte[] opCode, int pos, RefSyntax syntax, RefEntry entry, RefPriOpcd pri)
    {
      if (syntax.mnem == null || syntax.dst.Length != 1 || syntax.src.Length != 1) throw new NotImplementedException();
      var mnem = syntax.mnem;
      var dst = syntax.dst[0];
      var rgDst = GetRegs(dst);
      var src = syntax.src[0];
      var rgSrc = GetRegs(src);

      if (dst.nr != null)
      {
        if (src.a == "I" && (src.t == "b" || src.t == "vds"))
        {
          yield return Generator.StrB(opCode, pos - 1, src.t == "b" ? 1 : 4) + mnem.ToLower() + Para(rgDst[(int)dst.nr], "x");
          yield break;
        }
        throw new NotImplementedException();
      }

      bool swap = false;
      if (dst.a == "G" && src.a == "E") swap = true; else if (dst.a != "E" || src.a != "G") throw new NotImplementedException();

      for (int b = 0; b < 256; b++)
      {
        opCode[pos] = (byte)b;
        int p1 = b & 7;
        int p2 = b >> 3 & 7;
        int px = b >> 6;

        int fixConst = 0;
        switch (px)
        {
          case 0:
          {
            string r1 = Asm.RegistersR64[p1];
            string r2 = rgSrc[p2];
            switch (r1)
            {
              case "rsp":
              {
                pos++;
                for (int b2 = 0; b2 < 256; b2++)
                {
                  opCode[pos] = (byte)b2;
                  int pp1 = b2 & 7;
                  int pp2 = b2 >> 3 & 7;
                  int ppx = b2 >> 6;
                  int cbytes = fixConst;

                  if (pp1 == 5 && cbytes == 0)
                  {
                    r1 = Asm.RegistersR64[pp2] + " * " + (1 << ppx) + " + x";
                    cbytes = 4;
                  }
                  else
                  {
                    r1 = Asm.RegistersR64[pp1] + " + " + Asm.RegistersR64[pp2] + " * " + (1 << ppx);
                  }
                  r1 = r1.Replace("rsp * 1", "").Replace("rsp * 2", "").Replace("rsp * 4", "").Replace("rsp * 8", "");
                  r1 = r1.Replace(" * 1", "");
                  r1 = r1.Trim(' ', '+');

                  yield return Generator.StrB(opCode, pos, cbytes) + mnem.ToLower() + Para("[" + r1 + (fixConst != 0 ? " + x" : "") + "]", r2, swap);
                }
                pos--;
              } break;
              case "rbp":
              {
                if (fixConst != 0) goto default;
                yield return Generator.StrB(opCode, pos, 4) + mnem.ToLower() + Para("[rip + x]", r2, swap);
              } break;
              default:
              {
                yield return Generator.StrB(opCode, pos, fixConst) + mnem.ToLower() + Para("[" + r1 + (fixConst != 0 ? " + x" : "") + "]", r2, swap); break;
              }
            }
          } break;
          case 1: fixConst = 1; goto case 0;
          case 2: fixConst = 4; goto case 0;
          case 3: yield return Generator.StrB(opCode, pos) + mnem.ToLower() + Para(rgDst[p1], rgSrc[p2], swap); break;
        }
      }
    }

    /// <summary>
    /// generiert alle OpCodes
    /// </summary>
    /// <returns>Enumerable der Zeilen, welche generiert wurden</returns>
    public static IEnumerable<string> GenerateOpCodes()
    {
      var opCode = new byte[15];

      var oneBytes = RefPriOpcd.ReadElements();

      foreach (var oneByte in oneBytes)
      {
        Array.Clear(opCode, 0, opCode.Length);
        int pos = 0;
        opCode[pos++] = oneByte.value;
        var entry = oneByte.entries.FirstOrDefault();
        if (oneByte.entries.Length != 1)
        {
          if (oneByte.entries.Any(x => x.attr == "invd" && x.note == "Invalid Instruction in 64-Bit Mode"))
          {
            yield return Generator.StrB(opCode, pos - 1) + "???";
            continue;
          }
          if (oneByte.value == 0x0f) break;
          throw new NotImplementedException();
        }
        foreach (var syntax in entry.syntax)
        {
          foreach (var line in Generate(opCode, pos, syntax, entry, oneByte)) yield return line;
        }
      }
    }
  }
}

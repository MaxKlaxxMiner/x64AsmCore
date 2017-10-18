#region # using *.*
// ReSharper disable RedundantUsingDirective
using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NotAccessedField.Global
#endregion

namespace RefScanner
{
  public sealed class RefEntry
  {
    public readonly int? direction;
    public readonly int? opSize;
    public readonly bool? r;
    public readonly bool? locked;
    public readonly string attr;
    public readonly string mode;
    public readonly string doc1632Ref;
    public readonly string Ref;
    public readonly string ring;
    public readonly bool? isUndoc;
    public readonly string doc;
    public readonly bool? isDoc;
    public readonly bool? particular;
    public readonly bool? fpush;
    public readonly int? signExt;
    public readonly string ringRef;
    public readonly string tttn;
    public readonly string alias;
    public readonly string docRef;
    public readonly string doc64Ref;
    public readonly string memFormat;
    public readonly string fpop;
    public readonly string mod;
    public readonly string partAlias;
    public readonly string docPartAliasRef;

    public readonly byte? pref;
    public readonly int? opcdExt;
    public readonly byte? secOpcd;    // ignored: escape
    public readonly string procStart; // ignored: post & lat_step
    public readonly string procEnd;
    public readonly RefSyntax[] syntax;
    public readonly InstrExt instrExt;
    public readonly string[] grps;
    public readonly string testF;
    public readonly string modifF;    // ignored: cond
    public readonly string defF;      // ignored: cond
    public readonly string undefF;
    public readonly string fVals;
    public readonly string modifFFpu;
    public readonly string defFFpu;
    public readonly string undefFFpu;
    public readonly string fValsFpu;
    public readonly string note;

    public RefEntry(XElement xml)
    {
      direction = xml.ParseInt("direction");
      opSize = xml.ParseInt("op_size");
      r = xml.ParseYesNo("r");
      locked = xml.ParseYesNo("lock");
      attr = xml.GetValue("attr");
      mode = xml.GetValue("mode");
      doc1632Ref = xml.GetValue("doc1632_ref");
      Ref = xml.GetValue("ref");
      ring = xml.GetValue("ring");
      isUndoc = xml.ParseYesNo("is_undoc");
      doc = xml.GetValue("doc");
      isDoc = xml.ParseYesNo("is_doc");
      particular = xml.ParseYesNo("particular");
      fpush = xml.ParseYesNo("fpush");
      signExt = xml.ParseInt("sign-ext");
      ringRef = xml.GetValue("ring_ref");
      tttn = xml.GetValue("tttn");
      alias = xml.GetValue("alias");
      docRef = xml.GetValue("doc_ref");
      doc64Ref = xml.GetValue("doc64_ref");
      memFormat = xml.GetValue("mem_format");
      fpop = xml.GetValue("fpop");
      mod = xml.GetValue("mod");
      partAlias = xml.GetValue("part_alias");
      docPartAliasRef = xml.GetValue("doc_part_alias_ref");

      pref = xml.ParseHex("pref");
      opcdExt = xml.ParseInt("opcd_ext");
      secOpcd = xml.ParseHex("sec_opcd");
      procStart = xml.GetValue("proc_start");
      procEnd = xml.GetValue("proc_end");
      syntax = xml.Descendants("syntax").Select(x => new RefSyntax(x)).ToArray();
      switch (xml.GetValue("instr_ext"))
      {
        case null: instrExt = InstrExt.None; break;
        case "mmx": instrExt = InstrExt.Mmx; break;
        case "sse1": instrExt = InstrExt.Sse1; break;
        case "sse2": instrExt = InstrExt.Sse2; break;
        case "sse3": instrExt = InstrExt.Sse3; break;
        case "ssse3": instrExt = InstrExt.Ssse3; break;
        case "sse41": instrExt = InstrExt.Sse41; break;
        case "sse42": instrExt = InstrExt.Sse42; break;
        case "vmx": instrExt = InstrExt.Vmx; break;
        case "smx": instrExt = InstrExt.Smx; break;
        default: throw new Exception("unknown: " + xml.GetValue("instr_ext"));
      }
      grps = xml.Descendants("grp1").Select(x => x.Value).Concat(xml.Descendants("grp2").Select(x => x.Value)).Concat(xml.Descendants("grp3").Select(x => x.Value)).ToArray();
      testF = xml.GetValue("test_f");
      modifF = xml.GetValue("modif_f");
      defF = xml.GetValue("def_f");
      undefF = xml.GetValue("undef_f");
      fVals = xml.GetValue("f_vals");
      modifFFpu = xml.GetValue("modif_f_fpu");
      defFFpu = xml.GetValue("def_f_fpu");
      undefFFpu = xml.GetValue("undef_f_fpu");
      fValsFpu = xml.GetValue("f_vals_fpu");
      if (xml.Element("note") != null && xml.Element("note").Element("brief") != null)
      {
        note = xml.Element("note").ToString(SaveOptions.DisableFormatting).Replace("<note>", "").Replace("</note>", "").Replace("<brief>", "").Replace("</brief>", "\r\n")
                  .Replace("<det>", "").Replace("</det>", "").Replace("<!--1.11 Packed -->", "")
                  .Replace("<sub>2</sub>", "2 ").Replace("<sub>10</sub>", "10 ").Replace("<sub>e</sub>", "e ").Replace("<sup>x</sup>", "^x ")
                  .Trim();
        if (note.Contains("<"))
        {
          throw new Exception("xml-char found -> replace it");
        }
      }
    }

    public override string ToString()
    {
      string res = "";
      if (direction != null) res += ", direction: " + direction;
      if (opSize != null) res += ", opSize: " + opSize;
      if (r != null) res += ", r: " + r;
      if (locked != null) res += ", locked: " + locked;
      if (attr != null) res += ", attr: " + attr;
      if (mode != null) res += ", mode: " + mode;
      if (doc1632Ref != null) res += ", doc1632Ref: " + doc1632Ref;
      if (Ref != null) res += ", Ref: " + Ref;
      if (ring != null) res += ", ring: " + ring;
      if (isUndoc != null) res += ", isUndoc: " + isUndoc;
      if (doc != null) res += ", doc: " + doc;
      if (isDoc != null) res += ", isDoc: " + isDoc;
      if (particular != null) res += ", particular: " + particular;
      if (fpush != null) res += ", fpush: " + fpush;
      if (signExt != null) res += ", signExt: " + signExt;
      if (ringRef != null) res += ", ringRef: " + ringRef;
      if (tttn != null) res += ", tttn: " + tttn;
      if (alias != null) res += ", alias: " + alias;
      if (docRef != null) res += ", docRef: " + docRef;
      if (doc64Ref != null) res += ", doc64Ref: " + doc64Ref;
      if (memFormat != null) res += ", memFormat: " + memFormat;
      if (fpop != null) res += ", fpop: " + fpop;
      if (mod != null) res += ", mod: " + mod;
      if (partAlias != null) res += ", partAlias: " + partAlias;
      if (docPartAliasRef != null) res += ", docPartAliasRef: " + docPartAliasRef;

      if (pref != null) res += ", pref: " + ((byte)pref).ToString("X2");
      if (opcdExt != null) res += ", opcdExt: " + opcdExt;
      if (secOpcd != null) res += ", secOpcd: " + ((byte)secOpcd).ToString("X2");
      if (procStart != null) res += ", procStart: " + procStart;
      if (procEnd != null) res += ", procEnd: " + procStart;
      if (syntax.Length != 0) res += ", syntax: RefSyntax[" + syntax.Length + "]";
      if (instrExt != null) res += ", instrExt: " + instrExt;
      if (grps.Length != 0) res += ", grps: {" + string.Join(", ", grps) + "}";
      if (testF != null) res += ", testF: " + testF;
      if (modifF != null) res += ", modifF: " + modifF;
      if (defF != null) res += ", defF: " + defF;
      if (undefF != null) res += ", undefF: " + undefF;
      if (fVals != null) res += ", fVals: " + fVals;
      if (modifFFpu != null) res += ", modifFFpu: " + modifFFpu;
      if (defFFpu != null) res += ", defFFpu: " + defFFpu;
      if (undefFFpu != null) res += ", undefFFpu: " + undefFFpu;
      if (fValsFpu != null) res += ", fValsFpu: " + fValsFpu;
      if (note != null) res += ", note: " + note;

      return res.TrimStart(',', ' ');
    }
  }
}

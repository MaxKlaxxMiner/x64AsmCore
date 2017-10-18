#region # using *.*
// ReSharper disable RedundantUsingDirective
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
      return res.TrimStart(',', ' ');
    }
  }
}

﻿#region # using *.*
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
  public sealed class RefSyntax
  {
    public readonly bool? modMem;
    public readonly string mnem; // ignored: sug
    public readonly RefParam[] dst;
    public readonly RefParam[] src;

    public RefSyntax(XElement xml)
    {
      switch (xml.GetValue("mod"))
      {
        case null: modMem = null; break;
        case "mem": modMem = true; break;
        case "nomem": modMem = false; break;
        default: throw new Exception("unkown: " + xml.GetValue("mod"));
      }
      mnem = xml.GetValue("mnem");

      dst = xml.Descendants("dst").Select(x => new RefParam(x)).ToArray();
      src = xml.Descendants("src").Select(x => new RefParam(x)).ToArray();
    }

    public override string ToString()
    {
      string res = "";

      if (mnem != null) res += ", mnem: " + mnem;
      if (modMem != null) res += ", modMem: " + modMem;
      if (dst.Length != 0) res += ", dst[" + dst.Length + "]";
      if (src.Length != 0) res += ", src[" + src.Length + "]";

      return res.TrimStart(',', ' ');
    }
  }
}


using Mn.Framework.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


public static class StringExtensions
{
    public static string RemoveBadChar(this string val, string badCharacters)
    {
        foreach (var chr in badCharacters.Select(c => c.ToString()).ToList())
            val = val.Replace(chr, "");
        return val;
    }

    public static bool EqualsTrim(this string str1, string str2)
    {
        if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
            return false;
        return str1.Replace(" ", "").ToLower().Equals(str2.Replace(" ", "").ToLower());
    }
    public static string SubstringM(this string str, int Length)
    {
        if (string.IsNullOrEmpty(str))
            return str;
        if (Length < str.Length)
            return str.Substring(0, Length);
        else
            return str;
    }

}


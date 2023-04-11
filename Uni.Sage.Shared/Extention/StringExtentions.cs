using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Shared.Extention
{
    public static class StringExtentions
    {
        public static void AddLigne(this string str, string ligne)
        {
            if (string.IsNullOrEmpty(str))
            {
                str = ligne;
            }
            else
            {
                str += Environment.NewLine + ligne;
            }
        }

        public static string Increment(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return "1";
            var match =System.Text.RegularExpressions.Regex.Match(str, @"(\d+)(?!.*\d)");
            if (match == null) return str + "1";
            return str.Replace(match.Value, (int.Parse(match.Value) +1).ToString());
           // var newString = System.Text.RegularExpressions.Regex.Replace(str, "\\d+", m => (int.Parse(m.Value) + 1).ToString(new string('0', m.Value.Length)));           
           // return newString;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SoapHelper
{
    public class HttpUrlValidation
    {
        public static bool IsValidHttpAddress(string input)
        {
            var expression = new Regex(@"(^|[\s.:;?\-\]<\(])(https?:\/\/[-\w;/?:@&=+$\|_.!~*\|'()\[\]%#,☺]+[\w/#](\(\))?)(?=$|[\s',\|\(\).:;?\-\[\]>\)])");
            var match = expression.Match(input);
            return match.Captures.Count > 0 && match.Success;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClientServer
{
    public class JSONStringSpliter
    {
        const string c_separetorPattern = "}{";
        const string c_jsonDataPattern = "{.*}";

        Regex _separatorRegex;
        Regex _jsonDataRagex;
        public JSONStringSpliter() 
        {
            _separatorRegex = new Regex(c_separetorPattern);
            _jsonDataRagex = new Regex(c_jsonDataPattern);
        }

        public List<string> SplitJSONStrings(string local)
        {
            List<string> jsonStrings = new List<string>();
            if(_separatorRegex.IsMatch(local))
            {
                string[] json_Strings = _jsonDataRagex.Split(local);
                return new List<string>(json_Strings);
            }
            else
            {
                jsonStrings.Add(local);
                return jsonStrings;
            }
        }
    }
}

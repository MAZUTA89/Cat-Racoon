using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace ClientServer
{
    public class PrefixWriterReader
    {
        public int PrefixSize { get; private set; }
        public int PrefixLength { get; private set; }
        const string c_prefixBase = "Size:[ ]";
        const string c_sizeGroupName = "Size";
        const string c_sizePattern = @"Size:\[(?'Size'\d+)\s*\]";
        string _prefix;
        StringBuilder _prefixBuilder;
        Regex _sizeRegex;
        public PrefixWriterReader(int prefixSize)
        {
            _prefixBuilder = new StringBuilder();
            _sizeRegex = new Regex(c_sizePattern);
            PrefixSize = prefixSize;
        }
        public void InitializePrefix(int byteLength)
        {
            _prefixBuilder.Clear();
            _prefixBuilder.Append(c_prefixBase);
            _prefix = byteLength.ToString();
            _prefixBuilder = _prefixBuilder.Replace(" ", _prefix);
        }
        
        public string WriteInfoPrefix(string local)
        {
            _prefixBuilder.Append(local);
            return _prefixBuilder.ToString();
        }
        public int ReadInfoPrefix(string local)
        {
            if (_sizeRegex.IsMatch(local))
            {
                Match match = _sizeRegex.Match(local);
                var sizeData = match.Value;
                PrefixLength = sizeData.Length;
                return int.Parse(match.Groups[c_sizeGroupName].Value);
            }
            else
            {
                throw new Exception("Prefix massage failed to read");
            }
        }
    }
}

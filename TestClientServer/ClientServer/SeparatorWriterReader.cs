using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServer
{
    public class SeparatorWriterReader
    {
        const string c_separatorSign = "|";

        StringBuilder _builder;
        public SeparatorWriterReader()
        {
            _builder = new StringBuilder();
        }

        public string WriteSeparator(string local)
        {
            _builder.Clear();
            _builder.Append(local);
            _builder.Append(c_separatorSign);
            return _builder.ToString();
        }

        //public List<string> ReadSeparated(string local)
        //{

        //}
    }
}

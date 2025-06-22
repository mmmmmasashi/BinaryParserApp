using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserLib.Text
{
    public class TableData(string? protocolName, List<List<string>> rows, List<string> headers)
    {
        private string? _protocolName = protocolName;
        private List<List<string>> _rows = rows ?? new List<List<string>>();
        private List<string> _headers = headers ?? new List<string>();

        public string ProtocolName { get => _protocolName ?? "---"; }
        public List<List<string>> Rows { get => new List<List<string>>(_rows); }

        //列名 → h1, h2, h3, ..., hN, data
        public List<string> GetHeaderNames() => new List<string>(_headers); 
    }
}

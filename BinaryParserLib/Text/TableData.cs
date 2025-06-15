using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserLib.Text
{
    public class TableData(string? protocolName, List<List<string>> rows)
    {
        private string? _protocolName = protocolName;
        private List<List<string>> _rows = rows ?? new List<List<string>>();

        public string ProtocolName { get => _protocolName ?? "---"; }
        public List<List<string>> Rows { get => new List<List<string>>(_rows); }
    }
}

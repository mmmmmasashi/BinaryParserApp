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
        public List<Dictionary<string, string>> RowsHeaderValuePair
        {
            //列名をキー、フィールドを値とする辞書のリストにRowsを変換する
            //TODO: Rowsにこのプロパティが将来的には置き換わる？
            get
            {
                if (_rows.Count == 0)
                    return new List<Dictionary<string, string>>();
                var headers = GetHeaderNames();
                var rowsWithHeader = new List<Dictionary<string, string>>();

                foreach (var row in _rows)
                {
                    var rowDict = new Dictionary<string, string>();
                    int lastIdx = row.Count - 1;
                    for (int i = 0; i <= lastIdx; i++)
                    {
                        if (i < lastIdx)
                        {
                            rowDict[headers[i]] = row[i];
                        }
                        else
                        {
                            rowDict["data"] = row[i];
                        }
                    }
                    rowsWithHeader.Add(rowDict);
                }
                return rowsWithHeader;
            }
        }

        //列名 → h1, h2, h3, ..., hN, data
        public List<string> GetHeaderNames()
        {
            if (_rows.Count == 0)
                return new List<string>();
            //列名 → h1, h2, h3, ..., hN
            var colCount = _rows[0].Count;
            var headers = new List<string>();
            for (int number = 1; number <= colCount - 1; number++)
            {
                headers.Add($"h{number}");
            }
            headers.Add("data");
            return headers;
        }
    }
}

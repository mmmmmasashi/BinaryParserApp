using BinaryParserLib.Parsed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserLib.Text;

public class ParsedDataConverter
{
    public List<string> FormatToTsv(ParsedData result)
    {
        var root = new Field(null, GetRootName(result), null, result.RootFields.ToList());
        var lines = TreeFormatter.ToIndentedLines<Field>(root);
        return lines;
    }

    public TableData ConvertToTableData(ParsedData result)
    {
        var root = new Field(null, GetRootName(result), null, result.RootFields.ToList());
        return TreeFormatter.ToTableData<Field>(root);
    }

    private string GetRootName(ParsedData data) => data.ProtocolName ?? "---";
}

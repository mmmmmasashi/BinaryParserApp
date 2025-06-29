using BinaryParserLib.Parsed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserLib.Text;

public class ParsedDataConverter
{
    private readonly TableFormatOption _opt;

    public ParsedDataConverter(TableFormatOption? opt = null)
    {
        if (opt is null)
        {
            opt = new TableFormatOption();
        }
        this._opt = opt;
    }

    public List<string> FormatToTsv(ParsedData result)
    {
        var root = new Field(null, GetRootName(result), null, result.RootFields.ToList());
        var lines = new TreeFormatter(_opt).ToIndentedLines<Field>(root);
        return lines;
    }

    public TableData ConvertToTableData(ParsedData result)
    {
        var root = new Field(null, GetRootName(result), null, result.RootFields.ToList());
        return new TreeFormatter(_opt).ToTableData(root);
    }

    private string GetRootName(ParsedData data) => data.ProtocolName ?? "---";
}

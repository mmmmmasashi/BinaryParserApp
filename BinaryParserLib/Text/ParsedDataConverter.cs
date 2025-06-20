﻿using BinaryParserLib.Parsed;
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
        //TODO:Fieldのnameなしも許容したい
        var rootName = result.ProtocolName ?? "---";
        var root = new Field(null, rootName, null, result.RootFields.ToList());
        var lines = TreeFormatter.ToIndentedLines<Field>(root);
        return lines;
    }

    public TableData ConvertToTableData(ParsedData result)
    {
        //TODO:Fieldのnameなしも許容したい
        var rootName = result.ProtocolName ?? "---";
        var root = new Field(null, rootName, null, result.RootFields.ToList());
        return TreeFormatter.ToTableData<Field>(root);
    }
}

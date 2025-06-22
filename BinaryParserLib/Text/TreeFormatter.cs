using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BinaryParserLib.Text;

internal class TreeFormatter
{
    private readonly TableFormatOption _option;

    internal TreeFormatter(TableFormatOption? option = null)
    {
        _option = option ?? new TableFormatOption();
    }

    /// <summary>
    /// ツリー構造のノードをインデント付きの文字列リストに変換します。
    /// </summary>
    internal List<string> ToIndentedLines<T>(T node, int depth = 0) where T : ITreeNode<T>
    {
        var tableData = ToTableData(node);
        return tableData.Rows.Select(row => string.Join("\t", row)).ToList();
    }

    class NamesAndValue
    {
        public List<string> Names { get; set; }
        public string Value { get; set; }
        public NamesAndValue(List<string> names, string value)
        {
            Names = names;
            Value = value;
        }

        internal NamesAndValue AlignHierarchy(int maxDepth)
        {
            // 名前の深さに応じて空要素を追加
            var countOfBlank = maxDepth - Names.Count;
            var blanks = Enumerable.Repeat(string.Empty, countOfBlank).ToList();
            return new NamesAndValue(Names.Concat(blanks).ToList(), Value);
        }
    }

    internal TableData ToTableData<T>(T node) where T : ITreeNode<T>
    {
        List<NamesAndValue> namesAndValueList = ToHierarchicalNamesAndValueList(node);

        var maxNameDepth = namesAndValueList.Max(n => n.Names.Count);

        //インデントを調整して、Valueの位置を揃える形でstring化する
        var alignedNamesAndValueList = namesAndValueList
            .Select(nv => nv.AlignHierarchy(maxNameDepth))
            .ToList();


        List<string> headers = CreateHeaders(alignedNamesAndValueList.First());
        List<List<string>> tableRows = CreateRows(alignedNamesAndValueList);

        return new TableData(node.Name, tableRows, headers);
    }

    private List<List<string>> CreateRows(List<NamesAndValue> alignedNamesAndValueList)
    {
        int number = 1;
        int currentByteIdx = 0;
        return alignedNamesAndValueList.Select(item =>
        {
            var row = new List<string>();
            if (_option.UseNumberOption) row.Add($"{number++}");
            row.AddRange(item.Names);
            if (_option.ShowIndex)
            {
                row.Add($"{currentByteIdx}");
                currentByteIdx += item.Value.Length / 2; // 1文字あたり2桁のHEX表現を想定
            }
            row.Add(item.Value);
            return row;
        }).ToList();
    }

    private List<string> CreateHeaders(NamesAndValue firstNamesAndValue)
    {
        var headers = new List<string>();
        if (_option.UseNumberOption) headers.Add("No.");
        headers.AddRange(Enumerable.Range(1, firstNamesAndValue.Names.Count).Select(number => $"h{number}"));
        if (_option.ShowIndex) headers.Add("index");
        headers.Add("data(HEX)");
        return headers;
    }

    /// <summary>
    /// ツリー構造のノードを、名称と値のペアのリストに変換します。
    /// </summary>
    private List<NamesAndValue> ToHierarchicalNamesAndValueList<T>(T node) where T : ITreeNode<T>
    {
        var namesAndValueList = new List<(List<string>, string)>();
        var names = new List<string>();
        ToNamesAndHexStr(node, namesAndValueList, names, isTopNode:true);
        return namesAndValueList.Select(item => new NamesAndValue(item.Item1, item.Item2)).ToList();
    }

    /// <summary>
    /// ツリー構造のノードを再帰的に処理し、名称のリストと値をペアとするリストを作成します。
    /// </summary>
    private void ToNamesAndHexStr<T>(
        T node,
        List<(List<string>, string)> namesAndValueList,
        List<string> namesCurrent,
        bool isTopNode) where T : ITreeNode<T>
    {
        if (!isTopNode)
        {
            namesCurrent.Add(node.Name);
        }

        bool isLeaf = node.Children.Count == 0;

        if (isLeaf)
        {
            // リーフノードの場合、名前と値を追加
            var newLine = (new List<string>(namesCurrent), node.HexStr) ;//注意 : 新しくリストを作らないと、登録済のリストが消されてしまう
            namesAndValueList.Add(newLine);
        }
        else
        {
            // 子ノードがある場合、再帰的に処理
            foreach (var child in node.Children)
            {
                ToNamesAndHexStr(child, namesAndValueList, namesCurrent, false);
            }
        }

        //一階層上に戻るため、最後の要素を削除
        if (namesCurrent.Count > 0)
        {
            namesCurrent.RemoveAt(namesCurrent.Count - 1);
        }
    }


}

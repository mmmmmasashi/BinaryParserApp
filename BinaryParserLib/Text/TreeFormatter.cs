using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BinaryParserLib.Text;

internal static class TreeFormatter
{
    /// <summary>
    /// ツリー構造のノードをインデント付きの文字列リストに変換します。
    /// </summary>
    internal static List<string> ToIndentedLines<T>(T node, int depth = 0) where T : ITreeNode<T>
    {
        var tableData = ToTableData(node);
        return tableData.Rows.Select(row => string.Join("\t", row)).ToList();
    }

    internal static TableData ToTableData<T>(T node) where T : ITreeNode<T>
    {
        List<(List<string>, string)> namesAndValueList = ToAlignedTableFormat(node);

        var maxNameDepth = namesAndValueList.Max(n => n.Item1.Count);

        //インデントを調整して、Valueの位置を揃える形でstring化する

        var tableRows = namesAndValueList.Select(pair =>
        {
        var names = pair.Item1;
        var value = pair.Item2;

            //名前の深さに応じて空要素を入れる
            var countOfBlank = maxNameDepth - names.Count;
            var blanks = Enumerable.Repeat(string.Empty, countOfBlank).ToList();
            var ans = names.Concat(blanks).ToList();
            ans.Add(value);
            return ans;
        }).ToList();

        return new TableData(node.Name, tableRows);
    }

    /// <summary>
    /// ツリー構造のノードを、名称と値のペアのリストに変換します。
    /// </summary>
    private static List<(List<string>, string)> ToAlignedTableFormat<T>(T node) where T : ITreeNode<T>
    {
        var namesAndValueList = new List<(List<string>, string)>();
        var names = new List<string>();
        ToNamesAndHexStr(node, namesAndValueList, names, isTopNode:true);
        return namesAndValueList;
    }

    /// <summary>
    /// ツリー構造のノードを再帰的に処理し、名称のリストと値をペアとするリストを作成します。
    /// </summary>
    private static void ToNamesAndHexStr<T>(
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserLib.Text;

public static class TreeFormatter
{
    public static List<string> ToIndentedLines<T>(T node, int depth = 0) where T : ITreeNode<T>
    {
        var namesAndValueList = new List<(List<string>, string)>();
        var names = new List<string>();
        ToNamesAndHexStr(node, namesAndValueList, names);

        var maxNameDepth = namesAndValueList.Max(n => n.Item1.Count);

        //インデントを調整して、Valueの位置を揃える形でstring化する

        return namesAndValueList.Select(pair =>
        {
            var names = pair.Item1;
            var value = pair.Item2;

            //名前の深さに応じてインデントを調整
            const char tab = '\t';
            var indent = new string(tab, (maxNameDepth - names.Count));
            var namePart = string.Join(tab, names);
            return $"{namePart}{indent}{tab}{value}";
        }).ToList();
    }

    /// <summary>
    /// ツリー構造のノードを再帰的に処理し、名称のリストと値をペアとするリストを作成します。
    /// </summary>
    private static void ToNamesAndHexStr<T>(T node, List<(List<string>, string)> namesAndValueList, List<string> namesCurrent) where T : ITreeNode<T>
    {
        namesCurrent.Add(node.Name);

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
                ToNamesAndHexStr(child, namesAndValueList, namesCurrent);
            }
        }

        //一階層上に戻るため、最後の要素を削除
        if (namesCurrent.Count > 0)
        {
            namesCurrent.RemoveAt(namesCurrent.Count - 1);
        }
    }


}

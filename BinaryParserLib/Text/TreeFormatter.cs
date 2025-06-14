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
        var lines = new List<string>();

        // 現在のノードの行を追加（インデント付き）
        lines.Add(new string('\t', depth) + node.ToLine());

        // 子ノードを再帰的に処理
        foreach (var child in node.Children)
        {
            lines.AddRange(ToIndentedLines(child, depth + 1));
        }

        return lines;
    }
}

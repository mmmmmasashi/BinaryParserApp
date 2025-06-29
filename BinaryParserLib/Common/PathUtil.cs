using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserLib.Common
{
    public class PathUtil
    {

        /// <summary>
        /// 先頭と末尾にあるダブルクォーテーションがあれば削除する
        /// </summary>
        public static string RemoveDoubleQuatation(string filePath)
        {
            if (filePath.StartsWith("\"") && filePath.EndsWith("\""))
            {
                return filePath[1..^1]; // 先頭と末尾のダブルクォーテーションを削除
            }
            return filePath; // 変更なし
        }
    }
}

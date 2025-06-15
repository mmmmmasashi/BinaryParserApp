using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserLib.Text;
 
public  interface ITreeNode<T> : ILine {
    public List<T> Children { get; }
}

public interface ILine
{
    public string ToLine();
    public string Name { get; }
    public string HexStr { get; }
}
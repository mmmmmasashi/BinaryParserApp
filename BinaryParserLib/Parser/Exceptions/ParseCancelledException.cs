using System;

namespace BinaryParserLib.Parser.Exceptions
{
    public class ParseCancelledException : Exception
    {
        public ParseCancelledException() : base("Parse operation was cancelled by user.") { }
    }
}
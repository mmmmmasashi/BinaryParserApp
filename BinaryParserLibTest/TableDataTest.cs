using BinaryParserLib.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BinaryParserLibTest
{
    public class TableDataTest
    {
        [Fact]
        public void TableDataのヘッダー作成テスト_optionなし()
        {
            var rows = new List<List<string>>
            {
                new List<string> { "data1", "data1-1", "01" },
                new List<string> { "data1", "data1-2", "AB" }
            };
            var tableData = new TableData("TestProtocol", rows, new List<string>() { "h1", "h2", "data"} );
            var headers = tableData.GetHeaderNames();
            Assert.Equal(3, headers.Count);
            Assert.Equal("h1", headers[0]);
            Assert.Equal("h2", headers[1]);
            Assert.Equal("data", headers[2]);
        }

        [Fact]
        public void TableDataのヘッダー作成テスト_numberオプションあり()
        {
            var option = new TableFormatOption()
            {
                UseNumberOption = true
            };

            var rows = new List<List<string>>
            {
                new List<string> { "data1", "data1-1", "01" },
                new List<string> { "data1", "data1-2", "AB" }
            };
            var headersContent = new List<string> { "No.", "h1", "h2", "data" };
            var tableData = new TableData("TestProtocol", rows, headersContent);
            var headers = tableData.GetHeaderNames();
            Assert.Equal(4, headers.Count);
            Assert.Equal("No.", headers[0]);
            Assert.Equal("h1", headers[1]);
            Assert.Equal("h2", headers[2]);
            Assert.Equal("data", headers[3]);
        }
    }
}

using BinaryParserLib.Common;
using BinaryParserLib.Setting;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Xunit;

namespace BinaryParserLibTest
{
    public class SettingsFileHistoryTest
    {
        [Fact]
        public void 新しいパスを追加できる()
        {
            var history = new SettingsFileHistory();
            Assert.Empty(history.GetHistory());

            var testPath = "test/path/file.json";

            // Act
            history.Add(testPath);

            Assert.Single(history.GetHistory());
            Assert.Contains(testPath, history.GetHistory());
        }

        [Fact]
        public void 同じパスは重複して追加されない()
        {
            var history = new SettingsFileHistory();
            Assert.Empty(history.GetHistory());
            var testPath = "test/path/file.json";
            // Act
            history.Add(testPath);
            history.Add(testPath);
            // Assert
            var currentHistory = history.GetHistory().ToArray();
            Assert.Single(currentHistory);
            Assert.Contains(testPath, currentHistory);
        }


        [Fact]
        public void 永続化された履歴を読み込める()
        {
            // Arrange
            var storage = new StringCollection();
            storage.Add("test1.json");
            storage.Add("test2.json");

            // Act
            var history = new SettingsFileHistory(storage);

            // Assert
            var loadedHistory = history.GetHistory().ToArray();
            Assert.Equal(2, loadedHistory.Length);
            Assert.Contains("test1.json", loadedHistory);
            Assert.Contains("test2.json", loadedHistory);
        }

        [Fact]
        public void 履歴を永続化して保存できる()
        {
            // Arrange
            var history = new SettingsFileHistory();
            var storage = new StringCollection();

            // Act
            history.Add("test1.json");
            history.Add("test2.json");
            history.SaveToStorage(storage);

            // Assert
            Assert.Equal(2, storage.Count);
            Assert.Contains("test1.json", storage.Cast<string>());
            Assert.Contains("test2.json", storage.Cast<string>());
        }

        [Fact]
        public void 使ったものは並び順が先頭に上がる()
        {
            // Arrange
            var history = new SettingsFileHistory();
            history.Add("test1.json");
            history.Add("test2.json");
            history.Add("test3.json");

            // Act
            var initialHistory = history.GetHistory().ToArray();
            Assert.Equal(3, initialHistory.Length);
            Assert.Contains("test3.json", initialHistory);
            Assert.Contains("test2.json", initialHistory);
            Assert.Contains("test1.json", initialHistory);

            // 使ったものを先頭に上げる
            history.Add("test2.json");
            // Assert
            var updatedHistory = history.GetHistory().ToArray();
            Assert.Equal(3, updatedHistory.Length);
            Assert.Equal("test2.json", updatedHistory[0]);
            Assert.Equal("test3.json", updatedHistory[1]);
            Assert.Equal("test1.json", updatedHistory[2]);
        }

        [Fact]
        public void 最大数はデフォルトでは20件に制限する()
        {
            // Arrange
            var history = new SettingsFileHistory();
            for (int i = 1; i <= 25; i++)
            {
                history.Add($"test{i}.json");
            }
            // Act
            var currentHistory = history.GetHistory().ToArray();
            // Assert
            Assert.Equal(20, currentHistory.Length);
            for (int i = 0; i < 20; i++)
            {
                Assert.Equal($"test{25 - i}.json", currentHistory[i]);
            }
        }
    }
}
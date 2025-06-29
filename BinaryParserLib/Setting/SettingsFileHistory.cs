using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserLib.Setting
{
    public class SettingsFileHistory
    {
        private readonly int MaxHistoryCount = 20; // 最大履歴数
        private List<string> _history = new List<string>();

        public SettingsFileHistory(StringCollection? storage = null)
        {
            if (storage == null) return;

            foreach (var item in storage)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    _history.Add(item);
                }
            }
        }

        public void Add(string testPath)
        {
            if (_history.Contains(testPath))
            {
                //既存なら削除
                _history.Remove(testPath);
            }

            //先頭に追加しなおす
            _history.Insert(0, testPath);

            //履歴数が最大を超えたら最後の要素を削除
            if (_history.Count > MaxHistoryCount)
            {
                _history.RemoveAt(_history.Count - 1);
            }

        }

        public List<string> GetHistory()
        {
            return _history.ToList();
        }

        public void SaveToStorage(StringCollection storage)
        {
            storage.Clear();
            foreach (var item in _history)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    storage.Add(item);
                }
            }
        }
    }
}

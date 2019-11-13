using System;
using System.Collections.Generic;
using System.Text;
using IniParser;
using IniParser.Model;

namespace YGO233
{
    public class ConfParser
    {
        private FileIniDataParser parser;
        private IniData data;
        private string filename;

        public ConfParser()
        {
            parser = new FileIniDataParser();
            parser.Parser.Configuration.CommentString = "#";
            parser.Parser.Configuration.NewLineStr = "\n";
        }

        public void Load(string name)
        {
            filename = name;
            data = parser.ReadFile(filename, Encoding.UTF8);
        }

        private void Save()
        {
            parser.WriteFile(filename, data, Encoding.UTF8);
        }

        public string GetStringValue(string key, string _default = "")
        {
            return data.Global[key] ?? _default;
        }

        public void SetStringValue(string key, string value)
        {
            data.Global[key] = value;
            Save();
        }

        public bool GetBoolValue(string key, bool _default = false)
        {
            int val;
            if (int.TryParse(data.Global[key], out val))
                return val > 0;
            else
                return _default;
        }

        public void SetBoolValue(string key, bool value)
        {
            data.Global[key] = value ? "1" : "0";
            Save();
        }

        public int GetIntValue(string key, int _default = 0)
        {
            int val;
            if (int.TryParse(data.Global[key], out val))
                return val;
            else
                return _default;
        }

        public void SetIntValue(string key, int value)
        {
            data.Global[key] = value.ToString();
            Save();
        }

        public UInt64 GetUInt64Value(string key, UInt64 _default = 0)
        {
            UInt64 val;
            if (UInt64.TryParse(data.Global[key], out val))
                return val;
            else
                return _default;
        }

        public void SetUInt64Value(string key, UInt64 value)
        {
            data.Global[key] = value.ToString();
            Save();
        }
    }
}

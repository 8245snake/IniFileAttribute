using System;

namespace Rakuraku.Config.IniFile.Store
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IniDataAttribute : Attribute
    {
        public string FileName { get; }
        public string Section { get; }
        public string Key { get; }
        public string Default { get; }

        public IniDataAttribute(string fileName, string section, string key, string defVal = "")
        {
            FileName = fileName;
            Section = section;
            Key = key;
            Default = defVal;
        }
    }
}
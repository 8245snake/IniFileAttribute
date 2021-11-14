namespace Rakuraku.Config.IniFile.Store
{
    public interface IIniDataRepository
    {
        string GetIniData(string file, string section, string key, string defaultVal = "");
    }
}
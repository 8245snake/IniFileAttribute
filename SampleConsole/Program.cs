using System;
using System.Diagnostics;
using Rakuraku.Config.IniFile.Store;

namespace SampleConsole
{
    class LoggingTest
    {
        [STAThread]
        static void Main(string[] args)
        {
            IniDataSource.IniDataRepository = new SampleRepository();
            Test1();

            Console.ReadLine();
        }

        /// <summary>
        /// プロパティ読み込みを繰り返して時間を測定してみる
        /// </summary>
        private static void Test1()
        {
            int count = 10000;

            MyClass myClass = new MyClass();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < count; i++)
            {
                var name = myClass.Name;
            }

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        
        }
    }


    public class MyClass : IniDataSource
    {
        [IniData("common.ini", "all", "name")]
        public string Name { get; set; }

    }


    public class SampleRepository : IIniDataRepository
    {
        public string GetIniData(string file, string section, string key, string defaultVal = "")
        {
            return "データ";
        }

    }
}
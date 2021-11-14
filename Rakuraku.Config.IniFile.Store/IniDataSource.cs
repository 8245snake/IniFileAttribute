using System;
using System.Runtime.Remoting.Proxies;

namespace Rakuraku.Config.IniFile.Store
{
    [IniFile]
    public class IniDataSource : ContextBoundObject
    {
        public static IIniDataRepository IniDataRepository { get; set; }
    }


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class IniFileAttribute : ProxyAttribute
    {
        public override MarshalByRefObject CreateInstance(Type serverType)
        {
            MarshalByRefObject target = base.CreateInstance(serverType);
            RealProxy rp = new IniDataProxy(target, serverType, IniDataSource.IniDataRepository);
            return rp.GetTransparentProxy() as MarshalByRefObject;
        }
    }
}
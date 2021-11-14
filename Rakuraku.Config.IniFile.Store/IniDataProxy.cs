using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Services;

namespace Rakuraku.Config.IniFile.Store
{
    internal class IniDataProxy : RealProxy
    {
        private readonly MarshalByRefObject _target;
        private readonly IIniDataRepository _dataRepository;
        private readonly Dictionary<string, ReturnMessage> _getterValueDictionary = new Dictionary<string, ReturnMessage>();
        private readonly Dictionary<string, IniDataAttribute> _getterAttributeDictionary = new Dictionary<string, IniDataAttribute>();

        public IniDataProxy(MarshalByRefObject target, Type t, IIniDataRepository dataRepository)
            : base(t)
        {
            _target = target;
            _dataRepository = dataRepository;

            foreach (PropertyInfo property in t.GetProperties().Where(prop => prop.GetCustomAttributes(false).OfType<IniDataAttribute>().Any()))
            {
                string gettrName = $"get_{property.Name}";
                _getterValueDictionary.Add(gettrName, null);

                IniDataAttribute attr = property.GetCustomAttributes(false).OfType<IniDataAttribute>().First();
                _getterAttributeDictionary.Add(gettrName, attr);
            }

        }

        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage call = msg as IMethodCallMessage;

            string methodName = call.MethodName;
            if (_getterValueDictionary.TryGetValue(methodName, out var value))
            {

                if (value != null)
                {
                    return value;
                }

                var attr = _getterAttributeDictionary[methodName];
                var iniVal = _dataRepository.GetIniData(attr.FileName, attr.Section, attr.Key, attr.Default);
                var ret = new ReturnMessage(iniVal, null, 0, call.LogicalCallContext, call); ;
                _getterValueDictionary[methodName] = ret;
                return ret;
            }

            if (call is IConstructionCallMessage ctor)
            {
                RealProxy rp = RemotingServices.GetRealProxy(_target);
                rp.InitializeServerObject(ctor);
                MarshalByRefObject tp = this.GetTransparentProxy() as MarshalByRefObject;
                return EnterpriseServicesHelper.CreateConstructionReturnMessage(ctor, tp);
            }

            return RemotingServices.ExecuteMessage(_target, call);
        }
    }
}
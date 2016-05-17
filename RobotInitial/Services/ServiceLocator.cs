using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Services
{
    
    public static class ServiceLocator
    {
        static Dictionary<Type, object> _map = new Dictionary<Type, object>();

        public static void RegisterService<TService>(TService service)
            where TService : class
        {
            _map[typeof(TService)] = service;
        }

        public static TService GetService<TService>()
            where TService : class
        {
            object service;
            _map.TryGetValue(typeof(TService), out service);
            return service as TService;
        }
    }
}

using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace TaskAutomapper.Mapping
{
    class AutoMapperConfig
    {
        public static void Config()
        {
            var types = Assembly.GetExecutingAssembly().GetExportedTypes();
            var maps = (types.SelectMany(t => t.GetInterfaces(), (t, i) => new {t, i})
                .Where(type => typeof (IHaveCustomMappings).IsAssignableFrom(type.t) &&
                              !type.t.IsAbstract &&
                              !type.t.IsInterface).Select(type => (IHaveCustomMappings) Activator.CreateInstance(type.t))).ToArray();

            foreach (var map in maps)
            {
                Mapper.Initialize(cfg =>
                {
                    map.CreateMappings(cfg);
                });
            }
        }
    }
}

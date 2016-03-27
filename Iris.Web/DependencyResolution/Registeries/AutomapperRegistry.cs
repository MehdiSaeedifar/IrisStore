using System.Collections.Generic;
using AutoMapper;
using AutoMapper.Mappers;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Web;

namespace Iris.Web.DependencyResolution.Registeries
{
    public class AutomapperRegistry : Registry
    {
        public AutomapperRegistry()
        {

            For<ConfigurationStore>().Singleton().Use<ConfigurationStore>()
               .Ctor<IEnumerable<IObjectMapper>>().Is(MapperRegistry.Mappers);

            For<IConfigurationProvider>().Use(ctx => ctx.GetInstance<ConfigurationStore>());

            For<IConfiguration>().Use(ctx => ctx.GetInstance<ConfigurationStore>());

            For<ITypeMapFactory>().Use<TypeMapFactory>();

            For<IMappingEngine>().Use(() => Mapper.Engine);
            //.SelectConstructor(() => Mapper.Engine);

            this.Scan(scanner =>
            {
                scanner.AssembliesFromApplicationBaseDirectory();

                scanner.ConnectImplementationsToTypesClosing(typeof(ITypeConverter<,>))
                       .OnAddedPluginTypes(t => t.HybridHttpOrThreadLocalScoped());

                scanner.ConnectImplementationsToTypesClosing(typeof(ValueResolver<,>))
                    .OnAddedPluginTypes(t => t.HybridHttpOrThreadLocalScoped());

            });
        }
    }
}
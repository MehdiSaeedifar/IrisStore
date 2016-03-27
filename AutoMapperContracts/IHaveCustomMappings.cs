using AutoMapper;

namespace AutoMapperContracts
{
    public interface IHaveCustomMappings
    {
        void CreateMappings(IConfiguration configuration);
    }
}

using AutoMapper;

namespace PayMe.Services.WebApi.Config
{
    public class CustomMapperInitializer
    {

        public static MapperConfiguration GetMappingConfigurations()
        {
            var config = new MapperConfiguration(c => c.AddProfile(new FrameworkEntityProfile()));
            return config;
        }

    }

    public class FrameworkEntityProfile : Profile
    {

        public FrameworkEntityProfile()
        {
            
        }
    }
}

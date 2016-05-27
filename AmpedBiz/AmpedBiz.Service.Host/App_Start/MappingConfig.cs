using System.Web.Http;
using AmpedBiz.Service.Dto.Mappers;

namespace AmpedBiz.Service.Host.App_Start
{
    public class MappingConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var mapper = config.DependencyResolver.GetService(typeof(IMapper)) as IMapper;
            mapper.Initialze();
        }
    }
}
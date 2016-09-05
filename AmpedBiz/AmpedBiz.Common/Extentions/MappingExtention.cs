using ExpressMapper;

namespace AmpedBiz.Common.Extentions
{
    public static class MappingExtention
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination = null) 
            where TSource : class 
            where TDestination : class
        {
            if (source == null)
                return null;

            return (destination != null)
                ? Mapper.Map<TSource, TDestination>(source, destination)
                : Mapper.Map<TSource, TDestination>(source);
        }
    }
}

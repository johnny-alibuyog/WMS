using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressMapper;

namespace AmpedBiz.Common.Extentions
{
    public static class MappingExtention
    {
        //public static TDestination MapAs<TDestination>(this object source)
        //    where TDestination : class
        //{
        //    if (source == null)
        //        return null;

        //    var sourceType = source.GetType();
        //    var destinationType = typeof(TDestination);

        //    return (TDestination)Mapper.Map(source, sourceType, destinationType);
        //}

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination = null) 
            where TSource : class 
            where TDestination : class
        {
            if (source == null)
                return null; //return Activator.CreateInstance<TDestination>();

            return (destination != null)
                ? Mapper.Map<TSource, TDestination>(source, destination)
                : Mapper.Map<TSource, TDestination>(source);
        }
    }
}

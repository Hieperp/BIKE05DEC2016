using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCClient.Converters
{
    public interface IConverter<out TDestination, in TSource>
    {
        TDestination Convert(TSource source);
    }
}
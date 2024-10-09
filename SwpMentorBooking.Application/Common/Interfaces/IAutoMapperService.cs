using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwpMentorBooking.Application.Common.Interfaces
{
    public interface IAutoMapperService
    {
        U Map<T, U>(T sourceType, U destinationType);
        IEnumerable<U> MapAll<T, U>(List<T> sourceTypeList);
    }
}

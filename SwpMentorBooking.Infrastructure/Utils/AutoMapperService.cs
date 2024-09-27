using AutoMapper;
using AutoMapper.Configuration;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwpMentorBooking.Infrastructure.Utils
{
    public class AutoMapperService
    {
        private readonly IMapper _mapper;
        public AutoMapperService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public U Map<T, U>(T sourceType, U destinationType)
        {
            return _mapper.Map(sourceType, destinationType);
        }

        public IEnumerable<U> MapAll<T, U>(List<T> sourceTypeList)
        {
            return _mapper.Map<List<T>, IEnumerable<U>>(sourceTypeList);
        }
    }
}

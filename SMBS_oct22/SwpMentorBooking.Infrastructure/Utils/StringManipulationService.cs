using SwpMentorBooking.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwpMentorBooking.Infrastructure.Utils
{
    public class StringManipulationService : IStringManipulationService
    {
        // Split properties that is separated by a delimiter
        public List<string> SplitProperty(string property, string delimiter)
        {
            if (!string.IsNullOrEmpty(property))
            {
                return property.Split(delimiter).Select(s => s.Trim()).ToList();
            }
            else
            {
                return new List<string>();

            }
        }
    }
}

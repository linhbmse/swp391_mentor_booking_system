using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwpMentorBooking.Application.Common.Interfaces
{
    public interface ICSVFileService
    {
        List<T> ReadCSVFile<T>(string filePath);
        List<(T Record, List<string> Errors)> ValidateCSVData<T>(List<T> records);
    }
}

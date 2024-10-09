using SwpMentorBooking.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwpMentorBooking.Infrastructure.Utils
{
    public class PasswordGeneratorService : IPasswordGeneratorService
    {
        private static readonly Random _random = new Random();
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

        public string GeneratePassword(int length = 12)
        {
            return new string(Enumerable.Repeat(_chars, length)
                                        .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}

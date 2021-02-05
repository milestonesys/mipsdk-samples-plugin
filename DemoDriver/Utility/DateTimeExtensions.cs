using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDriver.Utility
{
    public static class DateTimeExtensions
    {
        public static DateTime Truncate(this DateTime input, long resolution)
        {
            return new DateTime(input.Ticks - (input.Ticks % resolution), input.Kind);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS.DesktopUI
{
    public static class TimeParser
    {
        public static bool ParseTime(string text, out TimeSpan result)
        {
            int number = 0;
            if (int.TryParse(text, out number))
            {
                if (number >= 0 && number < 24)
                {
                    result = new TimeSpan(number, 0, 0);
                    return true;
                }
                else
                {
                    result = new TimeSpan();
                    return false;
                }
            }

            return TimeSpan.TryParse(text, out result);
        }
    }
}

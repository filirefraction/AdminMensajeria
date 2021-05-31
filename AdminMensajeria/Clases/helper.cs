using System;
using System.Configuration;

namespace AdminMensajeria.Clases
{
    public class helper
    {
        public static DateTime dateTimeZone(DateTime dateTime)
        {
            TimeZoneInfo systemTimeZoneById = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["TimeZone"].ToString());
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Local, systemTimeZoneById);
        }

        public static string DateToString(DateTime? datetime)
        {
            if (datetime == null)
            {
                return string.Empty;
            }
            else
            {
                return datetime.Value.ToString("dd/MM/yyyy");
            }
        }

        public static string DateTimeToString(DateTime? datetime)
        {
            if (datetime == null)
            {
                return string.Empty;
            }
            else
            {
                return datetime.Value.ToString("dd/MM/yyyy HH:mm");
            }
        }

    }
}
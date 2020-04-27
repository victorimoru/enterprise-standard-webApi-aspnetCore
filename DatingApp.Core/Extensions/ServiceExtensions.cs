using System;

namespace DatingApp.Core.Extensions
{
    public static class ServiceExtensions
    {
        public static int CalculateAge(this DateTime dateTime)
        {
            int age = DateTime.Today.Year - dateTime.Year;
            if (dateTime.AddYears(age).Month > DateTime.Today.Month)
                return age--;
            return age;

        }
    }
}

using System;

namespace Infrastructure.EF.Seeds
{
    public static class Common
    {
         public static  DateTime GetRandomDateTime(DateTime minDate, DateTime? maxDate = null, int offsetDays = 0, bool randomTime = false)
        {
            if(maxDate is null)
                maxDate = DateTime.Today;
            DateTime start = minDate;
            Random random = new Random();
            if(randomTime)
            {
                start = start.AddHours(random.Next(9, 19)).AddMinutes(55);
            }
            int range = (maxDate - minDate).Value.Days + offsetDays;
            return start.AddDays(random.Next(range));
        }
    }
}
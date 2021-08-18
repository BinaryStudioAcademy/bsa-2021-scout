using System;

namespace Infrastructure.EF.Seeds
{
    public static class Common
    {
         public static  DateTime GetRandomDateTime(DateTime minDate, DateTime? maxDate = null, int offsetDays = 0)
        {
            if(maxDate is null)
                maxDate = DateTime.Today;
            DateTime start = new DateTime(2020, 1, 1);
            Random random = new Random();
            int range = (maxDate - minDate).Value.Days + offsetDays;
            return start.AddDays(random.Next(range));
        }
    }
}
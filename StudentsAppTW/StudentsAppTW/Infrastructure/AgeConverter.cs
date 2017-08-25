using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StudentsAppTW.Infrastructure
{
    public class AgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            int age = (int)value;
            if (age >= 16 && age < 100)
            {
                string str = " года";
                int mod = age % 10;
                int[] numList = { 0, 5, 6, 7, 8, 9 };
                if ((age >= 11 && age <= 19) || numList.Contains(mod)) str = " лет";
                else if (mod == 1) str = " год";
                return age + str;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public interface IBaseCard
    {
    }

    public static class BaseCardExtensions
    {
        public static void CopyCardInfo(this IBaseCard card, IBaseCard anotherCard)
        {
            foreach (PropertyInfo propertyInfo in card.GetType().GetProperties())
            {
                propertyInfo.SetValue(card, propertyInfo.GetValue(anotherCard));

            }
        }
    }
}

using System.Collections.Generic;

namespace Diner.Framework
{
    public class NavigationParameters : Dictionary<string, object>
    {
        public static NavigationParameters Create<T>(T value)
        {
            return new NavigationParameters { { typeof(T).FullName, value } };
        }
    }
}
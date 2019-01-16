using Structure.Domain.Classes;
using System.Collections.Generic;

namespace Structure.Helper.Classes
{
    public static class BaseExtension
    {
        public static void Reset<T>(this IEnumerable<T> baseModels) where T : Base
        {
            foreach (T baseModel in baseModels)
            {
                baseModel.Reset();
            }
        }
    }
}

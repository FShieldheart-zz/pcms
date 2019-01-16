using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API.Helper.Classes
{
    public static class ModelStateExtensions
    {
        public static string GetErrors(this ModelStateDictionary input)
        {
            StringBuilder builder = new StringBuilder();
            IList<ModelErrorCollection> errors = input.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            foreach (ModelErrorCollection error in errors)
            {
                builder.Append(error[0].ErrorMessage).Append(" / ");
            }
            string result = builder.ToString();
            return result;
        }
    }
}

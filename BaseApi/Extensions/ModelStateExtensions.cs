using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace BaseApi.Extensions
{
    public static class ModelStateExtensions
    {
        public static List<string> GetErrorMessages(this ModelStateDictionary dictonary)
        {
            return dictonary.SelectMany(m => m.Value.Errors).Select(x => x.ErrorMessage).ToList<string>();
        }
    }
}

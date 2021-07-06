using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace LT.DigitalOffice.CompanyService.Business.Helper
{
    public static class CreateHistoryMessageHelper
    {
        public static string Create<T>(T obj) where T : class
        {
            return JsonSerializer.Serialize(obj);
        }

        public static string Create<T>(T obj, JsonPatchDocument<T> request) where T : class
        {
            Dictionary<string, string> changes = new();

            foreach (var property in obj.GetType().GetProperties())
            {
                var oper = request.Operations.FirstOrDefault(o => o.path.EndsWith(property.Name, StringComparison.OrdinalIgnoreCase));

                if (oper != null
                    && property.GetValue(obj).ToString() == oper.value.ToString())
                {
                    changes.Add(property.Name, property.GetValue(obj).ToString());
                }
            }

            return JsonSerializer.Serialize(changes);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace AzureBottolinoApp
{
    public static class EnviromentalConfig
    {
        public static string CnnValFromSecret(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
        public static string Trimbody(string body)
        {
            string output;
            int start = body.IndexOf("[");
            int end = body.IndexOf("]");
            output = body.Substring(start + 1, end - start - 1);
            return output;
        }
    }
}

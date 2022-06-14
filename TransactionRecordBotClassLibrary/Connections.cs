using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TransactionRecordBotClassLibrary
{
    public static class Connections
    {
        public static string CnnVal(string name)
        {
            // this code is running with app.config
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}

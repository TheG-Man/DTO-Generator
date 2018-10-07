using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    public class PluginLoadException : Exception
    {
        public PluginLoadException()
        {

        }

        public PluginLoadException(string message)
            :base(message)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMigrationTool.Core.Extensions
{
    public static class ExceptionHelpers
    {
        public static void ThrowIfNull(object argument, string argName)
        {
            if (argument == null) throw new ArgumentNullException(argName);
        }
    }
}

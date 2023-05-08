using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMigrationTool.Core.SearchObjects
{
    public class SearchParameters
    {
        public string Query { get; set; }
        public string OrderBy { get; set; }
        public int Top { get; set; }
    }
}

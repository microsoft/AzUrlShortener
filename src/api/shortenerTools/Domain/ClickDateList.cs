using System;
using System.Collections.Generic;

namespace Cloud5mins.domain
{
    public class ClickDateList
    {
        public List<ClickDate> Items { get; set; }
        public string Url { get; set; }
        public ClickDateList(){
            Url     = string.Empty;
        }
        public ClickDateList (List<ClickDate> list)
        {
            Items   = list;
            Url     = string.Empty;
        }
    }
}
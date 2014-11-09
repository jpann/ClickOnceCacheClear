using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClickOnceCacheClear.Events
{
    public class DirectoryDeleteEvent : EventArgs
    {
        public string Directory { get; set; }
        public string Term { get; set; }
    }
}

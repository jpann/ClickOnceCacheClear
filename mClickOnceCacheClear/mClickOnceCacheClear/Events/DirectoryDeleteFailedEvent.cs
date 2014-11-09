using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClickOnceCacheClear.Events
{
    public class DirectoryDeleteFailedEvent : EventArgs
    {
        public string Directory { get; set; }
        public string Term { get; set; }
        public string Message { get; set; }
        public Exception Ex { get; set; }
    }
}

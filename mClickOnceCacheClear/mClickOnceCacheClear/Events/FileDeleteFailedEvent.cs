using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClickOnceCacheClear.Events
{
    public class FileDeleteFailedEvent : EventArgs
    {
        public string File { get; set; }
        public string Term { get; set; }
        public string Message { get; set; }
        public Exception Ex { get; set; }
    }
}

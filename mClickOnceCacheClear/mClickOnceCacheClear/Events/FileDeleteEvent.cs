using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClickOnceCacheClear.Events
{
    public class FileDeleteEvent : EventArgs
    {
        public string File { get; set; }
        public string Term { get; set; }
    }
}

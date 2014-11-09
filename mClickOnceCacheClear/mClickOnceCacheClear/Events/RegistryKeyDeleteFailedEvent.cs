using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClickOnceCacheClear.Events
{
    public class RegistryKeyDeleteFailedEvent : EventArgs
    {
        public string Key { get; set; }
        public string Term { get; set; }
        public string Message { get; set; }
        public Exception Ex { get; set; }
    }
}

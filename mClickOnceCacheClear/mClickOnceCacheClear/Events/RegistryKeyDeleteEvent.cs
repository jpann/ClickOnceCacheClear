﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mClickOnceCacheClear.Events
{
    public class RegistryKeyDeleteEvent : EventArgs
    {
        public string Key { get; set; }
        public string Term { get; set; }
    }
}

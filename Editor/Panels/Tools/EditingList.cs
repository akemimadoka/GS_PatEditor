﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Panels.Tools
{
    interface EditingList<T>
    {
        IEnumerable<T> Data { get; }
    }
}

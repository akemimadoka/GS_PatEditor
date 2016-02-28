using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor
{
    interface ClipboardHandler
    {
        string DataID { get; }

        bool SelectedAvailable { get; }
        bool ClipboardDataAvailable(object data);

        object Copy();
        void Delete();
        void Paste(object data);
    }
}

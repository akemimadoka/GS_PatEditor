using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Simulation
{
    public interface ActionProvider
    {
        Pat.Action GetActionByID(string id);
    }
}

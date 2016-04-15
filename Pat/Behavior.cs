using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Pat
{
    public class ActionEffects
    {
        public EffectList InitEffects;
        public EffectList UpdateEffects;
        public List<EffectList> KeyFrameEffects;

        public ActionEffects(Action action)
        {
            InitEffects = new EffectList { Effects = new List<Effect>(action.InitEffects) };
            UpdateEffects = new EffectList { Effects = new List<Effect>(action.UpdateEffects) };
            KeyFrameEffects = action.KeyFrameEffects.Select(x =>
                new EffectList { Effects = new List<Effect>(x) }).ToList();
        }
    }

    [Serializable]
    public abstract class Behavior
    {
        public abstract void MakeEffects(ActionEffects effects);
    }
}

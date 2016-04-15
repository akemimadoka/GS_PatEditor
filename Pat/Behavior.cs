using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Pat
{
    public class ActionEffects
    {
        public int SegmentCount { get; private set; }
        public EffectList InitEffects;
        public EffectList UpdateEffects;
        public List<EffectList> KeyFrameEffects;

        public ActionEffects(Action action)
        {
            SegmentCount = action.Segments.Count;

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

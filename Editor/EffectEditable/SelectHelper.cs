using GS_PatEditor.Pat.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.EffectEditable
{
    static class SelectHelper
    {
        public static T Create<T>(Type type, EditableEnvironment env)
        {
            T ret = (T)type.GetConstructor(new Type[0]).Invoke(new object[0]);
            if (ret is IEditableEnvironment)
            {
                ((IEditableEnvironment)ret).Environment = env;
            }
            return ret;
        }
    }
}

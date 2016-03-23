using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Panels.Tools.HitAttack
{
    class HitAtackBoxListDataProvider : EditingList<EditingHitAttackBox>
    {
        //TODO check readonly fields
        private readonly Editor _Editor;
        private readonly Func<Pat.Frame, List<Pat.Box>> _BoxList;

        public HitAtackBoxListDataProvider(Editor editor, Func<Pat.Frame, List<Pat.Box>> boxList)
        {
            editor.Frame.OnReset += ResetDataList;

            _Editor = editor;
            _BoxList = boxList;

            ResetDataList();
        }

        public void ResetDataList()
        {
            //TODO reuse items in the list
            DataList.Clear();

            var frame = _Editor.Frame.FrameData;
            if (frame != null)
            {
                var boxList = _BoxList(frame);
                for (int i = 0; i < boxList.Count; ++i)
                {
                    DataList.Add(new HitAttackBoxDataProvider(_Editor, boxList[i], i));
                }
            }
        }

        public readonly List<HitAttackBoxDataProvider> DataList = new List<HitAttackBoxDataProvider>();

        public IEnumerable<EditingHitAttackBox> Data
        {
            get
            {
                return DataList.AsEnumerable<EditingHitAttackBox>();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Editor.Panels.Tools.Hit
{
    class HitBoxListDataProvider : EditingList<EditingHitAttackBox>
    {
        //TODO check readonly fields
        private readonly Editor _Editor;

        public HitBoxListDataProvider(Editor editor)
        {
            editor.EditorNode.Animation.Frame.OnReset += ResetDataList;

            _Editor = editor;

            ResetDataList();
        }

        public void ResetDataList()
        {
            //TODO reuse items in the list
            DataList.Clear();

            var frame = _Editor.EditorNode.Animation.Frame.FrameData;
            if (frame != null)
            {
                for (int i = 0; i < frame.HitBoxes.Count; ++i)
                {
                    DataList.Add(new HitBoxDataProvider(_Editor, frame.HitBoxes[i], i));
                }
            }
        }

        public readonly List<HitBoxDataProvider> DataList = new List<HitBoxDataProvider>();

        public IEnumerable<EditingHitAttackBox> Data
        {
            get
            {
                return DataList.AsEnumerable<EditingHitAttackBox>();
            }
        }
    }
}

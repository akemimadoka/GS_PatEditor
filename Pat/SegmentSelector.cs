using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Pat
{
    [Serializable]
    public class SegmentSelector
    {
        public readonly List<int> IndexList = new List<int>();

        public string Index
        {
            get
            {
                return String.Join(",", IndexList);
            }
            set
            {
                var list = value.Split(',');
                var listInt = new List<int>();
                foreach (var i in list)
                {
                    int ii;
                    if (Int32.TryParse(i, out ii) && ii > 0)
                    {
                        listInt.Add(ii);
                    }
                    else
                    {
                        return;
                    }
                }
                IndexList.Clear();
                IndexList.AddRange(listInt);
            }
        }
    }
}

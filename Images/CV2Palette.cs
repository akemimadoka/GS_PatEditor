using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Images
{
    class CV2Palette
    {
        public static Color[] ReadPaletteFile(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                var ret = new Color[256];
                stream.ReadByte();
                byte[] buffer = new byte[2];
                for (int i = 0; i < ret.Length; ++i)
                {
                    stream.Read(buffer, 0, 2);
                    ret[i] = FromBGRA5551(BitConverter.ToInt16(buffer, 0));
                }
                return ret;
            }
        }
        private static Color FromBGRA5551(Int16 val)
        {
            int b = (val & ((1 << 5) - 1)) >> 0 << 3;
            int g = (val & ((1 << 10) - 1)) >> 5 << 3;
            int r = (val & ((1 << 15) - 1)) >> 10 << 3;
            int a = ((val & (1 << 15)) != 0) ? 255 : 0;
            return Color.FromArgb(a, r, g, b);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Images
{
    class DDSImage
    {
        private static void Main()
        {
            foreach (var filename in System.IO.Directory.EnumerateFiles(@"E:\Games\[game]GRIEFSYNDROME\griefsyndrome\gs00",
                "*.dds", System.IO.SearchOption.AllDirectories))
            {
                using (var file = System.IO.File.OpenRead(filename))
                {
                    using (var reader = new System.IO.BinaryReader(file))
                    {
                        var bitmap = DDSLoader.LoadDDS(reader);
                        if (bitmap != null)
                        {
                            bitmap.Save(@"E:\convert.png", System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }
                }
            }
        }
    }
}

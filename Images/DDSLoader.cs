using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GS_PatEditor.Images
{
    class DDSLoader
    {
        #region Header
        enum D3d10ResourceDimension : uint
        {
            Unknown    = 0,
            Buffer     = 1,
            Texture1D  = 2,
            Texture2D  = 3,
            Texture3D  = 4,
        };

        [Flags]
        enum DdsPixelFormatFlags
        {
            DDPF_ALPHAPIXELS = 0x1,
            DDPF_ALPHA = 0x2,
            DDPF_FOURCC = 0x4,
            DDPF_RGB = 0x40,
            DDPF_YUV = 0x200,
            DDPF_LUMINACE = 0x20000,
        };

        struct DdsPixelFormat
        {
            public UInt32 size;
            public DdsPixelFormatFlags flags;
            public byte[] four_cc;
            public UInt32 rgb_bit_count;
            public UInt32 r_bit_mask;
            public UInt32 g_bit_mask;
            public UInt32 b_bit_mask;
            public UInt32 a_bit_mask;
        };

        struct DdsHeaderDx10
        {
            public UInt32 dxgi_format;
            public D3d10ResourceDimension resource_dimension;
            public UInt32 misc_flag;
            public UInt32 array_size;
            public UInt32 misc_flags2;
        };

        enum DdsHeaderFlags
        {
            DDSD_CAPS        = 0x1,
            DDSD_HEIGHT      = 0x2,
            DDSD_WIDTH       = 0x4,
            DDSD_PITCH       = 0x8,
            DDSD_PIXELFORMAT = 0x1000,
            DDSD_MIPMAPCOUNT = 0x20000,
            DDSD_LINEARSIZE  = 0x80000,
            DDSD_DEPTH       = 0x800000,
        };

        struct DdsHeader
        {
            public UInt32 size;
            public DdsHeaderFlags flags;
            public UInt32 height;
            public UInt32 width;
            public UInt32 pitch_or_linear_size;
            public UInt32 depth;
            public UInt32 mip_map_count;
            public DdsPixelFormat pixel_format;
            public UInt32[] caps;
        };
        #endregion
        #region Magic
        private class MagicNumber
        {
            private byte[] _Data;

            private MagicNumber(string str)
            {
                _Data = Encoding.ASCII.GetBytes(str);
            }

            public static implicit operator MagicNumber(string str)
            {
                return new MagicNumber(str);
            }

            public bool Equals(byte[] data)
            {
                if (data == null || data.Length != _Data.Length)
                {
                    return false;
                }
                for (int i = 0; i < _Data.Length; ++i)
                {
                    if (data[i] != _Data[i])
                    {
                        return false;
                    }
                }
                return true;
            }

            public int Length { get { return _Data.Length; } }
        }

        private static readonly MagicNumber magic = "DDS\x20";
        private static readonly MagicNumber magic_dxt1 = "DXT1";
        private static readonly MagicNumber magic_dxt2 = "DXT2";
        private static readonly MagicNumber magic_dxt3 = "DXT3";
        private static readonly MagicNumber magic_dxt4 = "DXT4";
        private static readonly MagicNumber magic_dxt5 = "DXT5";
        private static readonly MagicNumber magic_dx10 = "DX10";
        #endregion
        #region Read Header
        private static void ReadPixelFormat(BinaryReader reader, ref DdsPixelFormat pixel_format)
        {
            pixel_format.size = reader.ReadUInt32();
            pixel_format.flags = (DdsPixelFormatFlags)(reader.ReadUInt32());
            pixel_format.four_cc = reader.ReadBytes(4);
            pixel_format.rgb_bit_count = reader.ReadUInt32();
            pixel_format.r_bit_mask = reader.ReadUInt32();
            pixel_format.g_bit_mask = reader.ReadUInt32();
            pixel_format.b_bit_mask = reader.ReadUInt32();
            pixel_format.a_bit_mask = reader.ReadUInt32();
        }
        private static void ReadHeader(BinaryReader reader, ref DdsHeader header)
        {
            header.size = reader.ReadUInt32();
            header.flags = (DdsHeaderFlags)(reader.ReadUInt32());
            header.height = reader.ReadUInt32();
            header.width = reader.ReadUInt32();
            header.pitch_or_linear_size = reader.ReadUInt32();
            header.depth = reader.ReadUInt32();
            header.mip_map_count = reader.ReadUInt32();
            reader.ReadBytes(4 * 11);
            ReadPixelFormat(reader, ref header.pixel_format);
            header.caps = new UInt32[4];
            for (int i = 0; i < 4; ++i)
            {
                header.caps[i] = reader.ReadUInt32();
            }
            reader.ReadBytes(4);
        }
        private static void ReadHeaderDx10(BinaryReader reader, ref DdsHeaderDx10 header)
        {
            header.dxgi_format = reader.ReadUInt32();
            header.resource_dimension = (D3d10ResourceDimension)(reader.ReadUInt32());
            header.misc_flag = reader.ReadUInt32();
            header.array_size = reader.ReadUInt32();
            header.misc_flags2 = reader.ReadUInt32();
        }
        #endregion
        #region Decoder
        private static UInt32 ColorBGR565ToBGRA(UInt16 c, UInt32 alpha)
        {
            UInt32 b = (UInt32)((c & 0x001F) << 3);
            UInt32 g = (UInt32)((c & 0x07E0) >> 3);
            UInt32 r = (UInt32)((c & 0xF800) >> 8);
            return b | g << 8 | r << 16 | alpha << 24;
        }
        private static UInt32 Interpolate3(UInt32 colorNear, UInt32 colorFar)
        {
            var c0 = ((colorNear & 255) * 2 + (colorFar & 255)) / 3;
            var c1 = ((colorNear >> 8 & 255) * 2 + (colorFar >> 8 & 255)) / 3;
            var c2 = ((colorNear >> 16 & 255) * 2 + (colorFar >> 16 & 255)) / 3;
            var c3 = ((colorNear >> 24 & 255) * 2 + (colorFar >> 24 & 255)) / 3;
            return c0 | c1 << 8 | c2 << 16 | c3 << 24;
        }
        private static UInt32 Interpolate2(UInt32 a, UInt32 b)
        {
            var c0 = ((a & 255) + (b & 255)) / 2;
            var c1 = ((a >> 8 & 255) + (b >> 8 & 255)) / 2;
            var c2 = ((a >> 16 & 255) + (b >> 16 & 255)) / 2;
            var c3 = ((a >> 24 & 255) + (b >> 24 & 255)) / 2;
            return c0 | c1 << 8 | c2 << 16 | c3 << 24;
        }
        //result should be UInt32[], but Marshal.Copy only support int[], so we have to use int here.
        //set result as BGRA, but only overwrite BGR, keeping A
        private static void DecodeBC1Block(BinaryReader reader, UInt32[] paletteBuffer, int[] result, int offset, int stride)
        {
            var c0 = ColorBGR565ToBGRA(reader.ReadUInt16(), 0);
            var c1 = ColorBGR565ToBGRA(reader.ReadUInt16(), 0);
            paletteBuffer[0] = c0;
            paletteBuffer[1] = c1;

            //not sure how to compare them
            if (c0 > c1)
            {
                paletteBuffer[2] = Interpolate3(c0, c1);
                paletteBuffer[3] = Interpolate3(c1, c0);
            }
            else
            {
                paletteBuffer[2] = Interpolate2(c0, c1);
                paletteBuffer[3] = 0u;
            }

            var lookup = reader.ReadUInt32();
            for (int y = 0; y < 4; ++y)
            {
                for (int x = 0; x < 4; ++x)
                {
                    var resultIndex = offset + x + y * stride;
                    result[resultIndex] &= unchecked((int)0xFF000000);
                    result[resultIndex] |= (int)paletteBuffer[lookup & 3];
                    lookup >>= 2;
                }
            }
        }
        //set result as BGRA but only overwrite A, keeping BGR
        private static void DecodeBC4Block(BinaryReader reader, byte[] paletteBuffer, int[] result, int offset, int stride)
        {
            byte c0 = reader.ReadByte();
            byte c1 = reader.ReadByte();
            paletteBuffer[0] = c0;
            paletteBuffer[1] = c1;

            if (c0 > c1)
            {
                for (int i = 2; i < 8; ++i)
                {
                    paletteBuffer[i] = (byte)(((8.0f - i) * paletteBuffer[0] + (i - 1.0f) * paletteBuffer[1]) / 7.0f);
                }
            }
            else
            {
                for (int i = 2; i < 6; ++i)
                {
                    paletteBuffer[i] = (byte)(((6.0f - i) * paletteBuffer[0] + (i - 1.0f) * paletteBuffer[1]) / 5.0f);
                }
                paletteBuffer[6] = 0;
                paletteBuffer[7] = 255;
            }
            
            for (int i = 0; i < 2; ++i)
            {
                UInt32 lookup = reader.ReadByte();
                lookup |= ((UInt32)reader.ReadByte()) << 8;
                lookup |= ((UInt32)reader.ReadByte()) << 16;
                for (int j = 0; j < 8; ++j)
                {
                    var pos = i * 8 + j;
                    var x = pos % 4;
                    var y = pos / 4;
                    var resultIndex = offset + x + y * stride;
                    result[resultIndex] &= 0x00FFFFFF;
                    result[resultIndex] |= paletteBuffer[lookup & 7] << 24;
                    lookup >>= 3;
                }
            }
        }
        private static Bitmap DecodeDXT1(BinaryReader reader, UInt32 width, UInt32 height)
        {
            return null;
            var ret = new Bitmap((int)width, (int)height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var locked = ret.LockBits(new Rectangle(0, 0, (int)width, (int)height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int stride = locked.Stride / 4; //number of pixels in a line
            var buffer = new int[stride * 4]; //four lines
            var pal = new UInt32[4];

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    DecodeBC1Block(reader, pal, buffer, x, stride);
                }
                Marshal.Copy(buffer, 0, locked.Scan0 + locked.Stride * y, buffer.Length); //last parameter is number of elements
            }

            ret.UnlockBits(locked);
            return ret;
        }
        private static Bitmap DecodeDXT5(BinaryReader reader, UInt32 width, UInt32 height)
        {
            var ret = new Bitmap((int)width, (int)height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var locked = ret.LockBits(new Rectangle(0, 0, (int)width, (int)height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int stride = locked.Stride / 4; //number of pixels in a line
            var buffer = new int[stride * 4]; //four lines
            var pal1 = new UInt32[4];
            var pal2 = new byte[8];

            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    DecodeBC4Block(reader, pal2, buffer, x, stride);
                    DecodeBC1Block(reader, pal1, buffer, x, stride);
                }
                Marshal.Copy(buffer, 0, locked.Scan0 + locked.Stride * y, buffer.Length); //last parameter is number of elements
            }

            ret.UnlockBits(locked);
            return ret;
        }
        #endregion

        public static Bitmap LoadDDS(BinaryReader reader)
        {
            if (!magic.Equals(reader.ReadBytes(magic.Length)))
            {
                return null;
            }

            var header = new DdsHeader();
            ReadHeader(reader, ref header);
            if (magic_dx10.Equals(header.pixel_format.four_cc))
            {
                var header10 = new DdsHeaderDx10();
                ReadHeaderDx10(reader, ref header10);
            }

            var width = header.width;
            var height = header.height;

            if (header.pixel_format.flags.HasFlag(DdsPixelFormatFlags.DDPF_FOURCC))
            {
                if (magic_dxt1.Equals(header.pixel_format.four_cc))
                    return DecodeDXT1(reader, width, height);
                else if (magic_dxt5.Equals(header.pixel_format.four_cc))
                    return DecodeDXT5(reader, width, height);
                else
                {
                    return null;
                }
            }
            else if (header.pixel_format.flags.HasFlag(DdsPixelFormatFlags.DDPF_RGB))
            {
                if (header.pixel_format.rgb_bit_count == 32)
                {
                    //TODO read BGRA8888
                    return null;
                }
            }
            return null;
        }
        
    }
}

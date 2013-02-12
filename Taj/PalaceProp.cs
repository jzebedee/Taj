using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Taj.Messages;
using Taj.Messages.Flags;

namespace Taj
{
	public class PalaceProp
	{
		#region Consts and palette
		const int dither20bit = 255 / 63;
		const int ditherS20Bit = 255 / 31;

		static uint[] clutARGB = new uint[] {
			0xffffffff, 0xffccffff, 0xff99ffff, 0xff66ffff, 0xff33ffff, 0xff00ffff, 0xffffdfff, 0xffccdfff, 
			0xff99dfff, 0xff66dfff, 0xff33dfff, 0xff00dfff, 0xffffbfff, 0xffccbfff, 0xff99bfff, 0xff66bfff, 
			0xff33bfff, 0xff00bfff, 0xffff9fff, 0xffcc9fff, 0xff999fff, 0xff669fff, 0xff339fff, 0xff009fff, 
			0xffff7fff, 0xffcc7fff, 0xff997fff, 0xff667fff, 0xff337fff, 0xff007fff, 0xffff5fff, 0xffcc5fff, 
			0xff995fff, 0xff665fff, 0xff335fff, 0xff005fff, 0xffff3fff, 0xffcc3fff, 0xff993fff, 0xff663fff, 
			0xff333fff, 0xff003fff, 0xffff1fff, 0xffcc1fff, 0xff991fff, 0xff661fff, 0xff331fff, 0xff001fff, 
			0xffff00ff, 0xffcc00ff, 0xff9900ff, 0xff6600ff, 0xff3300ff, 0xff0000ff, 0xffeeeeee, 0xffdddddd, 
			0xffcccccc, 0xffbbbbbb, 0xffffffaa, 0xffccffaa, 0xff99ffaa, 0xff66ffaa, 0xff33ffaa, 0xff00ffaa, 
			0xffffdfaa, 0xffccdfaa, 0xff99dfaa, 0xff66dfaa, 0xff33dfaa, 0xff00dfaa, 0xffffbfaa, 0xffccbfaa, 
			0xff99bfaa, 0xff66bfaa, 0xff33bfaa, 0xff00bfaa, 0xffaaaaaa, 0xffff9faa, 0xffcc9faa, 0xff999faa, 
			0xff669faa, 0xff339faa, 0xff009faa, 0xffff7faa, 0xffcc7faa, 0xff997faa, 0xff667faa, 0xff337faa, 
			0xff007faa, 0xffff5faa, 0xffcc5faa, 0xff995faa, 0xff665faa, 0xff335faa, 0xff005faa, 0xffff3faa, 
			0xffcc3faa, 0xff993faa, 0xff663faa, 0xff333faa, 0xff003faa, 0xffff1faa, 0xffcc1faa, 0xff991faa, 
			0xff661faa, 0xff331faa, 0xff001faa, 0xffff00aa, 0xffcc00aa, 0xff9900aa, 0xff6600aa, 0xff3300aa, 
			0xff0000aa, 0xff999999, 0xff888888, 0xff777777, 0xff666666, 0xffffff55, 0xffccff55, 0xff99ff55, 
			0xff66ff55, 0xff33ff55, 0xff00ff55, 0xffffdf55, 0xffccdf55, 0xff99df55, 0xff66df55, 0xff33df55, 
			0xff00df55, 0xffffbf55, 0xffccbf55, 0xff99bf55, 0xff66bf55, 0xff33bf55, 0xff00bf55, 0xffff9f55, 
			0xffcc9f55, 0xff999f55, 0xff669f55, 0xff339f55, 0xff009f55, 0xffff7f55, 0xffcc7f55, 0xff997f55, 
			0xff667f55, 0xff337f55, 0xff007f55, 0xffff5f55, 0xffcc5f55, 0xff995f55, 0xff665f55, 0xff335f55, 
			0xff005f55, 0xff555555, 0xffff3f55, 0xffcc3f55, 0xff993f55, 0xff663f55, 0xff333f55, 0xff003f55, 
			0xffff1f55, 0xffcc1f55, 0xff991f55, 0xff661f55, 0xff331f55, 0xff001f55, 0xffff0055, 0xffcc0055, 
			0xff990055, 0xff660055, 0xff330055, 0xff000055, 0xff444444, 0xff333333, 0xff222222, 0xff111111, 
			0xffffff00, 0xffccff00, 0xff99ff00, 0xff66ff00, 0xff33ff00, 0xff00ff00, 0xffffdf00, 0xffccdf00, 
			0xff99df00, 0xff66df00, 0xff33df00, 0xff00df00, 0xffffbf00, 0xffccbf00, 0xff99bf00, 0xff66bf00, 
			0xff33bf00, 0xff00bf00, 0xffff9f00, 0xffcc9f00, 0xff999f00, 0xff669f00, 0xff339f00, 0xff009f00, 
			0xffff7f00, 0xffcc7f00, 0xff997f00, 0xff667f00, 0xff337f00, 0xff007f00, 0xffff5f00, 0xffcc5f00, 
			0xff995f00, 0xff665f00, 0xff335f00, 0xff005f00, 0xffff3f00, 0xffcc3f00, 0xff993f00, 0xff663f00, 
			0xff333f00, 0xff003f00, 0xffff1f00, 0xffcc1f00, 0xff991f00, 0xff661f00, 0xff331f00, 0xff001f00, 
			0xffff0000, 0xffcc0000, 0xff990000, 0xff660000, 0xff330000, 0xff000000, 0xff000000, 0xff000000, 
			0xff000000, 0xff000000, 0xff000000, 0xff000000, 0xff000000, 0xff000000, 0xff000000, 0xff000000, 
			0xff000000, 0xff000000, 0xff000000, 0xff000000, 0xff000000, 0xff000000, 0xff000000, 0xff000000, 
			0xff000000, 0xff000000, 0xff000000, 0xff000000, 0xff000000, 0xff000000, 0xff000000, 0xff000000,	
		};
		#endregion

		private static byte[] DeflateData(byte[] Data)
		{
			using (MemoryStream nbstream = new MemoryStream(Data.TrimBuffer(12)), dnbstream = new MemoryStream())
			{
				nbstream.Seek(2, SeekOrigin.Begin);

				using (var dstream = new DeflateStream(nbstream, CompressionMode.Decompress))
				{
					dstream.CopyTo(dnbstream);
				}

				return dnbstream.ToArray();
			}
		}

		private byte[] _data;
		private int _width, _height, _horizontalOffset, _verticalOffset, _scriptOffset;
		private PropFormatFlags _flags;

		public PalaceProp(byte[] data, AssetType type, int ID, uint CRC = 0)
		{
			Process(data);
		}

		void Process(byte[] data)
		{
			if (data[1] == 0)
			{
				_width = data[0] | data[1] << 8;
				_height = data[2] | data[3] << 8;
				_horizontalOffset = data[4] | data[5] << 8;
				_verticalOffset = data[6] | data[7] << 8;
				_scriptOffset = data[8] | data[9] << 8;
				_flags = (PropFormatFlags)(data[10] | data[11] << 8);
			}
			else
			{
				_width = data[1] | data[0] << 8;
				_height = data[3] | data[2] << 8;
				_horizontalOffset = data[5] | data[4] << 8;
				_verticalOffset = data[7] | data[6] << 8;
				_scriptOffset = data[9] | data[8] << 8;
				_flags = (PropFormatFlags)(data[11] | data[10] << 8);
			}

			//rect = new Rectangle(horizontalOffset, verticalOffset, width, height);

			var head = _flags.HasFlag(PropFormatFlags.HEAD);
			var ghost = _flags.HasFlag(PropFormatFlags.GHOST);
			var rare = _flags.HasFlag(PropFormatFlags.RARE);
			var animate = _flags.HasFlag(PropFormatFlags.ANIMATE);
			var bounce = _flags.HasFlag(PropFormatFlags.BOUNCE);

			Bitmap decoded = null;
			if (_flags.HasFlag(PropFormatFlags._32BIT))
			{
				_data = DeflateData(data);
				decoded = decode32BitProp();
			}
			else if (_flags.HasFlag(PropFormatFlags._S20BIT))
			{
				_data = DeflateData(data);
				decoded = decodeS20BitProp();
			}
			else if (_flags.HasFlag(PropFormatFlags._20BIT))
			{
				_data = DeflateData(data);
				decoded = decode20BitProp();
			}
			else if (_flags.HasFlag(PropFormatFlags._16BIT))
			{
				_data = DeflateData(data);
				decoded = decode16BitProp();
			}
			else //if (flags.HasFlag(PropFormatFlags._8BIT))
			{
				_data = data.TrimBuffer(12);
				decoded = decode8BitProp();
			}

			Debug.Assert(decoded != null);

			var tempfile = Path.GetTempFileName() + ".png";
			decoded.Save(tempfile, ImageFormat.Png);
			System.Diagnostics.Process.Start(tempfile);
		}

		private Bitmap MarshalBitmap(int[] bitmapData)
		{
			var pin_bd = GCHandle.Alloc(bitmapData, GCHandleType.Pinned);
			try
			{
				return new Bitmap(_width, _height, ((_width * 32 + 31) & ~31) / 8, PixelFormat.Format32bppArgb, pin_bd.AddrOfPinnedObject());
			}
			finally
			{
				pin_bd.Free();
			}
		}

		private Bitmap decode32BitProp()
		{
			int
				ofst = 0, pos = 0,
				A, R, G, B;

			var ba = new int[_width * _height];
			for (var X = 0; X <= 1935; X++)
			{
				ofst = X * 4;

				R = _data[ofst];
				G = _data[ofst + 1];
				B = _data[ofst + 2];
				A = _data[ofst + 3];

				ba[pos++] = (A << 24 | R << 16 | G << 8 | B);
			}

			return MarshalBitmap(ba);
		}

		private Bitmap decodeS20BitProp()
		{
			int
				x = 0, y = 0, ofst = 0, pos = 0,
				C, A, R, G, B;

			var ba = new int[_width * _height];
			for (var X = 0; X < 968; X++)
			{
				ofst = X * 5;

				R = (((_data[ofst] >> 3) & 31) * ditherS20Bit) & 0xFF; //red
				C = (_data[ofst] << 8) | _data[ofst + 1];
				G = ((C >> 6 & 31) * ditherS20Bit) & 0xFF; //green
				B = ((C >> 1 & 31) * ditherS20Bit) & 0xFF; //blue
				C = (_data[ofst + 1] << 8) | _data[ofst + 2];
				A = ((C >> 4 & 31) * ditherS20Bit) & 0xFF; //alpha

				ba[pos++] = (A << 24 | R << 16 | G << 8 | B);

				C = (_data[ofst + 2] << 8) | _data[ofst + 3];
				R = ((C >> 7 & 31) * ditherS20Bit) & 0xFF; // << 3; //red
				G = ((C >> 2 & 31) * ditherS20Bit) & 0xFF; // << 3; //green
				C = (_data[ofst + 3] << 8) | _data[ofst + 4];
				B = ((C >> 5 & 31) * ditherS20Bit) & 0xFF; // << 3; //blue
				A = ((C & 31) * ditherS20Bit) & 0xFF; // << 3; //alpha				

				ba[pos++] = (A << 24 | R << 16 | G << 8 | B);

				if (++x > 43)
				{
					x = 0;
					y++;
				}
			}

			return MarshalBitmap(ba);
		}

		private Bitmap decode20BitProp()
		{
			int
				ofst = 0, pos = 0,
				C, A, R, G, B;

			var ba = new int[_width * _height];
			for (var X = 0; X < 967; X++)
			{
				ofst = X * 5;

				R = ((_data[ofst] >> 2) & 63) * dither20bit;
				C = (_data[ofst] << 8) | _data[ofst + 1];
				G = ((C >> 4) & 63) * dither20bit;
				C = (_data[ofst + 1] << 8) | _data[ofst + 2];
				B = ((C >> 6) & 63) * dither20bit;
				A = (((C >> 4) & 3) * 85);

				ba[pos++] = (A << 24 | R << 16 | G << 8 | B);

				C = (_data[ofst + 2] << 8) | _data[ofst + 3];
				R = ((C >> 6) & 63) * dither20bit;
				G = (C & 63) * dither20bit;
				C = _data[ofst + 4];
				B = ((C >> 2) & 63) * dither20bit;
				A = ((C & 3) * 85);

				ba[pos++] = (A << 24 | R << 16 | G << 8 | B);
			}

			return MarshalBitmap(ba);
		}

		private Bitmap decode16BitProp()
		{
			throw new NotImplementedException();
		}

		private Bitmap decode8BitProp()
		{
			var pixData = new int[_width * (_height + 1)];
			int n = 0, index = _width;
			for (int y = _height - 1; y >= 0; y--)
			{
				for (int x = _width; x > 0; )
				{
					int cb = _data[n++] & 0xff;

					int mc = cb >> 4;
					int pc = cb & 0xF;
					x -= mc + pc;

					Trace.Assert(x >= 0);
					index += mc;

					while (pc-- > 0)
						if (_data.Length > n)
							pixData[index++] = (int)clutARGB[_data[n++] & 0xff];
				}
			}

			var bitmapBytes = new int[_width * _height];
			int pos = 0;
			for (var y = 44; y < pixData.Length; y++)
				bitmapBytes[pos++] = pixData[y];

			return MarshalBitmap(bitmapBytes);
		}
	}
}

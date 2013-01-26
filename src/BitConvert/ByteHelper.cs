using System;
using System.Globalization;
using System.Text;

namespace BitConvert
{
	public class ByteHelper
	{

		public static string BytesToHexString(byte[] bytes)
		{
			var sBuilder = new StringBuilder();

			if (bytes != null)
			{
				for (int i = 0; i < bytes.Length; i++)
				{
					sBuilder.Append(bytes[i].ToString("X2"));
				}
			}

			return sBuilder.ToString();
		}

		public static byte[] HexStringToBytes(string text)
		{
			var bytes = new byte[4];

			for (int i = 0; i < 4; i++)
			{
				var index = i * 2;
				if (text.Length <= index)
				{
					bytes[i] = 0;
				}
				else
				{
					bytes[i] = byte.Parse(text.Substring(index, 2), NumberStyles.HexNumber);
				}
			}


			return bytes;
		}
	}
}
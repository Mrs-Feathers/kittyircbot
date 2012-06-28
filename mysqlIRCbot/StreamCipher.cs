using System;

namespace kittyIRCbot
{
	public class StreamCipher
	{
		public StreamCipher ()
		{
		}

		public static string encrypt (string key, string message)
		{
			byte[] inputBytes = new byte[message.Length];
			byte[] keyBytes = new byte[message.Length];
			byte[] outputBytes = new byte[message.Length];
			
			inputBytes = StrToByteArray(message);
			keyBytes = StrToByteArray(key);
			keyBytes = PRG(keyBytes,message.Length);
			XOR(inputBytes,keyBytes , ref outputBytes);
			System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
			return ConvertStringToHex(enc.GetString(outputBytes));
		}
		
		public static string decrypt (string key, string message)
		{
			byte[] cipherBytes = new byte[message.Length];
            byte[] keyBytes = new byte[message.Length];
            byte[] outputBytes = new byte[message.Length];
			
			cipherBytes = StrToByteArray(ConvertHexToString(message));
			keyBytes = StrToByteArray(key);
			keyBytes = PRG(keyBytes,(message.Length / 2));
			XOR(cipherBytes,keyBytes , ref outputBytes);
			System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
			return enc.GetString(outputBytes);
		}
		
        private static void XOR(byte[] input,byte[]key ,ref byte[]output)
        {
            for (int i = 0; i < input.Length; i++)
                output[i] = (byte)(key[i] ^ input[i]);
        }
		
		public static byte[] StrToByteArray(string str)
		{
   			System.Text.UTF8Encoding  encoding=new System.Text.UTF8Encoding();
   			return encoding.GetBytes(str);
		}

		public static string ConvertStringToHex(string asciiString)
    	{
    		string hex = "";
    		foreach (char c in asciiString)
    		{			
    			int tmp = c;
    			hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
    		}
    		return hex;
    	}
     
    	public static string ConvertHexToString(string HexValue)
    	{
    		string StrValue = "";
    		while (HexValue.Length > 0)
    		{
    			StrValue += System.Convert.ToChar(System.Convert.ToUInt32(HexValue.Substring(0, 2), 16)).ToString();
    			HexValue = HexValue.Substring(2, HexValue.Length - 2);
    		}
    		return StrValue;
    	}
		
		private static byte[] PRG (byte[] key, int length)
		{	
			System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
			string keytext = enc.GetString(key);
			int number; 
			//if (Parse.Int32(keytext) > 1) number = Parse.Int32(keytext);
			/*else*/ number = Convert.ToInt32(26 * 8576 + 65);
    		while (length >= keytext.Length) {
				int[] scramblers = { 3, 5, 7, 31, 343, 2348, 89897 };
    			keytext += Convert.ToChar((length / 7) + 6);
    			foreach (int scrambler in scramblers) 
    			{
      				keytext += Convert.ToString(((number * scrambler) % length) + ((number * scrambler) / length));
    			}
			}
			return StrToByteArray(keytext);
  		}
		
		private static void outputkthnx (byte[] stuff)
		{
			System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
			string stufflol = enc.GetString(stuff);
			Console.WriteLine(stufflol);
		}
	}
}


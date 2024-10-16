using System.Text;

namespace HttpServer.Frame.Tools
{
    public class Tools
    {
        /// <summary>
        /// String To Unicode
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        public static string StringToUnicode(string inputText)
        {
            string newStr = "";
            for (int i = 0; i < inputText.Count(); ++i)
            {
                if (inputText[i] == '\\') newStr += '\\';
                newStr += inputText[i];
            }
            char[] charBuffer = newStr.ToCharArray();
            byte[] buffer;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < charBuffer.Length; i++)
            {
                if ((int)charBuffer[i] > 127)
                {
                    buffer = System.Text.Encoding.Unicode.GetBytes(charBuffer[i].ToString());
                    stringBuilder.Append(string.Format("\\u{0:X2}{1:X2}", buffer[1], buffer[0]));
                }
                else
                {
                    stringBuilder.Append(charBuffer[i].ToString());
                }
            }
            return stringBuilder.ToString();
        }
    }
}

using System.IO;
using System.Text.RegularExpressions;

namespace Runner
{
    public class ConvertTools
    {
        public const string NtStatus = "#define STATUS_(?<name>(\\w+)).*(?<value>(0x[0-9a-fA-F]+))";

        public static void ConvertCHeaderToEnum(string sourceFile, string outputFile, string pattern = NtStatus)
        {
            var header = File.ReadAllLines(sourceFile);
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            using (var output = new StreamWriter(File.OpenWrite(outputFile)))
            {
                for (var x = 0; x < header.Length; x++)
                {
                    var line = header[x];
                    var m = regex.Match(line);
                    if (!m.Success)
                        continue;
                    output.WriteLine($"{ProcessName(m.Groups["name"].Value)} = {m.Groups["value"].Value},");
                }
            }
        }

        private static string ProcessName(string name)
        {
            var shift = true;
            string result = string.Empty;
            foreach (var c in name)
            {
                // Uppercase
                if (shift)
                {
                    result += char.ToUpper(c);
                    shift = false;
                    continue;
                }

                // Uppercase next, skip
                if (c == '_')
                {
                    shift = true;
                }
                // Lowercase
                else
                {
                    result += char.ToLower(c);
                }
            }

            return result;
        }
    }
}

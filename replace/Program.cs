using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace replace
{
    class Program
    {
        static Encoding utf8 = new UTF8Encoding(true);

        static void Main(string[] args)
        {
            if (args.Length < 4)
                return;

            string baseFolder = args[0];
            string fileFilter = args[1];
            string pattern = args[2];
            string formatter = args[3];

            Regex re = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            Replace(baseFolder, fileFilter, re, formatter);
        }

        static void Replace(string baseFolder, string fileFilter, Regex re, string formatter)
        {
            foreach (string filePath in Directory.GetFiles(baseFolder))
            {
                if (!Regex.IsMatch(filePath, fileFilter))
                    continue;                

                string tmpfilepath = filePath + "_";
                bool isMatch = false;

                using (FileStream sr = new FileStream(filePath, FileMode.Open))
                using (FileStream sw = new FileStream(tmpfilepath, FileMode.OpenOrCreate))
                {
                    byte[] buffer = new byte[sr.Length];
                    sr.Read(buffer, 0, buffer.Length);

                    string data = utf8.GetString(buffer);
                    if (re.IsMatch(data))
                    {
                        byte[] outbuffer = utf8.GetBytes(re.Replace(data, formatter));
                        sw.Write(outbuffer, 0, outbuffer.Length);
                        isMatch = true;
                    }                    
                }

                if (isMatch)
                {
                    Console.WriteLine(filePath);
                    File.Delete(filePath);
                    File.Move(tmpfilepath, filePath);
                }
                else
                {
                    File.Delete(tmpfilepath);
                }
            }

            foreach (string folderPath in Directory.GetDirectories(baseFolder))
            {
                Replace(folderPath, fileFilter, re, formatter);
            }
        }
    }
}

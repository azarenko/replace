using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace find
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
                return;

            string baseFolder = args[0];
            string fileFilter = args[1];
            string pattern = args[2];            

            Regex re = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex fileFilterRe = new Regex(fileFilter, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            Find(baseFolder, fileFilterRe, re);
        }
        static void Find(string baseFolder, Regex fileFilterRe, Regex re)
        {
            foreach (string filePath in Directory.GetFiles(baseFolder))
            {
                if (!fileFilterRe.IsMatch(filePath))
                    continue;

                bool isMatch = false; 

                using (StreamReader sr = new StreamReader(filePath))                
                {
                    int linenumber = 0;
                    while (!sr.EndOfStream)
                    {
                        linenumber++;
                        string line = sr.ReadLine();
                        if (re.IsMatch(line))
                        {
                            isMatch = true;
                            //Console.WriteLine(string.Format("\t{0}: {1}", linenumber, line));
                        }                        
                    }
                }

                if (isMatch)
                {
                    Console.WriteLine(filePath);
                }
            }

            foreach (string folderPath in Directory.GetDirectories(baseFolder))
            {
                Find(folderPath, fileFilterRe, re);
            }
        }
    }
}

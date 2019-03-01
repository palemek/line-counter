using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineCounter
{
    class Program
    {
        static List<List<string>> allFiles = new List<List<string>>();
        static List<List<int>> alllines = new List<List<int>>();
        static List<List<int>> allsizes = new List<List<int>>();
        static List<List<int>> allcomments = new List<List<int>>();
        static List<List<int>> allclearlines = new List<List<int>>();
        static List<List<int>> allsmalllines = new List<List<int>>();

        static List<string> ext = new List<string>();
        static List<string> ignore = new List<string>();
        
        static string empty(int _i)
        {
            string temp = "";
            for (int i = 0; i < _i; i++)
                temp += ' ';
            return temp;
        }

        static void Main(string[] args)
        {
            if(args.Length==0)
            {
                Console.WriteLine("you didnt pass any arguments. Please do");
                Console.Read();
                return;
            }

            if (args[0] == "help")
            {
                Console.WriteLine("first argument is directory for check, all next arguments are extensions of files in which lines will be counted");
                Console.WriteLine("all files including frazes stated like !arg wont be taken into account");
            }

            if (!Directory.Exists(args[0]))
            {
                if (!Directory.Exists(args[0] + " " + args[1]))
                {
                    Console.WriteLine("directory you gave does not exist");
                    Console.Read();
                    return;
                }
                else
                {
                    args[0] += " " + args[1];
                    for (int i = 1; i < args.Count()-1; i++)
                    {
                        args[i] = args[i + 1];
                    }
                }
                    
            }

            int argslen = args.Length;
            string directory = args[0];
            for (int i = 0; i < argslen - 1; i++)
            {
                if (args[i + 1][0] == '!')
                    ignore.Add(args[i + 1]);
                else
                    ext.Add(args[i + 1]);
            }
            //Console.WriteLine("Jakich rozszerzen szukamy(z kropką, po przecinkach, bez spacji): ");
            //ext = new List<string>(Console.ReadLine().Split(','));
            for (int i = 0; i < ext.Count; i++)
            {
                allFiles.Add(new List<string>());
                alllines.Add(new List<int>());
                allsizes.Add(new List<int>());
                allcomments.Add(new List<int>());
                allclearlines.Add(new List<int>());
                allsmalllines.Add(new List<int>());
            }
            //Console.WriteLine("Lokacja z ktorej sprawdzamy:");
            //string directory = Console.ReadLine();
            getThem(directory);

            //podsumowanie:

            List<int> lines = new List<int>();
            List<int> sizes = new List<int>();
            List<int> files = new List<int>();
            List<int> comments = new List<int>();
            List<int> clears = new List<int>();
            List<int> smalllines = new List<int>();

            string printExt = "";
            string printFiles = "";
            string printLines = "";
            string printCodes = "";
            string printSmall = "";
            string printClear = "";
            string printComment = "";
            string printSize = "";

            int length = 10;

            for (int i = 0; i < ext.Count; i++)
            {
                lines.Add(0);
                sizes.Add(0);
                files.Add(0);
                comments.Add(0);
                clears.Add(0);
                smalllines.Add(0);
                for (int j = 0; j < allFiles[i].Count; j++)
                {
                    lines[i] += alllines[i][j];
                    sizes[i] += allsizes[i][j];
                    comments[i] += allcomments[i][j];
                    clears[i] += allclearlines[i][j];
                    smalllines[i] += allsmalllines[i][j];
                    files[i]++;
                }


                printExt        += empty(length - ext[i].Length) + ext[i];
                printFiles      += empty(length - files[i].ToString().Length) + files[i];
                int trueLines = (lines[i] + clears[i] + comments[i]);
                printLines      += empty(length - trueLines.ToString().Length)+ trueLines;
                printCodes      += empty(length - lines[i].ToString().Length) + lines[i];
                printSmall      += empty(length - smalllines[i].ToString().Length)+ smalllines[i];
                printClear      += empty(length - clears[i].ToString().Length) + clears[i];
                printComment    += empty(length - comments[i].ToString().Length) + comments[i];
                int sizeSize = (sizes[i] / 1024);
                printSize       += empty(length - sizeSize.ToString().Length) + sizeSize;
                
            }

            int totalLines = 0;
            int totalSize = 0;
            int totalFiles = 0;
            int totalComments = 0;
            int totalClears = 0;
            int totalSmall = 0;

            for (int i = 0; i < ext.Count; i++)
            {
                totalFiles += files[i];
                totalLines += lines[i];
                totalSize += sizes[i];
                totalClears += clears[i];
                totalComments += comments[i];
                totalSmall += smalllines[i];
            }
            Console.WriteLine();
            Console.WriteLine("Summary:                                  total{0}", printExt);
            Console.WriteLine(">>files with searched extensions:    {0}{1}{2}", empty(length - totalFiles.ToString().Length), totalFiles, printFiles);
            Console.WriteLine(">>lines:                             {0}{1}{2}", empty(length - (totalLines + totalClears + totalComments).ToString().Length), totalLines + totalClears + totalComments, printLines);
            Console.WriteLine(">>   code lines:                     {0}{1}{2}", empty(length - totalLines.ToString().Length), totalLines, printCodes);
            Console.WriteLine(">>       (less then 3 characters):   {0}{1}{2}", empty(length - totalSmall.ToString().Length), totalSmall, printSmall);
            Console.WriteLine(">>   clear lines(spaces or tabs):    {0}{1}{2}", empty(length - totalClears.ToString().Length), totalClears, printClear);
            Console.WriteLine(">>   comment lines                   {0}{1}{2}", empty(length - totalComments.ToString().Length), totalComments, printComment);
            Console.WriteLine(">>size of files(kilobytes):          {0}{1}{2}", empty(length - (totalSize / 1024).ToString().Length), totalSize / 1024, printSize);
        }

        static void getThem(string sdir)
        {
            foreach (string f in Directory.GetFiles(sdir))
            {
                foreach(string ign in ignore)
                    if (f.Contains(ign))
                        continue;
                
                for(int i = 0; i < ext.Count; i++)
                {
                    if (Path.GetExtension(f) == ext[i])
                    {
                        int currlines = 0;
                        int currclearlines = 0;
                        int currcomments = 0;
                        int currsmalls = 0;
                        bool comment = false;
                        foreach(string line in File.ReadLines(f))
                        {
                            string newline = line;
                            char tab = '\u0009';
                            newline = newline.Replace(tab.ToString(), "");
                            newline = newline.Replace(" ", "");
                            if(newline.Length == 0)
                            {
                                currclearlines++;
                                continue;
                            }

                            if(comment)
                            {
                                currcomments++;
                                if (newline.Contains(@"*/"))
                                    comment = false;
                                else
                                    continue;
                            }
                            else
                            {
                                if (newline.Contains(@"/*")&&!newline.Contains(@"*/"))
                                    comment = true;
                            }

                            if (!newline.StartsWith(@"//"))
                            {
                                currlines++;
                                if (newline.Length < 3)
                                    currsmalls++;
                            } 
                            else
                                currcomments++;
                        }
                        alllines[i].Add(currlines);
                        allclearlines[i].Add(currclearlines);
                        allcomments[i].Add(currcomments);
                        allsizes[i].Add((int)new System.IO.FileInfo(f).Length);
                        allsmalllines[i].Add(currsmalls);
                        allFiles[i].Add(f);
                    }
                }
                
            }
            foreach (string f in Directory.GetDirectories(sdir))
            {
                getThem(f);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;

namespace CSV_Name_Extractor
{
    internal class CSVNamex
    {
        static void Main(string[] args)
        {
            if (File.Exists("out.txt"))
            {
                Console.WriteLine("out.txt already exists. Press any key to exit.");
                Console.ReadKey();
                return;
            }

            using (var reader = new StreamReader(args[0]))
            {
                List<string> names = new List<string>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    names.Add(values[0]);
                }

                if (names[0].ToLower() != "member" && names[0].ToLower() != "members" && names[0].ToLower() != "name" && names[0].ToLower() != "names")
                {
                    while (true) {
                        Console.Clear();
                        Console.WriteLine("Uh-oh! YouTube changed the format of the .csv file!\n");
                        Console.WriteLine("The first entry of the list was '" + names[0] + "', expected member, members, name, or names\n");
                        Console.WriteLine("Press 1 to exit, press 2 to continue and keep the first entry, press 3 to continue and remove the first entry.");

                        ConsoleKey keypress = Console.ReadKey().Key;
                        if (keypress == ConsoleKey.D1)
                        {
                            return;
                        }
                        else if (keypress == ConsoleKey.D2)
                        {
                            break;
                        }
                        else if (keypress == ConsoleKey.D3)
                        {
                            names.RemoveAt(0);
                            break;
                        }
                    }
                }
                else
                {
                    names.RemoveAt(0);
                }

                names.Sort();

                File.WriteAllLines("out.txt", names);
            }
            return;
        }
    }
}

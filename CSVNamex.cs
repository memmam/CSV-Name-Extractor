using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSV_Name_Extractor
{
    internal class CSVNamex
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide the path to the .csv file as a command-line argument Press any key to exit.");
                Console.ReadKey();
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("The specified CSV file does not exist. Press any key to exit.");
                Console.ReadKey();
                return;
            }

            string outfile = args.Length > 1 ? args[1] : "out.txt";

            if (!outfile.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                Console.Clear();
                Console.WriteLine(outfile + " is not a valid filename. Please enter a filename that ends in '.txt', and press enter:");
                outfile = Console.ReadLine().Trim();
                while (!outfile.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) || File.Exists(outfile))
                {
                    if (!outfile.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("\nFilename does not end in '.txt'. Please try again.");
                    }
                    else if (File.Exists(outfile))
                    {
                        Console.WriteLine("\nFile already exists. Please try again.");
                    }
                    outfile = Console.ReadLine().Trim();
                }
            }

            if (File.Exists(outfile))
            {
                bool exitFlag = false;
                do
                {
                    Console.Clear();
                    Console.WriteLine(outfile + " already exists.");
                    Console.WriteLine("Press 1 to exit, press 2 to overwrite " + outfile + ", press 3 to specify a new filename.");

                    ConsoleKey keypress = Console.ReadKey().Key;
                    if (keypress == ConsoleKey.D1)
                    {
                        return;
                    }
                    else if (keypress == ConsoleKey.D2)
                    {
                        exitFlag = true;
                    }
                    else if (keypress == ConsoleKey.D3)
                    {
                        Console.WriteLine("\nPlease enter the desired filename, including the '.txt', and press enter:");
                        outfile = Console.ReadLine().Trim();
                        while (!outfile.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) || File.Exists(outfile))
                        {
                            if (!outfile.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("\nFilename does not end in '.txt'. Please try again.");
                            }
                            else if (File.Exists(outfile)) {
                                Console.WriteLine("\nFile already exists. Please try again.");
                            }
                            outfile = Console.ReadLine().Trim();
                        }
                        exitFlag = true;
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid input. Press any key to try again.");
                        Console.ReadKey();
                    }
                } while (!exitFlag);
            }

            try
            {
                using (StreamReader reader = new StreamReader(args[0]))
                {
                    List<string> names = new List<string>();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string name = SplitCsvLine(line);

                        names.Add(name);
                    }

                    if (names.Count == 0)
                    {
                        Console.WriteLine("The provided CSV file is empty. Press any key to exit.");
                        Console.ReadKey();
                        return;
                    }

                    string header = names[0].Trim();
                    if (!header.Equals("member", StringComparison.OrdinalIgnoreCase) &&
                        !header.Equals("members", StringComparison.OrdinalIgnoreCase) &&
                        !header.Equals("name", StringComparison.OrdinalIgnoreCase) &&
                        !header.Equals("names", StringComparison.OrdinalIgnoreCase))
                    {
                        bool exitFlag = false;
                        do
                        {
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
                                exitFlag = true;
                            }
                            else if (keypress == ConsoleKey.D3)
                            {
                                names.RemoveAt(0);
                                exitFlag = true;
                            }
                            else
                            {
                                Console.WriteLine("\nInvalid input. Press any key to try again.");
                                Console.ReadKey();
                            }
                        } while (!exitFlag);
                    }
                    else
                    {
                        names.RemoveAt(0);
                    }

                    names.Sort();

                    File.WriteAllLines(outfile, names);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}. Press any key to exit.");
                Console.ReadKey();
                return;
            }
            return;
        }

        public static string SplitCsvLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return string.Empty;
            }

            bool inQuotes = false;
            StringBuilder current = new StringBuilder();

            foreach (char c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                    if (!inQuotes)
                    {
                        break;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    break;
                }
                else
                {
                    current.Append(c);
                }
            }

            return current.ToString();
        }
    }
}

using System;
using System.Runtime.CompilerServices;
using WorkingFileWatcher;

namespace WorkingFileWatcherConsole
{
    internal class WorkingFileWatcherConsole
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Working File Watcher 1.0");

            IFileWatcher watcher = new FileWatcher(new FileWatcherFileReader(new FileWatcherStringReader()));
            while (true)
            {
                Console.WriteLine(" ");
                Console.WriteLine("Commands:");
                Console.WriteLine("directly - enter file and destination directly to console. ");
                Console.WriteLine("from file - enter a file to read files and directories from. ");
                Console.WriteLine("start - Start the watcher. ");

                string entry = Console.ReadLine();
                entry = entry.Trim().ToLower();
                if(entry == "from file")
                {
                    Console.WriteLine("Enter file to read from: ");
                    entry = Console.ReadLine();
                    entry = entry.Trim();
                    if(string.IsNullOrWhiteSpace(entry))
                    {
                        Console.WriteLine("Filename was empty");
                    }
                    else
                    {
                        bool added = watcher.AddFilesAndDirectoryFromFile(entry);
                        if(added)
                        {
                            Console.WriteLine("Added files. ");
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong. ");
                        }
                    }
                }
                else if (entry == "directly")
                {
                    Console.WriteLine("Enter Filename");
                    string filename = Console.ReadLine();
                    filename = filename.Trim();
                    if (string.IsNullOrWhiteSpace(filename))
                    {
                        Console.WriteLine("Filename was empty");
                    }
                    else
                    {
                        Console.WriteLine("Enter Directory");
                        string dir = Console.ReadLine();
                        dir = dir.Trim();
                        if (string.IsNullOrWhiteSpace(dir))
                        {
                            Console.WriteLine("Directory was empty");
                        }
                        else
                        {
                            bool added = watcher.AddFileToWatch(filename, dir);
                            if (added)
                            {
                                Console.WriteLine("Added watcher. ");
                            }
                            else
                            {
                                Console.WriteLine("Something went wrong. ");
                            }
                        }

                    }
                }
                else if (entry == "start")
                {
                    Console.WriteLine("Starting");
                    break;
                }
                else
                {
                    Console.WriteLine("No idea what you just wrote :(");
                }
            }

            watcher.Start();

            while (true)
            {

            }
        }
    }
}
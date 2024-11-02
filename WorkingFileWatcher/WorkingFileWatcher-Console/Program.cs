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
                string entry = Console.ReadLine();
                entry = entry.Trim().ToLower();
                if(entry == "from file")
                {
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
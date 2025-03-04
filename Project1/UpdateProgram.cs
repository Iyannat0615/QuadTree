using System;
using System.IO;

namespace QuadtreeConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: QuadtreeConsole <command_file>");
                return;
            }

            string commandFile = args[0];

            if (!File.Exists(commandFile))
            {
                Console.WriteLine($"Error: Command file '{commandFile}' not found.");
                return;
            }

            Quadtree quadtree = new Quadtree();

            try
            {
                string[] commands = File.ReadAllLines(commandFile);

                foreach (string command in commands)
                {
                    ProcessCommand(quadtree, command);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static void ProcessCommand(Quadtree quadtree, string command)
        {
            string[] parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return;

            string action = parts[0].ToLower();

            try
            {
                switch (action)
                {
                    case "insert":
                        if (parts.Length != 5)
                        {
                            Console.WriteLine("Invalid insert command format. Use: insert <x> <y> <length> <width>");
                            return;
                        }
                        double x = double.Parse(parts[1]);
                        double y = double.Parse(parts[2]);
                        double length = double.Parse(parts[3]);
                        double width = double.Parse(parts[4]);

                        Rectangle existingRect = quadtree.Find(x, y);
                        if (existingRect != null)
                        {
                            Console.WriteLine("Warning: A rectangle already exists at this position. Overlapping is allowed.");
                        }

                        quadtree.Insert(new Rectangle(x, y, length, width));
                        break;

                    case "find":
                        if (parts.Length != 3)
                        {
                            Console.WriteLine("Invalid find command format. Use: find <x> <y>");
                            return;
                        }
                        double findX = double.Parse(parts[1]);
                        double findY = double.Parse(parts[2]);

                        Rectangle foundRect = quadtree.Find(findX, findY);
                        if (foundRect != null)
                        {
                            Console.WriteLine($"Rectangle at {findX}, {findY}: {foundRect.Length}x{foundRect.Width}");
                        }
                        else
                        {
                            Console.WriteLine($"Nothing is at {findX}, {findY}.");
                        }
                        break;

                    case "delete":
                        if (parts.Length != 3)
                        {
                            Console.WriteLine("Invalid delete command format. Use: delete <x> <y>");
                            return;
                        }
                        double deleteX = double.Parse(parts[1]);
                        double deleteY = double.Parse(parts[2]);

                        if (quadtree.Delete(deleteX, deleteY))
                        {
                            Console.WriteLine($"Deleted rectangle at {deleteX}, {deleteY}.");
                        }
                        else
                        {
                            Console.WriteLine($"Nothing to delete at {deleteX}, {deleteY}.");
                        }
                        break;

                    case "update":
                        if (parts.Length != 5)
                        {
                            Console.WriteLine("Invalid update command format. Use: update <x> <y> <new_length> <new_width>");
                            return;
                        }
                        double updateX = double.Parse(parts[1]);
                        double updateY = double.Parse(parts[2]);
                        double newLength = double.Parse(parts[3]);
                        double newWidth = double.Parse(parts[4]);

                        if (quadtree.Update(updateX, updateY, newLength, newWidth))
                        {
                            Console.WriteLine($"Updated rectangle at {updateX}, {updateY} to {newLength}x{newWidth}.");
                        }
                        else
                        {
                            Console.WriteLine($"Nothing to update at {updateX}, {updateY}.");
                        }
                        break;

                    case "dump":
                        Console.WriteLine("Quadtree Structure:");
                        quadtree.Dump();
                        break;

                    case "help":
                        Console.WriteLine("Available commands:");
                        Console.WriteLine("  insert <x> <y> <length> <width>");
                        Console.WriteLine("  find <x> <y>");
                        Console.WriteLine("  delete <x> <y>");
                        Console.WriteLine("  update <x> <y> <new_length> <new_width>");
                        Console.WriteLine("  dump");
                        break;

                    default:
                        Console.WriteLine($"Unknown command: {command}. Type 'help' for a list of commands.");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine($"Error: Invalid numeric format in command: {command}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing command: {command} - {ex.Message}");
            }
        }
    }
}
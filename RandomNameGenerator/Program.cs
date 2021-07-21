using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RandomNameGenerator
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //ListPrinter print;
            //print = new ListPrinter();
            //await print.Main();

            JpnNameGenerator nameGen;
            nameGen = new JpnNameGenerator();
            string newPName, newFName;
            string[] formattedPName, formattedFName;

            while (true)
            {
                Console.WriteLine("Pick a gender option");
                Console.WriteLine("0 - Any random / no preference");
                Console.WriteLine("1 - Male");
                Console.WriteLine("2 - Female");
                Console.WriteLine("3 - Neutral");
                Console.WriteLine("4 - Cancel / Close");

                string choice = Console.ReadLine();
                int gender, number;
                bool check = int.TryParse(choice, out gender);

                if (check && gender >= 0 && gender <= 4)
                {
                    if (gender == 4)
                    {
                        break;
                    }

                    while (true)
                    {
                        Console.WriteLine("Enter number of names to print.");
                        string string_nOfNames = Console.ReadLine();

                        check = int.TryParse(string_nOfNames, out number);

                        if (check)
                        {
                            Stopwatch sw1 = new Stopwatch();
                            sw1.Restart();

                            int i = 0;
                            string[] namesList = new string[number];

                            while (i < number)
                            {
                                newPName = nameGen.getRandomPersonalName(gender);
                                formattedPName = nameGen.getFormattedName(newPName);

                                newFName = nameGen.getRandomFamilyName();
                                formattedFName = nameGen.getFormattedName(newFName);

                                JpnName name = new JpnName(formattedPName[0], formattedPName[1], formattedPName[2], formattedFName[0], formattedFName[1]);
                                namesList[i] = (i + 1) + ". " + name.ToString();

                                Console.WriteLine((i + 1) + ". " + name);
                                i++;
                            }

                            sw1.Stop();

                            Console.WriteLine("Elapsed time to generate names: " + sw1.Elapsed);

                            Stopwatch sw2 = new Stopwatch();

                            Console.WriteLine("Insert a filename to store the names: (Don't type .txt)");
                            string filename = Console.ReadLine() + ".txt";

                            sw2.Restart();

                            await File.WriteAllLinesAsync(filename, namesList);
                            sw2.Stop();

                            Console.WriteLine("Elapsed time to write names to file: " + sw2.Elapsed);

                            Console.WriteLine("Please check " + filename + " for the names.");
                            Console.WriteLine("Press ENTER to continue.");
                            Console.ReadLine();

                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid entry. Must be a whole number.");
                        }
                    }

                    break;
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                }
            }
        }
    }
}
using AngleSharp;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RandomNameGenerator
{
    internal class ListPrinter

    //This class is unused by the name generator and is only useful for making the name lists
    //found in this directory. The commented code to use it is found at the top of Program.cs in Main.
    //If you want to use this, for whatever reason, replace REPLACEME on line 46 with:
    //
    //masc - male names
    //fem - female names
    //given - neutral names
    //surname - family names
    //
    //Next, change N_OF_PAGES (line 35) to however many pages of Jisho you want it to read.
    //Make sure that many pages actually exist, lol. I think the max is 2000 but male names only reach 987.
    //
    //After that, change REPLACEME on line 218 with whatever you want the output file to be named.
    //Keep '.txt', unless you want to change the kind of file it creates.
    //The default names for the files are MaleNames.txt, FemaleNames.txt, NeutralNames.txt, and FamilyNames.txt.

    {
        public async Task Main()
        {
            string[] fullNames, kanjiNames, romajiNames;

            var config = Configuration.Default.WithDefaultLoader();
            int i = 1;
            int skippedIndexes = 0;
            const int N_OF_PAGES = 5;
            int index = 0;
            int lastValue = 0;
            kanjiNames = new string[N_OF_PAGES * 20];
            romajiNames = new string[N_OF_PAGES * 20];
            char[] trim = { '(', '1', ')', ' ' };

            while (i <= N_OF_PAGES)
            {
                Console.WriteLine("Processing page " + i);
                string[] processingRomaji, processingKanji;
                var URL = "https://jisho.org/search/%23names%23REPLACEME?page=" + i.ToString();
                var document = await BrowsingContext.New(config).OpenAsync(URL);

                var kanjiDoc = document.All.Where(m =>
                m.HasAttribute("class") &&
                m.GetAttribute("class").Contains("concept_light-readings")
             );

                var romajiDoc = document.All.Where(m =>
                    m.HasAttribute("class") &&
                    m.GetAttribute("class").Contains("meaning-meaning")
                 );

                var defDoc = document.All.Where(m =>
                    m.HasAttribute("class") &&
                    m.GetAttribute("class").Contains("meaning-tags")
                 );

                var compareDoc = document.All.Where(m =>
                    m.HasAttribute("class") &&
                    m.GetAttribute("class").Contains("break-unit")
                    );

                index = 0;
                skippedIndexes = 0;

                processingKanji = new string[kanjiDoc.Count()];

                while (index < kanjiDoc.Count())
                {
                    processingKanji[index] = kanjiDoc.ElementAt(index).TextContent.ToString();
                    //Skip names that suck
                    bool skip = false;

                    if (romajiDoc.ElementAt(index).TextContent.ToString().Contains("stage name") || defDoc.ElementAt(index).TextContent.ToString().Contains("Full name") || skip)
                    {
                        skippedIndexes++;
                        index++;
                        continue;
                    }
                    //Find the name
                    bool found = false;
                    if (processingKanji[index].Contains("【"))
                    {
                        foreach (char letter in processingKanji[index])
                        {
                            if (letter.Equals('【') && found == false)
                            {
                                found = true;
                                continue;
                            }

                            if (found)
                            {
                                if (letter.Equals('】'))
                                {
                                    break;
                                }
                                else
                                {
                                    kanjiNames[lastValue + index - skippedIndexes] += letter;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (char letter in processingKanji[index])
                        {
                            if (!letter.Equals('\n') && !letter.Equals(' ') && found == false)
                            {
                                found = true;
                            }

                            if (found)
                            {
                                if (letter.Equals(' ') || letter.Equals('\n'))
                                {
                                    break;
                                }
                                else
                                {
                                    kanjiNames[lastValue + index - skippedIndexes] += letter;
                                }
                            }
                        }
                    }

                    index++;
                }

                index = 0;
                processingRomaji = new string[romajiDoc.Count()];
                Console.WriteLine("Processing Romaji");
                //Processing Romaji
                skippedIndexes = 0;
                while (index < romajiDoc.Count())
                {
                    Console.WriteLine("Index: " + index + "/" + romajiDoc.Count() + " at array space " + (lastValue + index - skippedIndexes) + " = " + romajiDoc.ElementAt(index).TextContent.ToString());
                    Console.WriteLine("Array calculations: " + lastValue + " + " + index + " - " + skippedIndexes);
                    processingRomaji[index] = romajiDoc.ElementAt(index).TextContent.ToString();
                    bool skip = false;

                    foreach (var item in compareDoc)
                    {
                        Console.WriteLine("Comparing " + item.TextContent.ToString() + " and " + processingRomaji[index]);
                        if (item.TextContent.ToString() == processingRomaji[index])
                        {
                            skip = true;
                            Console.WriteLine("Found match!");
                            Console.WriteLine("Overwriting " + kanjiNames[lastValue + index - skippedIndexes - 1]);
                            kanjiNames[lastValue + index - skippedIndexes - 1] = processingRomaji[index];
                        }
                    }

                    if (romajiDoc.ElementAt(index).TextContent.ToString().Contains("stage name") || defDoc.ElementAt(index).TextContent.ToString().Contains("Full name") || skip)
                    {
                        Console.WriteLine("Skipping this entry.");
                        skippedIndexes++;
                        index++;
                        skip = false;
                        continue;
                    }

                    if (processingRomaji[index].Contains("("))
                    {
                        Console.WriteLine("String has (. Checking each part individually.");
                        int tempIndex = 0;
                        foreach (char letter in processingRomaji[index])
                        {
                            if (letter == '(' && tempIndex != 0)
                            {
                                break;
                            }
                            else
                            {
                                romajiNames[lastValue + index - skippedIndexes] += letter;
                                tempIndex++;
                            }
                        }
                    }
                    else
                    {
                        romajiNames[lastValue + index - skippedIndexes] = processingRomaji[index];
                    }
                    romajiNames[lastValue + index - skippedIndexes] = romajiNames[lastValue + index - skippedIndexes].Trim();
                    romajiNames[lastValue + index - skippedIndexes] = romajiNames[lastValue + index - skippedIndexes].Trim(trim);
                    romajiNames[lastValue + index - skippedIndexes] = romajiNames[lastValue + index - skippedIndexes].Replace("; ", ";");
                    index++;
                }

                Console.WriteLine(i + " processed!");
                //Concatenating Names
                Console.WriteLine("Updating lastValue to " + (lastValue + index - skippedIndexes) + " = " + lastValue + " (lastValue), " + index + " (index), " + skippedIndexes + " (skippedIndexes)");
                lastValue = lastValue + index - skippedIndexes;
                i++;
                int items = 0;

                fullNames = new string[lastValue];

                while (items < lastValue)
                {
                    fullNames[items] = romajiNames[items] + "/" + kanjiNames[items];
                    items++;
                }

                await File.WriteAllLinesAsync("REPLACEME.txt", fullNames);
            }
        }
    }
}
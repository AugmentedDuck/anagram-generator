using System.Diagnostics;
using System.Text.RegularExpressions;
using AnagramGenerator.Core;

namespace AnagramGenerator
{
    class Program
    {
        static readonly Core.Core core = new();

        static void Main()
        {
            bool includeEnglish = false;
            bool includeDanish = false;
            bool includeUserWords = false;
            bool includeSingles = false;
            int maxRecursion;

            Console.WriteLine("Welcome to the anagram generator");

            while (!(includeDanish || includeUserWords || includeEnglish))
            {
                Console.WriteLine("Do you want to include the English dictionary? (Y/N)");
                string englishConfermation = Console.ReadLine();

                if (englishConfermation == "yes" || englishConfermation == "Y" || englishConfermation == "Yes" || englishConfermation == "y")
                {
                    includeEnglish = true;
                }

                Console.WriteLine("Do you want to include the Danish dictionary? (Y/N)");
                string danishConfermation = Console.ReadLine();

                if (danishConfermation == "yes" || danishConfermation == "Y" || danishConfermation == "Yes" || danishConfermation == "y")
                {
                    includeDanish = true;
                }

                Console.WriteLine("Do you want to include the User dictionary? (Y/N)");
                string userConfermation = Console.ReadLine();

                if (userConfermation == "yes" || userConfermation == "Y" || userConfermation == "Yes" || userConfermation == "y")
                {
                    includeUserWords = true;
                }

                if (!(includeDanish || includeUserWords || includeEnglish))
                {
                    Console.WriteLine("You need at least one dictionary to continue!");
                }
            }

            Console.WriteLine("Are non-vowel single letter words considered words? (Y/N)");
            string singleLetterWords = Console.ReadLine();
            if (singleLetterWords == "yes" || singleLetterWords == "Y" || singleLetterWords == "Yes" || singleLetterWords == "y")
            {
                includeSingles = true;
            }

            Console.WriteLine("Maximum amount of words per solution, 0 to disable (3):");
            string intResponse = Console.ReadLine();

            Regex numbersOnly = new("^[0-9]+$");

            if (!(numbersOnly.IsMatch(intResponse) && intResponse != null))
            {
                Console.WriteLine("Not a valid choice, remaining as default");
                intResponse = "3";
            }

            maxRecursion = Convert.ToInt32(intResponse);

            Console.WriteLine("Please write the input word(s):");

            string input = Console.ReadLine();
            string fixedInput = core.NormalizeString(input);


            string[] words = core.ImportWords(includeEnglish, includeDanish, includeUserWords);

            Stopwatch stopwatch = Stopwatch.StartNew();
            string[] anagrams = core.AnagramFinder(fixedInput, words, includeSingles, maxRecursion);
            stopwatch.Stop();

            foreach (string word in anagrams)
            {
                Console.WriteLine(word);
            }

            Console.WriteLine("This took: " + stopwatch.ElapsedMilliseconds + "ms");
        }
    }
}
using AnagramGenerator.Core;
using System.Diagnostics;

namespace PerformanceTester
{
    internal class Program
    {

        static void Main(string[] args)
        {

            Core core = new Core();
            string[] testWords = { "text", "full", "pour", "idea", "pain", "palm", "tire", "flow", "walk", "site",
                                    "shave", "jewel", "shout", "sugar", "march", "knock", "print", "model", "chaos", "lease",
                                    "garage", "cheese", "murder", "agenda", "method", "sodium", "agency", "desert", "pardon", "affair",
                                    "dismiss", "glasses", "protest", "divorce", "storage", "profile", "liberty", "pioneer", "welfare", "convict",
                                    "offender", "dressing", "headline", "forecast", "disclose", "activate", "possible", "argument", "password", "illusion",
                                    "construct", "explosion", "hostility", "policeman", "housewife", "favorable", "chemistry", "recording", "entertain", "assertive",
                                    "decorative", "tournament", "confidence", "obligation", "repetition", "chimpanzee", "democratic", "federation", "preference", "artificial"};

            string[] firstRun = { "first", "run" };

            string first = Tester(firstRun, 1);

            string testResults = "";
            
            testResults += Tester(testWords, 1);
            testResults += Tester(testWords, 2);
            testResults += Tester(testWords, 3);

            Directory.CreateDirectory("Results");
            File.WriteAllText($"Results/{DateTime.Now}-Results.txt", testResults);
        }

        static string Tester(string[] testWords, int recursionLevel)
        {
            Core core = new Core();
            string[] words = core.ImportWords(true, true, true);

            string tempTestResults = "";
            foreach (string word in testWords)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                string[] anagrams = core.AnagramFinder(word, words, false, recursionLevel);
                stopwatch.Stop();

                tempTestResults += $"{word.Length}:{recursionLevel} - {stopwatch.ElapsedMilliseconds} ms\n";
                Console.WriteLine(tempTestResults);
            }

            return tempTestResults;
        }
    }
}
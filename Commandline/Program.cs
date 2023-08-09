using System.Text.Json.Serialization;

namespace AnagramGenerator
{
    class Program
    {
        static void Main()
        {
            bool includeEnglish = false;
            bool includeDanish = false;
            bool includeUserWords = false;
            bool includeSingles = false;

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

            Console.WriteLine("Please write the input word(s):");

            string input = Console.ReadLine();
            string fixedInput = NormalizeString(input);

            string[] words = ImportWords(includeEnglish, includeDanish, includeUserWords);
            string[] anagrams = AnagramFinder(fixedInput, words, includeSingles);

            foreach (string word in anagrams)
            {
                Console.WriteLine(word);
            }
        }

        static string[] ImportWords(bool inEnglish, bool inDanish, bool includeUserWords)
        {
            string allWordsInString = "";
            string directory = "dictionaries";

            if (inEnglish)
            {
                Console.WriteLine("Importing words from the English dictionary");
                allWordsInString += File.ReadAllText(directory + "/english.txt") + "\n";
            }

            if (inDanish)
            {
                Console.WriteLine("Importing words from the Danish dictionary");
                allWordsInString += File.ReadAllText(directory + "/dansk.txt") + "\n";
            }

            if (includeUserWords)
            {
                Console.WriteLine("Importing words from the User dictionary");
                allWordsInString += File.ReadAllText(directory + "/user.txt") + "\n";
            }

            string[] words = allWordsInString.Split("\n");

            string[] output = words.Distinct().ToArray();

            return output;
        }

        static string NormalizeString(string inputString)
        {
            return String.Concat(inputString.Where(c => !Char.IsWhiteSpace(c))).ToLower();
        }

        static string[] AnagramFinder(string inputString, string[] words, bool includeSingleLetters)
        {
            string[] outputWords = Array.Empty<string>();
            int indexOfOutput = 0;

            foreach (string word in words)
            {
                if (word.Length < 2 && (!(word == "a" && word == "e" && word == "i" && word == "o" && word == "u" && word == "y" && word == "æ" && word == "ø" && word == "å") || includeSingleLetters))
                {
                    continue;
                }

                char[] tempInput = new char[inputString.Length];

                for (int i = 0; i < tempInput.Length; i++)
                {
                    tempInput[i] = inputString[i];
                }

                if (word.Length > tempInput.Length)
                {
                    continue;
                }

                int nonMatchingLetters = 0;

                foreach (char letter in word)
                {
                    int indexInWord = 0;
                    foreach (char checkLetter in tempInput)
                    {
                        if (letter == checkLetter)
                        {
                            tempInput[indexInWord] = ' ';
                            break;
                        }

                        indexInWord++;

                        if (indexInWord == tempInput.Length)
                        {
                            nonMatchingLetters++;
                        }
                    }
                }

                if (nonMatchingLetters != 0)
                {
                    continue;
                }

                string remainingLetters = "";

                foreach (char letter in tempInput)
                {
                    remainingLetters += letter;
                }

                remainingLetters = NormalizeString(remainingLetters);

                if (remainingLetters.Length > 0)
                {
                    string[] remainingAnagrams = AnagramFinder(remainingLetters, words, includeSingleLetters);

                    foreach (string anagram in remainingAnagrams)
                    {
                        Array.Resize(ref outputWords, outputWords.Length + 1);
                        outputWords[indexOfOutput] = word + " " + anagram;
                        indexOfOutput++;
                    }
                }

                Array.Resize(ref outputWords, outputWords.Length + 1);
                outputWords[indexOfOutput] = word;
                indexOfOutput++;
            }

            return outputWords;
        }
    }
}
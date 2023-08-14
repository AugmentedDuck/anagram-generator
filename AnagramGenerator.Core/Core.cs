using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AnagramGenerator.Core
{
    public class Core
    {
        public string[] ImportWords(bool inEnglish, bool inDanish, bool includeUserWords)
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

            string[] words = allWordsInString.Split('\n');

            string[] output = words.Distinct().ToArray();

            return output;
        }

        public string NormalizeString(string inputString)
        {
            return String.Concat(inputString.Where(c => !Char.IsWhiteSpace(c))).ToLower();
        }

        public string[] AnagramFinder(string inputString, string[] words, bool includeSingleLetters, int maxLevel)
        {
            List<string> outputWords = new List<string>();

            foreach (string word in words)
            {
                if (word.Length < 2 && (!(word == "a" && word == "e" && word == "i" && word == "o" && word == "u" && word == "y" && word == "æ" && word == "ø" && word == "å") || includeSingleLetters))
                {
                    continue;
                }

                if (word.Length > inputString.Length)
                {
                    continue;
                }

                char[] tempInput = inputString.ToCharArray();

                int nonMatchingLetters = 0;
                
                foreach (char letter in word)
                {
                    int indexInWord = Array.IndexOf(tempInput, letter);
                    if (indexInWord != -1)
                    {
                        tempInput[indexInWord] = ' ';
                    }
                    else
                    {
                        nonMatchingLetters++;
                    }
                }

                if (nonMatchingLetters != 0)
                {
                    continue;
                }

                string remainingLetters = new string(tempInput).Replace(" ", "");
                remainingLetters = NormalizeString(remainingLetters);

                if (remainingLetters.Length > 0)
                {
                    string[] remainingAnagrams = { };

                    if (maxLevel == 0)
                    {
                        remainingAnagrams = AnagramFinder(remainingLetters, words, includeSingleLetters, maxLevel);
                    }
                    else
                    {
                        remainingAnagrams = AnagramFinder(remainingLetters, words, includeSingleLetters, maxLevel, 1);
                    }

                    foreach (string anagram in remainingAnagrams)
                    {
                        outputWords.Add(word + " " + anagram);
                    }
                }

                outputWords.Add(word);
            }

            return outputWords.ToArray();
        }

        string[] AnagramFinder(string inputString, string[] words, bool includeSingleLetters, int maxLevel, int level)
        {
            if (maxLevel == level)
            {
                return new string[] { "" };
            }

            List<string> outputWords = new List<string>();

            foreach (string word in words)
            {
                if (word.Length < 2 && (!(word == "a" && word == "e" && word == "i" && word == "o" && word == "u" && word == "y" && word == "æ" && word == "ø" && word == "å") || includeSingleLetters))
                {
                    continue;
                }

                if (word.Length > inputString.Length)
                {
                    continue;
                }

                char[] tempInput = inputString.ToCharArray();

                int nonMatchingLetters = 0;

                foreach (char letter in word)
                {
                    int indexInWord = Array.IndexOf(tempInput, letter);
                    if (indexInWord != -1)
                    {
                        tempInput[indexInWord] = ' ';
                    }
                    else
                    {
                        nonMatchingLetters++;
                    }
                }

                if (nonMatchingLetters != 0)
                {
                    continue;
                }

                string remainingLetters = new string(tempInput).Replace(" ", "");
                remainingLetters = NormalizeString(remainingLetters);

                if (remainingLetters.Length > 0)
                {
                    string[] remainingAnagrams = AnagramFinder(remainingLetters, words, includeSingleLetters, maxLevel, level++);

                    foreach (string anagram in remainingAnagrams)
                    {
                        outputWords.Add(word + " " + anagram);
                    }
                }

                outputWords.Add(word);
            }

            return outputWords.ToArray();
        }
    }
}
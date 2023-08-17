using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

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
            Dictionary<char, int> inputCounts = CountCharacters(NormalizeString(inputString));

            foreach (string word in words)
            {
                if (word.Length < 2 && !includeSingleLetters)
                {
                    continue;
                }

                if (word.Length > inputString.Length)
                {
                    continue;
                }

                Dictionary<char, int> wordCounts = CountCharacters(word);

                if (!IsSubset(inputCounts, wordCounts))
                {
                    continue;
                }

                Dictionary<char, int> remainingCounts = SubstractCharacterCounts(inputCounts, wordCounts);
                string remainingLetters = GetStringFromCharacterCounts(remainingCounts);

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


        private Dictionary<string, string[]> memo = new Dictionary<string, string[]>();

        string[] AnagramFinder(string inputString, string[] words, bool includeSingleLetters, int maxLevel, int level)
        {
            if (maxLevel == level)
            {
                return new string[] { "" };
            }

            string memoKey = $"{inputString}_{level}";
            if (memo.ContainsKey(memoKey))
            {
                return memo[memoKey];
            }
            
            List<string> outputWords = new List<string>();
            Dictionary<char, int> inputCounts = CountCharacters(NormalizeString(inputString));


            foreach (string word in words)
            {
                if (word.Length < 2 && !includeSingleLetters)
                {
                    continue;
                }

                if (word.Length > inputString.Length)
                {
                    continue;
                }

                Dictionary<char, int> wordCounts = CountCharacters(word);

                if (!IsSubset(inputCounts, wordCounts))
                {
                    continue;
                }

                Dictionary<char, int> remainingCounts = SubstractCharacterCounts(inputCounts, wordCounts);
                string remainingLetters = GetStringFromCharacterCounts(remainingCounts);

                string[] remainingAnagrams = AnagramFinder(remainingLetters, words, includeSingleLetters, maxLevel, level++);

                foreach (string anagram in remainingAnagrams)
                {
                    outputWords.Add(word + " " + anagram);
                }
                

                outputWords.Add(word);
            }

            string[] result = outputWords.ToArray();
            memo[memoKey] = result;
            return outputWords.ToArray();
        }

        private Dictionary<char, int> SubstractCharacterCounts(Dictionary<char, int> setCounts, Dictionary<char, int> wordCounts)
        {
            Dictionary<char, int> remainingCounts = new Dictionary<char, int>(setCounts);
            foreach (var kvp in wordCounts)
            {
                if (remainingCounts.ContainsKey(kvp.Key))
                {
                    remainingCounts[kvp.Key] -= kvp.Value;
                    if (remainingCounts[kvp.Key] == 0)
                    {
                        remainingCounts.Remove(kvp.Key);
                    }
                }
            }
            return remainingCounts;
        }

        private Dictionary<char, int> CountCharacters(string word)
        {
            Dictionary<char, int> counts = new Dictionary<char, int>();
            foreach (char letter in word) 
            {
                if (counts.ContainsKey(letter))
                {
                    counts[letter]++;
                }
                else 
                { 
                    counts[letter] = 1; 
                }
            }
            return counts;
        }

        private string GetStringFromCharacterCounts(Dictionary<char, int> counts)
        {
            StringBuilder sb = new StringBuilder();
            
            foreach (var kvp in counts)
            {
                for (int i = 0; i < kvp.Value; i++)
                {
                    sb.Append(kvp.Key);
                }
            }

            return sb.ToString();
        }

        private bool IsSubset(Dictionary<char, int> setCounts, Dictionary<char, int> wordCounts)
        {
            foreach (var kvp in wordCounts)
            {
                if (!setCounts.ContainsKey(kvp.Key) || setCounts[kvp.Key] < kvp.Value)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
namespace AnagramGenerator
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

            string[] words = allWordsInString.Split("\n");

            string[] output = words.Distinct().ToArray();

            return output;
        }

        public string NormalizeString(string inputString)
        {
            return String.Concat(inputString.Where(c => !Char.IsWhiteSpace(c))).ToLower();
        }

        public string[] AnagramFinder(string inputString, string[] words, bool includeSingleLetters)
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
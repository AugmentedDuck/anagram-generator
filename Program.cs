namespace AnagramGenerator
{
    class Program
    {
        static void Main()
        {
            bool includeEnglish = false;
            bool includeDanish = false;
            bool includeUserWords = false;
            
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

                if ( !(includeDanish || includeUserWords || includeEnglish) )
                {
                    Console.WriteLine("You need at least one dictionary to continue!");
                }
            }

            Console.WriteLine("Please write the input word(s):");

            string input = Console.ReadLine();
            string fixedInput = NormalizeString(input);

            string[] words = ImportWords(includeEnglish, includeDanish, includeUserWords);
            string[] anagrams = AnagramFinder(fixedInput, words);
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

        static string[] AnagramFinder(string inputString, string[] words)
        {
            string[] outputWords = Array.Empty<string>();
            int indexOfOutput = 0;

            foreach (string word in words)
            {
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

                Array.Resize(ref outputWords, indexOfOutput + 1);

                string remainingLetters = "";

                foreach (char letter in tempInput)
                {
                    remainingLetters += letter;
                }

                outputWords[indexOfOutput] = word + " - " + NormalizeString(remainingLetters);
                indexOfOutput++;
            }

            Console.WriteLine("Number of anagrams generated: " + outputWords.Length);

            return outputWords;
        }
    }
}
namespace AnagramGenerator
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Welcome to the anagram generator");
            Console.WriteLine("Please write the input word(s):");

            string input = Console.ReadLine();
            string fixedInput = NormalizeString(input);

            string[] words = ImportWords();
            
            string[] outputWords = Array.Empty<string>();
            int indexOfOutput = 0;

            foreach (string word in words)
            {
                char[] tempInput = new char[fixedInput.Length];

                for (int i = 0; i < tempInput.Length; i++)
                {
                    tempInput[i] = fixedInput[i];
                }

                if (word.Length > tempInput.Length)
                {
                    continue;
                }

                int nonMatchingLetters = 0;

                foreach(char letter in word)
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

            foreach (string word in outputWords)
            {
                Console.WriteLine(word);
            }

            Console.WriteLine(outputWords.Length);
        }

        static string[] ImportWords()
        {
            string allWordsInString = "";
            string directory = "dictionaries";

            string[] fileEntries = Directory.GetFiles(directory);
            foreach (string fileName in fileEntries)
            {
                if (fileName.ToLower().EndsWith(".txt"))
                {
                    Console.WriteLine("Importing words from the " + fileName.Remove(0, directory.Length + 1) + " dictionary");
                    allWordsInString += File.ReadAllText(fileName);
                }
            }

            string[] words = allWordsInString.Split("\n");

            return words;
        }

        static string NormalizeString(string inputString)
        {
            return String.Concat(inputString.Where(c => !Char.IsWhiteSpace(c))).ToLower();
        }
    }
}
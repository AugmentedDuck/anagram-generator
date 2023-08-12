namespace AnagramGenerator
{
    class Program
    {
        static readonly Core core = new();

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
            string fixedInput = core.NormalizeString(input);

            string[] words = core.ImportWords(includeEnglish, includeDanish, includeUserWords);
            string[] anagrams = core.AnagramFinder(fixedInput, words, includeSingles);

            foreach (string word in anagrams)
            {
                Console.WriteLine(word);
            }
        }
    }
}
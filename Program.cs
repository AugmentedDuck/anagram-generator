using System.IO;
using System.Linq;

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
      Console.WriteLine(fixedInput);

      string stringOfWords = ImportWords();
      string[] words = stringOfWords.Split("\n");
    }

    static string ImportWords()
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

      return allWordsInString;
    }

    static string NormalizeString(string inputString)
    {
      return String.Concat(inputString.Where(c => !Char.IsWhiteSpace(c))).ToLower();
    }
  }
}
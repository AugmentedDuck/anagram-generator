using AnagramGenerator.Core;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;


namespace GUI
{
    public partial class Form1 : Form
    {
        readonly Core core = new Core();

        bool isEnglish = false;
        bool isDanish = false;
        bool useUserWords = false;
        bool useSingleWords = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void InEnglish(object sender, EventArgs e)
        {
            isEnglish = !isEnglish;
        }

        private void UseUserWords(object sender, EventArgs e)
        {
            useUserWords = !useUserWords;
        }

        private void IsDanish(object sender, EventArgs e)
        {
            isDanish = !isDanish;
        }

        private void IsSingleWord(object sender, EventArgs e)
        {
            useSingleWords = !useSingleWords;
        }

        private void GenerateAnagrams(object sender, EventArgs e)
        {
            string inputString = textBox1.Text;
            int levels = Convert.ToInt16(numericUpDown2.Value);

            string[] words = core.ImportWords(isEnglish, isDanish, useUserWords);
            string[] anagrams = core.AnagramFinder(core.NormalizeString(inputString), words, useSingleWords, levels);

            string anagramString = "";

            foreach (string word in anagrams)
            {
                anagramString += word + "\n";
            }

            string outputFile = $"Results/'{inputString}'-anagrams.txt";

            Directory.CreateDirectory("Results");
            File.WriteAllText(outputFile, anagramString);
        }
    }
}

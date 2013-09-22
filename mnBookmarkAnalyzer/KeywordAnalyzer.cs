using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace mnBookmarkAnalyzer
{
    /// <summary>
    /// Class to analyze the keywords on a link
    /// </summary>
    public class KeywordAnalyzer
    {
        #region Members
        /// <summary>
        /// Collection of valid text 
        /// </summary>
        List<string> listValidText =null;
        /// <summary>
        /// Collection of keywords
        /// </summary>
        List<string> listKeywords = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor of the keyword analyzer
        /// </summary>
        public KeywordAnalyzer()
        {
            listValidText = new List<string>();
            listKeywords = new List<string>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the keywords from a specific url
        /// </summary>
        /// <param name="urlAnalyze">url address to analyze</param>
        /// <returns>Collection of keywords</returns>
        public Dictionary<string, int> GetKeywordsFromUrl(string urlAnalyze)
        {
            listKeywords = new List<string>();
            HtmlAgilityPack.HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = null;
            try
            {
                doc = web.Load(urlAnalyze);
            }
            catch (Exception exception)
            {
                //if the url cannot be loaded, it's skipped by returning null
                return null;
            }

            //Recursive function to get teh keywords in an html page
            foreach (HtmlNode node in doc.DocumentNode.ChildNodes)
            {
                IterateChildNodes(node);
            }
            return WeightWords();
        }

        /// <summary>
        /// Recursive function to read the inner text of the nodes and get the keywords.
        /// </summary>
        /// <param name="mainNode">Main node to iterate</param>
        private void IterateChildNodes(HtmlNode mainNode)
        {
            foreach (HtmlNode childNode in mainNode.ChildNodes)
            {
                if (childNode.InnerText != null)
                {
                    ValidText(childNode.InnerText);
                }
                foreach (HtmlNode main8 in childNode.ChildNodes)
                {
                    if (main8.InnerText != null)
                    {
                        ValidText(main8.InnerText);
                    }
                }
                IterateChildNodes(childNode);
            }
        }

        /// <summary>
        /// Validates that the text doesn't contain special characters
        /// </summary>
        /// <param name="innerText">Text to be analyzed</param>
        /// <returns>True if it's a valid text or false if it isn't</returns>
        /// <remarks>Comments, skip lines and & are invalid charcaters</remarks>
        private bool ValidText(string innerText)
        {
            string test0 = innerText;
            string test1 = test0.Replace('\n', ' ');
            string test2 = test1.Replace('\r', ' ');
            string text = test2.Trim();

            if (text.Contains("<!-")){
                return false;}
            if (text.Contains("&")){
                return false;}

            if (text.Length > 0){
                listValidText.Add(text);
                SeparateWord(text);
                return true;}

            return false;
        }
        /// <summary>
        /// Reads a sentence and separates it in words
        /// </summary>
        /// <param name="text">Sentence to be seprated into words</param>
        private void SeparateWord(string text)
        {
            int indexFirstSpace = 0;
            string word = string.Empty;
            string restSentence = string.Empty;

            while (text.Length > 0)
            {
                indexFirstSpace = text.IndexOf(' ');
                if (indexFirstSpace > 0)
                {
                    word = text.Substring(0, indexFirstSpace);
                    if (word.Length > 3)
                    {
                        AddWord(word.ToLower());
                    }
                    text = text.Remove(0, indexFirstSpace + 1);
                    text = text.Trim();
                }
                else
                {
                    text = text.Trim();
                    if (text.Length > 3)
                    {
                        AddWord(text.ToLower());

                    }
                    text = text.Remove(0, text.Length);
                }
            }
        }
        /// <summary>
        /// Adds the word in the collection of keywords
        /// </summary>
        /// <param name="word">word to be added to the collection of keywords</param>
        /// <remarks>
        /// Numbers and words like: home, about and blog are not added
        /// </remarks>
        private void AddWord(string word)
        {
            if (word.Contains("home") || word.Contains("about") || word.Contains("blog") || word.Contains("#")
             || word.Contains("0") || word.Contains("1") || word.Contains("2") || word.Contains("3") || word.Contains("4")
              || word.Contains("5") || word.Contains("6") || word.Contains("7") || word.Contains("8") || word.Contains("9"))
            {
                return;
            }
            listKeywords.Add(word);
        }
        /// <summary>
        /// Records the times the word appears in the website to keep a ranking
        /// </summary>
        /// <returns>Collection of keywords order by the top ranking.</returns>
        private Dictionary<string, int> WeightWords()
        {
            Dictionary<string, int> dictionaryContract = new Dictionary<string, int>();

            foreach (string word in listKeywords)
            {
                if (!dictionaryContract.ContainsKey(word))
                {
                    var linkAlreadyAnalyzed = from word1 in listKeywords where word1 == word select word1;
                    if (linkAlreadyAnalyzed.ToList().Count > 0)
                    {
                        dictionaryContract.Add(word, linkAlreadyAnalyzed.ToList().Count);
                    }
                }
            }
            dictionaryContract = dictionaryContract.OrderByDescending(i => i.Value).ToDictionary(c => c.Key.ToString(), c => c.Value);
            return dictionaryContract;
        }
        #endregion
    }
}

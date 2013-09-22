using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace mnBookmarkAnalyzer
{
    public class KeywordAnalyzer
    {
        #region Members
        List<string> listValidText =null;
        List<string> listKeywords = null;
        #endregion

        #region Constructor
        public KeywordAnalyzer()
        {
            listValidText = new List<string>();
            listKeywords = new List<string>();
        }
        #endregion

        #region Methods
        public Dictionary<string, int> GetKeywordsFromUrl(string urlAnalyze)
        {
            listKeywords = new List<string>();

            HtmlAgilityPack.HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = null;
            try
            {
                doc = web.Load(urlAnalyze);
            }
            catch (Exception ex)
            {
                return null;
            }

            foreach (HtmlNode main1 in doc.DocumentNode.ChildNodes)
            {
                foreach (HtmlNode main2 in main1.ChildNodes)
                {
                    foreach (HtmlNode main3 in main2.ChildNodes)
                    {
                        foreach (HtmlNode main4 in main3.ChildNodes)
                        {
                            foreach (HtmlNode main5 in main4.ChildNodes)
                            {
                                foreach (HtmlNode main6 in main5.ChildNodes)
                                {
                                    foreach (HtmlNode main7 in main6.ChildNodes)
                                    {
                                        if (main7.InnerText != null)
                                        {
                                            ValidText(main7.InnerText);
                                        }
                                        foreach (HtmlNode main8 in main7.ChildNodes)
                                        {
                                            if (main8.InnerText != null)
                                            {
                                                ValidText(main8.InnerText);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return WeightWords();
        }
        private bool ValidText(string innerText)
        {
            string test0 = innerText;
            string test1 = test0.Replace('\n', ' ');
            string test2 = test1.Replace('\r', ' ');
            string text = test2.Trim();

            if (text.Contains("<!-"))
            {
                return false;
            }
            if (text.Contains("&"))
            {
                return false;
            }

            if (text.Length > 0)
            {
                listValidText.Add(text);
                SeparateWord(text);
                return true;
            }

            return false;
        }
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
                        ExistWord(word.ToLower());
                    }
                    text = text.Remove(0, indexFirstSpace + 1);
                    text = text.Trim();
                }
                else
                {
                    text = text.Trim();
                    if (text.Length > 3)
                    {
                        ExistWord(text.ToLower());

                    }
                    text = text.Remove(0, text.Length);
                }
            }
        }
        private void ExistWord(string word)
        {
            if (word.Contains("home") || word.Contains("about") || word.Contains("blog") || word.Contains("#")
             || word.Contains("0") || word.Contains("1") || word.Contains("2") || word.Contains("3") || word.Contains("4")
              || word.Contains("5") || word.Contains("6") || word.Contains("7") || word.Contains("8") || word.Contains("9"))
            {
                return;
            }
            listKeywords.Add(word);
        }
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

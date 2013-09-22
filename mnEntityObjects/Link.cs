using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnEntityObjects
{
    /// <summary>
    /// Impplementation of ILink which is the link found after crawling a website
    /// </summary>
    public class Link : ILink
    {
        #region Members
        int id;
        string url;
        double raiting;
        List<string> keywords;
        #endregion

        #region Properties
        /// <summary>
        /// Id of the link
        /// </summary>
        public int Id
        {
            set { id = value; }
            get { return id; }
        }
        /// <summary>
        /// Url address
        /// </summary>
        public string Url
        {
            set { url = value; }
            get { return url; }
        }
        /// <summary>
        /// Ranking weight
        /// </summary>
        public double Ranking
        {
            set { raiting = value; }
            get { return raiting; }
        }
        /// <summary>
        /// Collections of keywords found in the website
        /// </summary>
        public List<string> Keywords
        {
            set { keywords = value; }
            get { return keywords; }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnEntityObjects
{
    /// <summary>
    /// Link found after crawling a website
    /// </summary>
    public interface ILink
    {
        /// <summary>
        /// Id of the link
        /// </summary>
        int Id { get;set; }
        /// <summary>
        /// Url address
        /// </summary>
        string Url { get; set; }
        /// <summary>
        /// Ranking weight
        /// </summary>
        double Ranking { get; set; }
        /// <summary>
        /// Collections of keywords found in the website
        /// </summary>
        List<string> Keywords { get; set; }
    }
}

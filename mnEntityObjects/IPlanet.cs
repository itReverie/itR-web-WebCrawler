using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnEntityObjects
{
    /// <summary>
    /// Planet is a website stored in a bookmark file.
    /// </summary>
    public interface IPlanet
    {
        /// <summary>
        /// Id of the planet
        /// </summary>
        int ID { get; set; }
        /// <summary>
        /// Name of the planet,it's the same set in the bookmark file
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// List of links found after crawling the website
        /// </summary>
        List<ILink> ListLinks { get; set; }
    }
}

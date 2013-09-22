using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnEntityObjects
{
    /// <summary>
    /// Implementation of IPlanet which is a website stored in a bookmark file.
    /// </summary>
    public class Planet : IPlanet
    {
        #region Members
        int id;
        string name;
        List<ILink> listLinks;
        #endregion

        #region Properties
        /// <summary>
        /// Id of the planet
        /// </summary>
        public int ID
        {
            set {id= value;}
            get{return id;}
        }
        /// <summary>
        /// Name of the planet,it's the same set in the bookmark file
        /// </summary>
        public string Name
        {
            set { name = value; }
            get { return name; }
        }
        /// <summary>
        /// List of links found after crawling the website
        /// </summary>
        public List<ILink> ListLinks
        {
            set { listLinks = value; }
            get { return listLinks; }
        }
        #endregion
    }
}

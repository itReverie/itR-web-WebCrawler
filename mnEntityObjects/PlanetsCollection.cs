using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnEntityObjects
{
    /// <summary>
    /// Implementation of IPlanetsCollection which is a collection of planets stored in a bookmark file
    /// </summary>
    public class PlanetsCollection : IPlanetsCollection
    {
        #region Members
        Dictionary<string, List<string>> _planetsCollection = new Dictionary<string, List<string>>();
        #endregion

        #region Methods
        /// <summary>
        /// Collection of planets at list of links found on the website
        /// </summary>
        public Dictionary<string, List<string>> _PlanetsCollection
        {
            get { return _planetsCollection;}
            set { _planetsCollection = value;}
        }
        #endregion
    }
}

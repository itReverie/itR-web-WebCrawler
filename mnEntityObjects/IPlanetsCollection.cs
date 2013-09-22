using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mnEntityObjects
{
    /// <summary>
    /// Collection of planets stored in a bookmark file
    /// </summary>
    public interface IPlanetsCollection
    {
        /// <summary>
        /// Collection of planets at list of links found on the website
        /// </summary>
        Dictionary<string, List<string>> _PlanetsCollection { get; set; }
    }
}

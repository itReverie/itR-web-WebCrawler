using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;
using mnEntityObjects;
using mnXMLGenerator;
using System.Configuration;

namespace mnBookmarkAnalyzer
{
    /// <summary>
    /// Class that reads an html bookmark file.
    /// Header -> Creates a new Planet
    /// Links -> Creates a new Links for the corresponding Planet
    /// </summary>
    public class LinkAnalyzer
    {
        #region Members
        XMLPlanetsGenerator xmlPlanetsGenerator;
        PlanetsCollection listPlanets;
        XmlNode xmlPlanetsCollection = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor of the class that initializes 
        /// PlanetManager, 
        /// PlanetsCollection which keeps record of all the planets found
        /// XMLPlanetGenerator which is an auxiliar file to keep record of the planets, links and keywords found
        /// </summary>
        public LinkAnalyzer()
        {
            xmlPlanetsGenerator = new XMLPlanetsGenerator();
            listPlanets = new PlanetsCollection();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Based on a html bookmark file it returns a collection of planets and its links
        /// </summary>
        /// <param name="fileName">html bookmark file path</param>
        /// <returns>Collection of planets and its links</returns>
        public PlanetsCollection GetLinksFromFavoritesFile(string fileName)
        {
            int counterPlanets = -1;
            //Generating an XML file with the main root as PlanetsCollection
            xmlPlanetsCollection = xmlPlanetsGenerator.CreatePlanetsCollection();
            int iterationCounter = 0;
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(fileName);

                HtmlNodeCollection nodeCollectionLinksTest = doc.DocumentNode.SelectNodes("/dl/dt/dl/dt");
                foreach (HtmlNode node in nodeCollectionLinksTest)
                {
                    CrawlDT(node, xmlPlanetsCollection, iterationCounter);
                }
                xmlPlanetsGenerator.CloseDocument();
            }
            catch (Exception exception)
            {
                xmlPlanetsGenerator.CloseDocument();
                throw exception;
                //System.Diagnostics.Debug.Write(exception.Message);
            }
            return listPlanets;
        }

        /// <summary>
        /// Recursive function that crawls the bookmark html file and adds:
        /// New planet if it finds  h3
        /// Call the link creation if it finds   dl
        /// </summary>
        /// <param name="htmlNode">current html node to crawl</param>
        /// <param name="xmlPlanetsCollection">Auxiliar XML node with the collection of Planets. CollectionPlanets is the root of the file</param>
        /// <param name="iterationCounter"> Auxiliar variable to delimit how many planets will be read. At the moment we are limiting the counter to 70</param>
        private void CrawlDT(HtmlNode htmlNode, XmlNode xmlPlanetsCollection, int iterationCounter)
        {
            try
            {
                //If you want to get more results, change this variable
                Int16 crawlingLevel = Convert.ToInt16(ConfigurationManager.AppSettings["CrawlingLevel"].ToString());
                if (iterationCounter <= crawlingLevel)//TODO: Allow unlimited crawling of planets (iterationCounter)
                {
                    Planet planet = null;
                    System.Xml.XmlNode planetNode = null;

                    //Select all dt
                    string xPathDT = "dt";
                    HtmlNodeCollection tagPlanet = htmlNode.SelectNodes(xPathDT);
                    if (tagPlanet != null)
                    {
                        foreach (HtmlNode planetHtmlNode in tagPlanet)
                        {
                            //Create the Planet
                            /// <example>
                            /// <H3 ADD_DATE=1363288860 LAST_MODIFIED=1363534113>Arsenal fans site</H3> 
                            /// </example>
                            string xPathPlanetSearch = "h3";
                            HtmlNode planetH3 = planetHtmlNode.SelectSingleNode(xPathPlanetSearch);
                            if (planetH3 != null)
                            {
                                planet = AddPlanet(planetH3.InnerText);
                                planetNode = xmlPlanetsGenerator.CreatePlanet(xmlPlanetsCollection, planet.Name);
                                iterationCounter = iterationCounter + 1;
                            }
                            //Create the Links
                            string xPathLinkSearch = "dl";
                            HtmlNodeCollection linkDL = planetHtmlNode.SelectNodes(xPathLinkSearch);
                            if (planet != null && linkDL != null)
                            {
                                CrawlHrefs(linkDL, planet, planetNode, iterationCounter);
                            }
                            if (planetHtmlNode != null)
                            {
                                CrawlDT(planetHtmlNode, xmlPlanetsCollection, iterationCounter);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                xmlPlanetsGenerator.CloseDocument();
                //TODO: At the moment if there is an invalid url it's skipped and continue with the others
                System.Diagnostics.Debug.Write(exception.Message);
            }
        }

        /// <summary>
        /// Recursive function that crawls the bookmark html file and adds:
        /// New link  if it finds   a href
        /// </summary>
        /// <param name="htmlNodeCollection">current html node to crawl</param>
        /// <param name="planet">Planet to add the links</param>
        /// <param name="xmlPlanetNode">Auxiliar XML node with the planet. Planets is the parent for the link</param>
        /// <param name="iterationCounter">Auxiliar variable to delimit how many planets will be read. At the moment we are limiting the counter to 70</param>
         private void CrawlHrefs(HtmlNodeCollection htmlNodeCollection, Planet planet, XmlNode xmlPlanetNode, int iterationCounter)
        {
            try
            {
                if (htmlNodeCollection != null)
                {
                    foreach (HtmlNode dtNode in htmlNodeCollection)
                    {
                        if (dtNode.ChildNodes.Count() == 5)
                        {
                            CrawlDT(dtNode, xmlPlanetsCollection, iterationCounter);
                        }
                        else
                        {
                            /// <example>
                            ///<A HREF=http://www.allgoonerdup.com/ ADD_DATE=1363292833>allgoonerdup.com</A>
                            /// </example>
                            string xPathASearch = "a[@href]";
                            HtmlNode nodeA = dtNode.SelectSingleNode(xPathASearch);
                            if (nodeA != null)
                            {
                                //Add Link to teh Planet
                                string url = nodeA.Attributes["href"].Value.ToString();
                                if (url != null)
                                {
                                    AddLink(url, planet, xmlPlanetNode);
                                }
                            }
                            string xPathDTSearch = "dt";
                            HtmlNodeCollection hrefsLinks = dtNode.SelectNodes(xPathDTSearch);
                            CrawlHrefs(hrefsLinks, planet, xmlPlanetNode, iterationCounter);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                xmlPlanetsGenerator.CloseDocument();
                //TODO: At the moment if there is an invalid url it's skipped and continue with the others
                System.Diagnostics.Debug.Write(exception.Message);
            }
        }

        /// <summary>
        /// Creates and adds a planet in the list
        /// </summary>
        /// <param name="planetName">Name of the planet</param>
        /// <returns>Planet object</returns>
        private Planet AddPlanet(string planetName)
        {
            Planet planet = new Planet();
            planet.Name = planetName;
            listPlanets._PlanetsCollection.Add(planet.Name, new List<string>());
            return planet;
        }

        /// <summary>
        /// Createas and adds a link to the corresponding planet
        /// </summary>
        /// <param name="url">url of the link</param>
        /// <param name="planet">Planet to add teh link</param>
        /// <param name="xmlPlanetNode">Auxiliar XML node with the planet. Planets is the parent for the link</param>
        private void AddLink(string url, Planet planet, XmlNode xmlPlanetNode)
        {
            Link link = new Link();
            link.Url = url;
            if (listPlanets._PlanetsCollection.ContainsKey(planet.Name))
            {
                XmlNode linkNode = xmlPlanetsGenerator.CreateLink(xmlPlanetNode);
                XmlNode nodeLinkUrl = xmlPlanetsGenerator.CreateUrl(linkNode, url);
                listPlanets._PlanetsCollection.Last().Value.Add(url);

                List<string> listString = null;
                listString=AddKeywords(url, linkNode, planet);
            }
        }

        /// <summary>
        /// Creates and adds the keywords to the corresponding link
        /// </summary>
        /// <param name="url">url of the link</param>
        /// <param name="xmlLinkNode">Auxiliar XML node with the link. Link is the parent for the keyword</param>
        private List<string> AddKeywords(string url, XmlNode xmlLinkNode, Planet planet)
        {
            KeywordAnalyzer keywordAnalyzer = new KeywordAnalyzer();
            Dictionary<string, int> listKeywords = keywordAnalyzer.GetKeywordsFromUrl(url);
            int counterWords = 0;
            List<string> listString = new List<string>();

            if (listKeywords != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, int> pair in listKeywords)
                {
                    if (counterWords < 10 && pair.Value > 1)//TODO: At the moment there is a top of 10 keywords
                    {
                        xmlPlanetsGenerator.CreateKeyword(xmlLinkNode, pair.Key, pair.Value);
                        listString.Add(pair.Key);
                        counterWords++;
                    }
                }
            }
            return listString;
        }
        #endregion
    }
}

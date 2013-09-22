using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace mnXMLGenerator
{
    /// <summary>
    /// Creates the XML result format for links and keywords fown in a website
    /// </summary>
    public class XMLPlanetsGenerator
    {
        #region Members
        XmlDocument doc = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor to create the XMLDocument
        /// </summary>
        public XMLPlanetsGenerator()
        {
            doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates the main branch for the collections of planets
        /// </summary>
        /// <returns>XMLNode for the PlanetsCollection</returns>
        public XmlNode CreatePlanetsCollection()
        {
            XmlNode linkCollection = doc.CreateElement("PlanetsCollection");
            doc.AppendChild(linkCollection);
            return linkCollection;
        }

        /// <summary>
        /// Creates the node for the planet and its corresponding name as an attribute
        /// </summary>
        /// <param name="xmlPlanetCollection">Collection brach to add the planet node</param>
        /// <param name="planetName">Name of the planet to add</param>
        /// <returns>XmlNode for the Planet</returns>
        public XmlNode CreatePlanet(XmlNode xmlPlanetCollection, string planetName)
        {

            XmlAttribute xmlAttributeName = doc.CreateAttribute("Name");
            xmlAttributeName.Value = planetName;

            XmlNode xmlLink = doc.CreateElement("Planet");
            xmlLink.Attributes.Append(xmlAttributeName);

            xmlPlanetCollection.AppendChild(xmlLink);
            return xmlLink;
        }

        /// <summary>
        /// Creates the node for the link 
        /// </summary>
        /// <param name="xmlLinkCollection">Collection brach to add the Link node</param>
        /// <returns>XmlNode for the Link</returns>
        public XmlNode CreateLink(XmlNode xmlLinkCollection)
        {
            XmlNode xmlLink = doc.CreateElement("Link");
            try
            { 
                xmlLinkCollection.AppendChild(xmlLink);
            }
            catch (Exception ex)
            {
                return null;
            }
            return xmlLink;
        }

        /// <summary>
        /// Creates the node for the url 
        /// </summary>
        /// <param name="xmlLink">Collection brach to add the url node</param>
        /// <param name="url">url address</param>
        /// <returns>XmlNode for the url</returns>
        public XmlNode CreateUrl(XmlNode xmlLink, string url)
        {
            XmlNode xmlUrl = doc.CreateElement("url");
            try
            {
                if (url.Length < 250)
                {
                    xmlUrl.InnerXml = url;
                    xmlLink.AppendChild(xmlUrl);
                   
                }
                else
                {
                    xmlUrl.InnerXml = "Error";
                    xmlLink.AppendChild(xmlUrl);
                }
                return xmlUrl;
            }
            catch (Exception ex)
            {
                xmlUrl.InnerXml = "Error";
                xmlLink.AppendChild(xmlUrl);
                return null;
            }
            return xmlUrl;
        }

        /// <summary>
        /// Creates the node for the keywordCollection 
        /// </summary>
        /// <param name="xmlLink">Collection brach to add the keywordCollection</param>
        /// <returns>XmlNode for the keyword collection</returns>
        public XmlNode CreateKeywordsCollection(XmlNode xmlLink)
        {
            XmlNode xmlKeywordCollection = doc.CreateElement("keywordCollection");
            xmlLink.AppendChild(xmlKeywordCollection);
            return xmlKeywordCollection;
        }

        /// <summary>
        /// Creates the node for the keyword 
        /// </summary>
        /// <param name="keywordsCollection">Collection brach to add the keyword node</param>
        /// <param name="keyword">keyword</param>
        /// <param name="weight">certain weight for the keywords to identify the top "n"</param>
        /// <returns>XmlNode for the keyword</returns>
        public XmlNode CreateKeyword(XmlNode keywordsCollection, string keyword, int weight)
        {
            List<char> notAllowed = new List<char>();
            notAllowed.Add('<');
            notAllowed.Add('"');
            notAllowed.Add('$');
            notAllowed.Add('>');
            notAllowed.Add('(');
            notAllowed.Add(')');
            notAllowed.Add('*');
            notAllowed.Add('|');
            notAllowed.Add('/');
            notAllowed.Add('\\');

            if (keyword.ToArray().Where(c => notAllowed.Contains(c)).ToArray().Count() > 0)
            {
                return null;
            }

            XmlNode xmlKeyword = doc.CreateElement("keyword");

            try
            {

                XmlAttribute weightAttribute = doc.CreateAttribute("weight");
                weightAttribute.Value = weight.ToString();
                xmlKeyword.Attributes.Append(weightAttribute);
                
                xmlKeyword.InnerXml = keyword;
                keywordsCollection.AppendChild(xmlKeyword);

            }
            catch (Exception ex)
            {
                doc.Save(ConfigurationManager.AppSettings["mnPlanetFormat"].ToString());
            }

            return xmlKeyword;
        }

        /// <summary>
        /// Closes the XML document
        /// </summary>
        public void CloseDocument()
        {
            string resultFileName= ConfigurationManager.AppSettings["mnPlanetFormat"].ToString();
            //if (!File.Exists(resultFileName))
            //{
            //    File.Create(resultFileName);
            //}
            doc.Save(resultFileName);
        }

        #endregion
    }
}

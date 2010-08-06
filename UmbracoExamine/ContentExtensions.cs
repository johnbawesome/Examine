﻿using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using umbraco;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;
using Examine.LuceneEngine;

namespace UmbracoExamine
{
    /// <summary>
    /// Static methods to help query umbraco xml
    /// </summary>
    public static class LinqXmlExtensions
    {

        /// <summary>
        /// Converts a content node to XDocument
        /// </summary>
        /// <param name="node"></param>
        /// <param name="cacheOnly">true if data is going to be returned from cache</param>
        /// <returns></returns>
        /// <remarks>
        /// If the type of node is not a Document, the cacheOnly has no effect, it will use the API to return
        /// the xml. 
        /// </remarks>
        public static XDocument ToXDocument(this Content node, bool cacheOnly)
        {
            if (cacheOnly && node.GetType().Equals(typeof(Document)))
            {
                var umbXml = library.GetXmlNodeById(node.Id.ToString());
                return umbXml.ToXDocument();
            }

            //if it's not a using cache and it's not cacheOnly, then retrieve the Xml using the API

            XmlDocument xDoc = new XmlDocument();
            var xNode = xDoc.CreateNode(XmlNodeType.Element, "node", "");
            node.XmlPopulate(xDoc, ref xNode, false);

            if (xNode.Attributes["nodeTypeAlias"] == null)
            {
                //we'll add the nodeTypeAlias ourselves                                
                XmlAttribute d = xDoc.CreateAttribute("nodeTypeAlias");
                d.Value = node.ContentType.Alias;
                xNode.Attributes.Append(d);
            }

            return new XDocument(xNode.ToXElement());
        }

    }
}

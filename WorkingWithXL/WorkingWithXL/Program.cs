using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace WorkingWithXL
{
    class Program
    {
        /// <summary>
        /// Examples based on documentation on https://csharp.net-tutorials.com/xml/introduction/
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Resources\WikimediaExample.xml");
            //UsingXmlReader(path);
            //UsingXmlDocument(path);
            UsingXmlDocumentWithXPath(path);

        }

        /// <summary>
        /// The XmlReader is a faster and less memory-consuming alternative. 
        /// It lets you run through the XML content one element at a time, while allowing you to look at the value, and then moves on to the next element. 
        /// By doing so, it obviously consumes very little memory because it only holds the current element, 
        /// and because you have to manually check the values, you will only get the relevant elements, making it very fast. 
        /// </summary>
        private static void UsingXmlReader(string path)
        {
            XmlReader xmlReader = XmlReader.Create(path);

            while (xmlReader.Read())
            {
                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "project"))
                {
                    if (xmlReader.HasAttributes)
                        Console.WriteLine(xmlReader.GetAttribute("name") + " : " + xmlReader.GetAttribute("launch"));

                }
                else if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "edition"))
                {
                    if (xmlReader.HasAttributes)
                        Console.WriteLine(xmlReader.GetAttribute("language"));
                }
            }

            Console.ReadKey();

        }

        /// <summary>
        /// The XmlDocument is more memory consuming and possibly a bit slower than the XmlReader approach. 
        /// However, for many purposes, the XmlDocument can be easier to work with and often require less code. 
        /// Once the XML content has been read, you can read the data in a hierarchical way, 
        /// just like the XML structure, with a root element which can have child elements, 
        /// which can have child elements, and so on.
        /// </summary>
        /// <param name="path"></param>
        private static void UsingXmlDocument(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes[0].ChildNodes)
            {
                Console.WriteLine(xmlNode.Attributes["name"].Value + " : " + xmlNode.Attributes["launch"].Value);

                foreach (XmlNode xmlNodeItem in xmlNode.FirstChild.ChildNodes)
                {
                    Console.WriteLine(xmlNodeItem.Attributes["language"].Value);

                    Console.WriteLine("Inner Text :" + xmlNodeItem.InnerText);
                    Console.WriteLine("Inner Xml: " + xmlNodeItem.InnerXml);
                    Console.WriteLine("Outer Xml : " + xmlNodeItem.OuterXml);
                }
            }

        }

        /// XPath is maintained by the same organization which created the XML standard. 
        /// XPath is basically a query language for selecting nodes from an XML, with lots possibilities. 
        /// It is very powerful and extremly easy to ready. In this example we will look into some basic queries.
        /// 
        /// The XmlDocument class has several methods which takes an XPath query as a parameter and then returns 
        /// the resulting XmlNode(s). 
        /// In this chapter we will look into two methods: The SelectSingleNode() method, 
        /// which returns a single XmlNode based on the provided XPath query, 
        /// and the SelectNodes() method, which returns a XmlNodeList collection of XmlNode objects 
        /// based on the provided XPath query.
        private static void UsingXmlDocumentWithXPath(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNodeList itemNodes = xmlDoc.SelectNodes("//Wikimedia//projects//project");
            foreach (XmlNode itemNode in itemNodes)
            {
                Console.WriteLine(itemNode.Attributes["name"].Value + " : " + itemNode.Attributes["launch"].Value);

                foreach (XmlNode item in itemNode.SelectSingleNode("editions"))                
                {
                    Console.WriteLine(item.Attributes["language"].Value);
                }

            }

            Console.ReadKey();
        }
    }
}

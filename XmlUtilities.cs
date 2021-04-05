/*
 * Copyright (c) 2012 acDevSoftware
 * http://www.acdevsoftware.ch
 * 
 * This file is part of acTelerikStylesAssembly.
 *
 * acTelerikStylesAssembly is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.

 * acTelerikStylesAssembly is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.

 * You should have received a copy of the GNU Lesser General Public License
 * along with acTelerikStylesAssembly.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace acTelerikStylesAssembly
{
    public class XmlUtilities
    {
        public static void SaveObject<T>(string sFilename, T nData)
        {
            //Create the text writer to write the XML
            using (TextWriter nXmlWriter = new StreamWriter(sFilename))
            {
                //Create the serializer to serialize data of type ItemList
                XmlSerializer nXmlSerializer = new XmlSerializer(nData.GetType());
                if (nXmlSerializer == null)
                    throw new XmlException("XmlUtilities::SaveData: XmlSerializer could not be instantiated");

                // This is a trick to remove the namespace automatically added by Serializer
                XmlSerializerNamespaces nNoNameSpace = new XmlSerializerNamespaces();
                if (nNoNameSpace == null)
                    throw new XmlException("XmlUtilities::SaveData: XmlSerializerNamespaces could not be instantiated");
                nNoNameSpace.Add("", "");

                //Serialize the list of models into the writer
                nXmlSerializer.Serialize(nXmlWriter, nData, nNoNameSpace);

                //Close the writer
                nXmlWriter.Close();
            }
        }

        public static T LoadObject<T>(string sFilename)
        {
            //Create the reader to read from XML
            using (TextReader nXmlReader = new StreamReader(sFilename))
            {
                //Create the serializer to deserialize data of type ItemList
                XmlSerializer nXmlSerializer = new XmlSerializer(typeof(T));

                //Deserialize the reader into the list of items
                T nData = (T)nXmlSerializer.Deserialize(nXmlReader);

                //Close the reader
                nXmlReader.Close();

                return nData;
            }
        }
    }
}

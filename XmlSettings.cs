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
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace acTelerikStylesAssembly
{
    [XmlRoot("Settings")]
    public class XmlSettings
    {
        public const string FILENAME = "acTelerikStylesAssembly.xml";

        private string m_sOutputDirectory;
        private string m_sAssemblyName;
        private string m_sAssemblyVersion;
        private string m_sTelerikWebUIPath;
        private List<string> m_nStylesFileNames = new List<string>();

        public XmlSettings()
        {
        }

        [XmlElement("AssemblyName")]
        public string AssemblyName
        {
            get { return m_sAssemblyName; }
            set { m_sAssemblyName = value; }
        }

        [XmlElement("AssemblyVersion")]
        public string AssemblyVersion
        {
            get { return m_sAssemblyVersion; }
            set { m_sAssemblyVersion = value; }
        }

        [XmlElement("OutputDirectory")]
        public string OutputDirectory
        {
            get { return m_sOutputDirectory; }
            set { m_sOutputDirectory = value; }
        }

        [XmlElement("StylesFileNames")]
        public List<string> StylesFileNames
        {
            get { return m_nStylesFileNames; }
            set { m_nStylesFileNames = value; }
        }

        [XmlElement("TelerikWebUIPath")]
        public string TelerikWebUIPath
        {
            get { return m_sTelerikWebUIPath; }
            set { m_sTelerikWebUIPath = value; }
        }
    }
}

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
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace acTelerikStylesAssembly
{
    /// <summary>
    /// Main application to build Telerik Styles assembly.
    /// </summary>
    public class acTelerikStylesAssemblyApplication : IInitializeDispose
    {
        private Form _mainForm = null;
        private MainConsole _mainConsole = null;

        private StylesProcessing m_nStylesProcessing = null;
        private AssemblyGenerator m_nAssemblyGenerator = null;
        private XmlSettings m_nXmlSettings = null;
        private ApplicationSettings m_nApplicationSettings = null;

        public acTelerikStylesAssemblyApplication()
        {
            SetupInternal();

            SetupUI();
        }

        private void SetupInternal()
        {
            m_nStylesProcessing = new StylesProcessing();
            m_nXmlSettings = new XmlSettings();
            m_nApplicationSettings = new ApplicationSettings();
            m_nAssemblyGenerator = new AssemblyGenerator();
        }

        private void SetupUI()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        private void InitializeStylesProcessing()
        {
            m_nStylesProcessing.Initialize();
        }

        private void InitializeAssemblyGenerator()
        {
            m_nAssemblyGenerator.Initialize();
        }

        public void Initialize()
        {
            InitializeStylesProcessing();
            InitializeAssemblyGenerator();
        }

        private void DisposeStylesProcessing()
        {
            m_nStylesProcessing.Dispose();
        }

        private void DisposeAssemblyGenerator()
        {
            m_nAssemblyGenerator.Dispose();
        }

        public void Dispose()
        {
            DisposeStylesProcessing();
            DisposeAssemblyGenerator();
        }

        private T GetAssemblyInformation<T>() where T : Attribute
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(T), false);
            if (attributes.Length == 0)
                return null;
            else
                return (T)attributes[0];
        }

        public string AssemblyTitle
        {
            get
            {
                AssemblyTitleAttribute nAssemblyTitleAttribute = GetAssemblyInformation<AssemblyTitleAttribute>();

                if (nAssemblyTitleAttribute != null && !string.IsNullOrEmpty(nAssemblyTitleAttribute.Title))
                    return nAssemblyTitleAttribute.Title;
                else
                    return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                AssemblyDescriptionAttribute nAssemblyDescriptionAttribute = GetAssemblyInformation<AssemblyDescriptionAttribute>();

                if (nAssemblyDescriptionAttribute != null)
                    return nAssemblyDescriptionAttribute.Description;
                else
                    return string.Empty;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                AssemblyProductAttribute nAssemblyProductAttribute = GetAssemblyInformation<AssemblyProductAttribute>();

                if (nAssemblyProductAttribute != null)
                    return nAssemblyProductAttribute.Product;
                else
                    return string.Empty;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                AssemblyCopyrightAttribute nAssemblyCopyrightAttribute = GetAssemblyInformation<AssemblyCopyrightAttribute>();

                if (nAssemblyCopyrightAttribute != null)
                    return nAssemblyCopyrightAttribute.Copyright;
                else
                    return string.Empty;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                AssemblyCompanyAttribute nAssemblyCompanyAttribute = GetAssemblyInformation<AssemblyCompanyAttribute>();

                if (nAssemblyCompanyAttribute != null)
                    return nAssemblyCompanyAttribute.Company;
                else
                    return string.Empty;
            }
        }

        public string AssemblyDate
        {
            get
            {
                AssemblyConfigurationAttribute nAssemblyConfigurationAttribute = GetAssemblyInformation<AssemblyConfigurationAttribute>();

                if (nAssemblyConfigurationAttribute != null)
                    return nAssemblyConfigurationAttribute.Configuration;
                else
                    return string.Empty;
            }
        }

        public AssemblyGenerator AssemblyGenerator
        {
            get { return m_nAssemblyGenerator; }
        }

        public XmlSettings XmlSettings
        {
            get { return m_nXmlSettings; }
            set { if (value != null) m_nXmlSettings = value; }
        }

        public ApplicationSettings ApplicationSettings
        {
            get { return m_nApplicationSettings; }
            set { if (value != null) m_nApplicationSettings = value; }
        }

        public StylesProcessing StylesProcessing
        {
            get { return m_nStylesProcessing; }
        }

        public string ApplicationPath
        {
            get { return Path.GetDirectoryName(Application.ExecutablePath); }
        }

        public void RunFormApp()
        {
            _mainForm = new MainForm();
            Application.Run(_mainForm);
        }

        public int RunConsoleApp(string[] nArguments)
        {
            _mainConsole = new MainConsole();
            return _mainConsole.Run(nArguments);
        }
    }
}

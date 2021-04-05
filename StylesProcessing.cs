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
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using ICSharpCode.SharpZipLib.Zip;

namespace acTelerikStylesAssembly
{
    /// <summary>
    /// Generates Telerik styles information by correcting style urls from Telerik StyleBuilder application (http://stylebuilder.telerik.com, version of 6 february 2012).
    /// </summary>
    /// <remarks>
    /// Several parts of this file have been reworked from Bryan Boudreau source code "ResourceGenerator.cs"
    /// Web: www.vitalz.com, www.fusionworlds.com, www.facebook.com/banshi
    /// Date: 08/03/2009
    /// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
    /// and associated documentation files (the "Software"), to deal in the Software without restriction, 
    /// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
    /// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is 
    /// furnished to do so, subject to the following conditions:
    /// The above copyright notice and this permission notice shall be included in all copies or 
    /// substantial portions of the Software.
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING 
    /// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
    /// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
    /// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
    /// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    /// </remarks>
    public class StylesInformation
    {
        private Dictionary<string, string> m_nStyles;
        private DirectoryInfo m_nStylesDirectory;
        private List<string> m_nStylesFiles;

        public StylesInformation(DirectoryInfo nStylesDirectory, List<string> nStylesFiles)
        {
            m_nStyles = new Dictionary<string, string>();
            m_nStylesDirectory = nStylesDirectory;
            m_nStylesFiles = nStylesFiles;
        }

        public Dictionary<string, string> Styles
        {
            get { return m_nStyles; }
        }

        public DirectoryInfo StylesDirectory
        {
            get { return m_nStylesDirectory; }
        }

        public List<string> StylesFiles
        {
            get { return m_nStylesFiles; }
        }
    }

    public class StylesProcessing: ProgressHandler,IInitializeDispose
    {
        public const string PROCESS_CODE = "StylesProcessing";
        public const string MIMETYPE_TEXTCSS = "text/css";

        public const string EXTENSIONFILE_CSS = ".css";

        private const char VIRTUALPATH_SEPARATOR = '/';

        private const string WEBRESOURCE_URL = "url(<%= WebResource(\"{0}\") %>)";
        private const string WEBRESOURCE_ASSEMBLY = "[assembly: WebResource(\"{0}\", \"{1}\")]";

        private readonly Dictionary<string, string> m_nMimeTypes = new Dictionary<string,string>() {
            { EXTENSIONFILE_CSS, MIMETYPE_TEXTCSS },
            { ".jpg", "image/jpg" },
            { ".gif", "image/gif" },
            { ".png", "image/png" },
            { ".ico", "image/x-icon" },
            { ".cur", "image/x-icon" }
        };

        private Regex m_nCorrectStyleFileRegEx = new Regex(@"(?<![-])\burl\b\((.+)\)");

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }

        private StylesInformation InitializeProcessing(string sOutputDirectory, List<string> nStylesFiles)
        {
            OnProgressStatus(new ProgressStatusEventArg(PROCESS_CODE, ProgressStatusEventArg.StatusType.STATUS_INITIALIZED));

            return new StylesInformation(FileDirUtilities.CreateDirectory(sOutputDirectory), nStylesFiles);
        }

        private void UnzipStylesFile(StylesInformation nStylesInformation, string sStylesFile)
        {
            DirectoryInfo nStylesDirectory = nStylesInformation.StylesDirectory.CreateSubdirectory(Path.GetFileNameWithoutExtension(sStylesFile));

            FastZip nFastZip = new FastZip();
            nFastZip.ExtractZip(sStylesFile, nStylesDirectory.FullName, FastZip.Overwrite.Always, null, string.Empty, string.Empty, true);

            OnProgressProcessing(new ProgressProcessingEventArg(string.Format("\r\nFile {0} unzipped into {1}\r\n", sStylesFile, nStylesDirectory.FullName)));
        }

        private string GetMimeTypeFromAcceptedExtension(string sFileExtension)
        {
            if (!m_nMimeTypes.ContainsKey(sFileExtension))
                return string.Empty;

            return m_nMimeTypes[sFileExtension];
        }

        private string CombineVirtualPath(string sPath1, string sPath2)
        {
            return string.Format("{0}{1}{2}", sPath1, VIRTUALPATH_SEPARATOR, sPath2);
        }

        private void CorrectCSSFile(string sStyleNamespace, string sCSSFile)
        {
            string sStyleFileRead;

            using (StreamReader nStreamReader = File.OpenText(sCSSFile))
            {
                sStyleFileRead = nStreamReader.ReadToEnd();
                nStreamReader.Close();
            }

            string sStyleFileCorrected = m_nCorrectStyleFileRegEx.Replace(
                 sStyleFileRead,
                 new MatchEvaluator(delegate(Match nMatch)
                 {
                     if (nMatch.Groups.Count == 2)
                     {
                         string sURL = nMatch.Value.Replace("url('", string.Empty).Replace("')", string.Empty);
                         if(string.IsNullOrEmpty(sURL))
                             throw new Exception(string.Format("An Url is empty in {0}", sCSSFile));

                         if (sURL.StartsWith("ImageHandler.ashx"))
                         {
                             NameValueCollection nameValueCollection = CollectionUtilities.CollectionFromString(sURL.Replace("ImageHandler.ashx", ""));

                             sURL = CombineVirtualPath(nameValueCollection["control"], nameValueCollection["file"]);
                         }
                         else if(sURL.StartsWith("../"))
                         {
                             sURL = sURL.Replace("../", string.Empty);
                        }

                         string[] nImagePathName = sURL.Split(new char[] { VIRTUALPATH_SEPARATOR });
                         if (nImagePathName.Length < 2)
                             throw new Exception(string.Format("Url {0} not recognized in {1}", sURL, sCSSFile));

                         string sStyleCSSNamespace = string.Format("{0}.{1}", sStyleNamespace, String.Join(".", nImagePathName));

                         return string.Format(WEBRESOURCE_URL, sStyleCSSNamespace);
                     }
                     return nMatch.Value;
                 }
            ));

            // Write the modification into the same file
            using (StreamWriter streamWriter = File.CreateText(sCSSFile))
            {
                streamWriter.Write(sStyleFileCorrected);
                streamWriter.Close();
            }
        }

        private void ProcessStylesFile(ref StylesInformation nStylesInformation, string sParentNamespace, DirectoryInfo nParentDirectory)
        {
            string sNamespace = string.Format("{0}.{1}", sParentNamespace, nParentDirectory.Name);

            FileInfo[] nStylesSubFiles = nParentDirectory.GetFiles();
            foreach (FileInfo nFileInfo in nStylesSubFiles)
            {
                string sStyleNamespace = string.Format("{0}.{1}", sNamespace, nFileInfo.Name),
                       sMimeType = GetMimeTypeFromAcceptedExtension(nFileInfo.Extension);

                if (!string.IsNullOrEmpty(sMimeType))
                {
                    nStylesInformation.Styles.Add(sStyleNamespace, sMimeType);

                    OnProgressProcessing(new ProgressProcessingEventArg(string.Format(WEBRESOURCE_ASSEMBLY, sStyleNamespace, sMimeType)));

                    if (string.Compare(nFileInfo.Extension, EXTENSIONFILE_CSS, true) == 0)
                        CorrectCSSFile(sNamespace, nFileInfo.FullName);
                }
                else
                    OnProgressProcessing(new ProgressProcessingEventArg(string.Format("No mime type for extension {0}", nFileInfo.Extension)));
            }
        }

        private void ProcessStylesDirectory(ref StylesInformation nStylesInformation, string sParentNamespace, DirectoryInfo nParentDirectory)
        {
            DirectoryInfo[] nStylesFileSubDirectories = nParentDirectory.GetDirectories();
            foreach (DirectoryInfo nDirectoryInfo in nStylesFileSubDirectories)
            {
                string sNamespace = string.Format("{0}.{1}", sParentNamespace, nParentDirectory.Name);

                ProcessStylesDirectory(ref nStylesInformation, sNamespace, nDirectoryInfo);
            }

            ProcessStylesFile(ref nStylesInformation, sParentNamespace, nParentDirectory);
        }

        private void ProcessStylesFileDirectory(ref StylesInformation nStylesInformation, string sAssemblyName, string stylesFile)
        {
            DirectoryInfo nStylesDirectory = nStylesInformation.StylesDirectory.CreateSubdirectory(Path.GetFileNameWithoutExtension(stylesFile));

            ProcessStylesDirectory(ref nStylesInformation, sAssemblyName, nStylesDirectory);
        }

        public StylesInformation Process(string sAssemblyName, string sOutputDirectory, List<string> nStylesFiles)
        {
            try
            {
                StylesInformation nStylesInformation = InitializeProcessing(sOutputDirectory, nStylesFiles);

                foreach (string sStyleFile in nStylesFiles)
                {
                    UnzipStylesFile(nStylesInformation, sStyleFile);

                    ProcessStylesFileDirectory(ref nStylesInformation, sAssemblyName, sStyleFile);
                }

                OnProgressStatus(new ProgressStatusEventArg(PROCESS_CODE, ProgressStatusEventArg.StatusType.STATUS_COMPLETED));

                return nStylesInformation;
            }
            catch
            {
                OnProgressStatus(new ProgressStatusEventArg(PROCESS_CODE, ProgressStatusEventArg.StatusType.STATUS_ERROR));

                throw;
            }
        }
    }
}

/*
 * Copyright (c) 2012-2015 acDevSoftware
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
using System.Threading;
using System.Reflection;
using System.Reflection.Emit;
using System.Web.UI;
using System.IO;
using System.Globalization;


namespace acTelerikStylesAssembly
{
    /// <summary>
    /// Generates a final Telrik Skins assembly from all telerik skin files.
    /// </summary>
    /// <remarks>
    /// Several parts of this file have been reworked from Bryan Boudreau source code "AssemblyGenerator.cs"
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
    public class AssemblyGenerator : ProgressHandler, IInitializeDispose
    {
        public const string PROCESS_CODE = "AssemblyGeneratorProcessing";
        public const string DEFAULT_ASSEMBLY_VERSION = "1.0.0.0";
        public const string TELERIK_WEB_UI_ASSEMBLY_NAME = "Telerik.Web.UI.dll";

        public class AssemblyGeneratorInformation
        {
            public string AssemblyName { get; private set;  }
            public string AssemblyVersion { get; private set; }
            public string AssemblyFileName { get; private set; }
            public string AssemblyCulture { get; private set; }
            public string OutputDirectory { get; private set; }

            public AssemblyGeneratorInformation(string sAssemblyName, string sAssemblyVersion, string sAssemblyCulture, string sOutputDirectory)
            {
                AssemblyName = FileDirUtilities.RemoveFileExtension(sAssemblyName);
                AssemblyVersion = (!string.IsNullOrEmpty(sAssemblyVersion) ? sAssemblyVersion : DEFAULT_ASSEMBLY_VERSION);
                AssemblyFileName = string.Format("{0}.dll", sAssemblyName);
                AssemblyCulture = sAssemblyCulture;

                OutputDirectory = sOutputDirectory;
            }
        }

        private struct RadControlType
        {
            public string m_sControlName;
            public string m_sShortControlName;
            public string m_sControlTypeName;

            public RadControlType(string sControlName, string sControlTypeName)
            {
                m_sControlName = sControlName;
                m_sShortControlName = string.Empty;
                m_sControlTypeName = sControlTypeName;
            }
            public RadControlType(string sControlName, string sShortControlName, string sControlTypeName)
            {
                m_sControlName = sControlName;
                m_sShortControlName = sShortControlName;
                m_sControlTypeName = sControlTypeName;
            }
        };

        private struct RadSkinType
        {
            public string m_sSkinName;
            public Type m_nControlType;

            public RadSkinType(string sSkinName, Type nControlType)
            {
                m_sSkinName = sSkinName;
                m_nControlType = nControlType;
            }
        };

        private readonly MultiValueDictionary<string, RadControlType> m_nControlTypes = new MultiValueDictionary<string, RadControlType>() {
            { "Ajax", new RadControlType("RadAjaxLoadingPanel", "Telerik.Web.UI.RadAjaxLoadingPanel") },
            { "Upload", new RadControlType("RadAsyncUpload", "Telerik.Web.UI.RadAsyncUpload") },
            { "AutoCompleteBox", new RadControlType("RadAutoCompleteBox", "Telerik.Web.UI.RadAutoCompleteBox") },
            { "Button", new RadControlType("RadButton", "Telerik.Web.UI.RadButton") },
            { "Calendar", new RadControlType("RadCalendar", "Telerik.Web.UI.RadCalendar") },
            { "Chart", new RadControlType("RadChart", "Telerik.Web.UI.RadChart") },
            { "CloudUpload", new RadControlType("RadCloudUpload", "Telerik.Web.UI.RadCloudUpload") },
            { "ColorPicker", new RadControlType("RadColorPicker", "Telerik.Web.UI.RadColorPicker") },
            { "ComboBox", new RadControlType("RadComboBox", "Telerik.Web.UI.RadComboBox") },           
            { "DataForm", new RadControlType("RadDataForm", "Telerik.Web.UI.RadDataForm") },
            { "DataPager", new RadControlType("RadDataPager", "Telerik.Web.UI.RadDataPager") },
            { "DatePicker", new RadControlType("RadDatePicker", "Telerik.Web.UI.RadDatePicker") },
            { "DateTimePicker", new RadControlType("RadDateTimePicker", "Telerik.Web.UI.RadDateTimePicker") },
            { "Diagram", new RadControlType("RadDiagram", "Telerik.Web.UI.RadDiagram") },
            { "Dock", new RadControlType("RadDock", "Telerik.Web.UI.RadDock") },
            { "Dock", new RadControlType("RadDockZone", "Telerik.Web.UI.RadDockZone") },
            { "DropDownList", new RadControlType("RadDropDownList", "Telerik.Web.UI.RadDropDownList") },
            { "DropDownTree", new RadControlType("RadDropDownTree", "Telerik.Web.UI.RadDropDownTree") },
            { "Editor", new RadControlType("RadEditor", "Telerik.Web.UI.RadEditor") },
            { "Editor", new RadControlType("UserControlResources", "Widgets", "Telerik.Web.UI.Dialogs.UserControlResources") },
            { "Editor", new RadControlType("EditorToolsBase", "Telerik.Web.UI.Dialogs.UserControlResources") },
            { "FileExplorer", new RadControlType("RadFileExplorer", "Telerik.Web.UI.RadFileExplorer") },
            { "Filter", new RadControlType("RadFilter", "Telerik.Web.UI.RadFilter") },
            { "FormDecorator", new RadControlType("RadFormDecorator", "Telerik.Web.UI.RadFormDecorator") },
            { "Gantt", new RadControlType("RadGantt", "Telerik.Web.UI.RadGantt") },
            { "Grid", new RadControlType("RadGrid", "Telerik.Web.UI.RadGrid") },
            { "HtmlChart", new RadControlType("RadHtmlChart", "Telerik.Web.UI.RadHtmlChart") },
            { "ImageEditor", new RadControlType("RadImageEditor", "Telerik.Web.UI.RadImageEditor") },
            { "ImageGallery", new RadControlType("RadImageGallery", "Telerik.Web.UI.RadImageGallery") },
            { "Input", new RadControlType("RadInputControl", "Telerik.Web.UI.RadInputControl") },
            { "Input", new RadControlType("RadInputManager", "Telerik.Web.UI.RadInputManager") },
            { "LightBox", new RadControlType("RadLightBox", "Telerik.Web.UI.RadLightBox") },
            { "LinearGauge", new RadControlType("RadLinearGauge", "Telerik.Web.UI.RadLinearGauge") },
            { "ListBox", new RadControlType("RadListBox", "Telerik.Web.UI.RadListBox") },
            { "ListView", new RadControlType("RadListView", "Telerik.Web.UI.RadListView") },
            { "Map", new RadControlType("RadMap", "Telerik.Web.UI.RadMap") },
            { "MediaPlayer", new RadControlType("RadMediaPlayer", "Telerik.Web.UI.RadMediaPlayer") },
            { "Menu", new RadControlType("RadMenu", "Telerik.Web.UI.RadMenu") },
            { "Calendar", new RadControlType("RadMonthYearPicker", "Telerik.Web.UI.RadMonthYearPicker") },
            { "Notification", new RadControlType("RadNotification", "Telerik.Web.UI.RadNotification") },
            { "OrgChart", new RadControlType("RadOrgChart", "Telerik.Web.UI.RadOrgChart") },
            { "PageLayout", new RadControlType("RadPageLayout", "Telerik.Web.UI.RadPageLayout") },
            { "PanelBar", new RadControlType("RadPanelBar", "Telerik.Web.UI.RadPanelBar") },
            { "PivotGrid", new RadControlType("RadPivotGrid", "Telerik.Web.UI.RadPivotGrid") },
            { "Upload", new RadControlType("RadProgressArea", "Telerik.Web.UI.RadProgressArea") },
            { "RadialGauge", new RadControlType("RadRadialGauge", "Telerik.Web.UI.RadRadialGauge") },
            { "Rating", new RadControlType("RadRating", "Telerik.Web.UI.RadRating") },
            { "RibbonBar", new RadControlType("RadRibbonBar", "Telerik.Web.UI.RadRibbonBar") },
            { "Rotator", new RadControlType("RadRotator", "Telerik.Web.UI.RadRotator") },
            { "Scheduler", new RadControlType("RadScheduler", "Telerik.Web.UI.RadScheduler") },
            { "Scheduler", new RadControlType("RecurrenceEditor", "SchedulerRecurrenceEditor", "Telerik.Web.UI.RecurrenceEditor") },
            { "Scheduler", new RadControlType("ReminderDialog", "SchedulerReminderDialog", "Telerik.Web.UI.ReminderDialog") },
            { "SearchBox", new RadControlType("RadSearchBox", "Telerik.Web.UI.RadSearchBox") },
            { "Slider", new RadControlType("RadSlider", "Telerik.Web.UI.RadSlider") },
            { "SiteMap", new RadControlType("RadSiteMap", "Telerik.Web.UI.RadSiteMap") },
            { "SocialShare", new RadControlType("RadSocialShare", "Telerik.Web.UI.RadSocialShare") },
            { "Spell", new RadControlType("RadSpell", "Telerik.Web.UI.RadSpell") },
            { "Splitter", new RadControlType("RadSplitter", "Telerik.Web.UI.RadSplitter") },
            { "TabStrip", new RadControlType("RadTabStrip", "Telerik.Web.UI.RadTabStrip") },
            { "TagCloud", new RadControlType("RadTagCloud", "Telerik.Web.UI.RadTagCloud") },
            { "Tile", new RadControlType("RadBaseTile", "Telerik.Web.UI.RadBaseTile") },
            { "TileList", new RadControlType("RadTileList", "Telerik.Web.UI.RadTileList") },
            { "TimePicker", new RadControlType("RadTimePicker", "Telerik.Web.UI.RadTimePicker") },
            { "Calendar", new RadControlType("RadTimeView", "Telerik.Web.UI.RadTimeView") },
            { "ToolBar", new RadControlType("RadToolBar", "Telerik.Web.UI.RadToolBar") },
            { "ToolTip", new RadControlType("RadToolTipBase", "Telerik.Web.UI.RadToolTipBase") },
            { "TreeList", new RadControlType("RadTreeList", "Telerik.Web.UI.RadTreeList") },
            { "TreeMap", new RadControlType("RadTreeMap", "Telerik.Web.UI.RadTreeMap") },
            { "TreeView", new RadControlType("RadTreeView", "Telerik.Web.UI.RadTreeView") },
            { "Upload", new RadControlType("RadUpload", "Telerik.Web.UI.RadUpload") },
            { "Upload", new RadControlType("RadProgressManager", "Telerik.Web.UI.RadUpload") },
            { "Window", new RadControlType("RadWindowBase", "Telerik.Web.UI.RadWindowBase") },
            { "Wizard", new RadControlType("RadWizard", "Telerik.Web.UI.RadWizard") }
        };

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }

        private bool TryToLoadTelerikWebUIAssembly(string sTelerikWebUIPath, ref Assembly nTelerikWebUIAssembly)
        {
            nTelerikWebUIAssembly = null;

            if (!string.IsNullOrEmpty(sTelerikWebUIPath))
            {
                string sTelerikWebUIFullPath = Path.Combine(sTelerikWebUIPath, TELERIK_WEB_UI_ASSEMBLY_NAME);

                if (File.Exists(sTelerikWebUIFullPath))
                    nTelerikWebUIAssembly = Assembly.LoadFrom(sTelerikWebUIFullPath);
            }

            return nTelerikWebUIAssembly != null;
        }

        private Assembly InitializeTelerikWebUIAssembly(string sTelerikDllDirectory)
        {
            Assembly nTelerikWebUIAssembly = null;

            if (TryToLoadTelerikWebUIAssembly(sTelerikDllDirectory, ref nTelerikWebUIAssembly))
                return nTelerikWebUIAssembly;

            if (TryToLoadTelerikWebUIAssembly(Program.Application.ApplicationSettings.TelerikWebUIPath, ref nTelerikWebUIAssembly))
                return nTelerikWebUIAssembly;

            if (TryToLoadTelerikWebUIAssembly(Program.Application.ApplicationPath, ref nTelerikWebUIAssembly))
                return nTelerikWebUIAssembly;

            throw new Exception(string.Format("Impossible to find {0} in '{1}';'{2}';'{3}'", TELERIK_WEB_UI_ASSEMBLY_NAME, sTelerikDllDirectory, Program.Application.ApplicationSettings.TelerikWebUIPath, Program.Application.ApplicationPath));
        }

        private AssemblyName InitializeAssemblyName(AssemblyGeneratorInformation nAssemblyGeneratorInformation)
        {
            AssemblyName nAssemblyName = new AssemblyName();
            nAssemblyName.Name = nAssemblyGeneratorInformation.AssemblyName;
            nAssemblyName.CodeBase = nAssemblyGeneratorInformation.OutputDirectory;
            nAssemblyName.Version = new Version(nAssemblyGeneratorInformation.AssemblyVersion);

            if (!string.IsNullOrEmpty(nAssemblyGeneratorInformation.AssemblyCulture))
                nAssemblyName.CultureInfo = new CultureInfo(nAssemblyGeneratorInformation.AssemblyCulture);

            return nAssemblyName;
        }

        private void ProcessWebResourcesAttribute(ref AssemblyBuilder nAssemblyBuilder, StylesInformation nStylesInformation)
        {
            foreach (string sKey in nStylesInformation.Styles.Keys)
            {
                Type[] nConstructorTypes = new Type[] { typeof(string), typeof(string) };
                ConstructorInfo nConstructionInfo = typeof(WebResourceAttribute).GetConstructor(nConstructorTypes);
                CustomAttributeBuilder nCustomAttributeBuilder;

                if (string.Compare(nStylesInformation.Styles[sKey], StylesProcessing.MIMETYPE_TEXTCSS) != 0)
                {
                    nCustomAttributeBuilder = new CustomAttributeBuilder(
                                                    nConstructionInfo,
                                                    new object[] { sKey, nStylesInformation.Styles[sKey] });
                }
                else
                {
                    PropertyInfo[] nPropertyInfos = new PropertyInfo[] { typeof(WebResourceAttribute).GetProperty("PerformSubstitution") };

                    nCustomAttributeBuilder = new CustomAttributeBuilder(
                                                    nConstructionInfo,
                                                    new object[] { sKey, nStylesInformation.Styles[sKey] },
                                                    nPropertyInfos,
                                                    new object[] { true });
                }

                nAssemblyBuilder.SetCustomAttribute(nCustomAttributeBuilder);
            }
        }

        private AssemblyBuilder InitializeAssemblyBuilder(AssemblyName nAssemblyName, StylesInformation nStylesInformation)
        {
            AppDomain nAppDomain = Thread.GetDomain();
            AssemblyBuilder nAssemblyBuilder = nAppDomain.DefineDynamicAssembly(nAssemblyName, AssemblyBuilderAccess.RunAndSave, nAssemblyName.CodeBase);

            //add the web resource attributes to the assembly
            ProcessWebResourcesAttribute(ref nAssemblyBuilder, nStylesInformation);

            return nAssemblyBuilder;
        }

        private void ProcessFilesIntoResourceManifest(ref ModuleBuilder nModuleBuilder, StylesInformation nStylesInformation, string sParentNamespace, DirectoryInfo nParentDirectory)
        {
            string sNamespace = string.Format("{0}.{1}", sParentNamespace, nParentDirectory.Name);

            FileInfo[] nStylesSubFiles = nParentDirectory.GetFiles();
            foreach (FileInfo nFileInfo in nStylesSubFiles)
            {
                string sFilename = string.Format("{0}.{1}", sNamespace, nFileInfo.Name);

                nModuleBuilder.DefineManifestResource(sFilename, nFileInfo.Open(FileMode.Open, FileAccess.Read), ResourceAttributes.Public);
            }
        }

        private void ProcessDirectoryIntoResourceManifest(ref ModuleBuilder nModuleBuilder,StylesInformation nStylesInformation, string sParentNamespace, DirectoryInfo nParentDirectory)
        {
            DirectoryInfo[] nStylesFileSubDirectories = nParentDirectory.GetDirectories();
            foreach (DirectoryInfo nDirectoryInfo in nStylesFileSubDirectories)
            {
                string sNamespace = string.Format("{0}.{1}", sParentNamespace, nParentDirectory.Name);

                ProcessDirectoryIntoResourceManifest(ref nModuleBuilder, nStylesInformation, sNamespace, nDirectoryInfo);
            }

            ProcessFilesIntoResourceManifest(ref nModuleBuilder, nStylesInformation, sParentNamespace, nParentDirectory);
        }

        private void ProcessFilesDirectoryIntoResourceManifest(ref ModuleBuilder nModuleBuilder, StylesInformation nStylesInformation, string sAssemblyName, string sStylesFile)
        {
            DirectoryInfo nStylesDirectory = nStylesInformation.StylesDirectory.CreateSubdirectory(Path.GetFileNameWithoutExtension(sStylesFile));

            ProcessDirectoryIntoResourceManifest(ref nModuleBuilder, nStylesInformation, sAssemblyName, nStylesDirectory);
        }

        private void ProcessStylesFilesIntoResourceManifest(ref ModuleBuilder nModuleBuilder, string sAssemblyName, StylesInformation nStylesInformation)
        {
            foreach (string sStylesFile in nStylesInformation.StylesFiles)
            {
                ProcessFilesDirectoryIntoResourceManifest(ref nModuleBuilder, nStylesInformation, sAssemblyName, sStylesFile);
            }
        }

        private string GetControlName(string sKey, string sSkinName, string sAssemblyName)
        {
            return sKey.Replace(Path.ChangeExtension(sSkinName, StylesProcessing.EXTENSIONFILE_CSS), string.Empty).Replace(string.Format("{0}.{1}", sAssemblyName, sSkinName), string.Empty).TrimStart('.').TrimEnd('.');
        }

        private void ProcessEmbeddedSkin(ref Assembly nTelerikWebUIAssembly, ref MultiValueDictionary<string, EmbeddedSkinAttribute> nControlSkins, string sAssemblyName, StylesInformation nStylesInformation, string sStyleFile)
        {
            string sSkinName = Path.GetFileNameWithoutExtension(sStyleFile),
                   sSkinNameKey = string.Format(".{0}.", sSkinName);

            foreach (string sKey in nStylesInformation.Styles.Keys)
            {
                if (string.Compare(nStylesInformation.Styles[sKey], StylesProcessing.MIMETYPE_TEXTCSS) == 0 &&
                    sKey.Contains(sSkinNameKey))
                {
                    string sShortControlName = GetControlName(sKey, sSkinName, sAssemblyName);

                    if (!string.IsNullOrEmpty(sShortControlName) && m_nControlTypes.ContainsKey(sShortControlName))
                    {
                        HashSet<RadControlType> nRadControlTypes = m_nControlTypes.GetValues(sShortControlName, true);

                        foreach (RadControlType nRadControlType in nRadControlTypes)
                        {
                            Type nTelerikType = nTelerikWebUIAssembly.GetType(nRadControlType.m_sControlTypeName);
                            if(nTelerikType == null)
                                OnProgressProcessing(new ProgressProcessingEventArg(string.Format("Telerik Type {0} not included in {1}", nRadControlType.m_sControlTypeName, TELERIK_WEB_UI_ASSEMBLY_NAME)));

                            nControlSkins.Add(nRadControlType.m_sControlName, new EmbeddedSkinAttribute((string.IsNullOrEmpty(nRadControlType.m_sShortControlName) ? sShortControlName : nRadControlType.m_sShortControlName), sSkinName, nTelerikType));
                        }
                    }
                }
            }
        }

        private void ProcessEmbeddedSkins(ref Assembly nTelerikWebUIAssembly, ref ModuleBuilder nModuleBuilder, string sAssemblyName, StylesInformation nStylesInformation)
        {
            MultiValueDictionary<string, EmbeddedSkinAttribute> nControlSkins = new MultiValueDictionary<string, EmbeddedSkinAttribute>();

            OnProgressProcessing(new ProgressProcessingEventArg("\r\nProcess Embedded Skins"));

            // Add only 1 common for all skins
            nControlSkins.Add("Common", null);

            foreach (string sStyleFile in nStylesInformation.StylesFiles)
            {
                ProcessEmbeddedSkin(ref nTelerikWebUIAssembly, ref nControlSkins, sAssemblyName, nStylesInformation, sStyleFile);
            }

            OnProgressProcessing(new ProgressProcessingEventArg(string.Format("Telerik.Web.UI assembly version: {0}", nTelerikWebUIAssembly.GetName().Version.ToString())));

            Type[] nConstructorTypes = new Type[] { typeof(string), typeof(string), typeof(Type) };
            Type nEmbeddedSkinAttributeType = nTelerikWebUIAssembly.GetType("Telerik.Web.EmbeddedSkinAttribute");
            ConstructorInfo nConstructionInfo = nEmbeddedSkinAttributeType.GetConstructor(nConstructorTypes);

            foreach (string sControlName in nControlSkins.Keys)
            {
                TypeBuilder nTypeBuilder = nModuleBuilder.DefineType(string.Format("{0}.{1}", sAssemblyName, sControlName), TypeAttributes.Public | TypeAttributes.Class);
                Type nRadClassType = nTypeBuilder.CreateType();
                
                HashSet<EmbeddedSkinAttribute> nEmbeddedSkinAttributes = nControlSkins.GetValues(sControlName, true);

                if (nEmbeddedSkinAttributes != null)
                {
                    foreach (EmbeddedSkinAttribute nEmbeddedSkinAttribute in nEmbeddedSkinAttributes)
                    {
                        if (nEmbeddedSkinAttribute != null)
                        {                             
                            CustomAttributeBuilder nCustomAttributeBuilder = new CustomAttributeBuilder(
                                                        nConstructionInfo,
                                                        new object[] { nEmbeddedSkinAttribute.ShortControlName, nEmbeddedSkinAttribute.Skin, nRadClassType });

                            nTypeBuilder.SetCustomAttribute(nCustomAttributeBuilder);

                            OnProgressProcessing(new ProgressProcessingEventArg(string.Format("EmbeddedSkin: {0}.{1} ({2})", nEmbeddedSkinAttribute.Skin, sControlName, nEmbeddedSkinAttribute.ShortControlName)));
                        }
                        else
                            OnProgressProcessing(new ProgressProcessingEventArg(string.Format("EmbeddedSkin: {0}", sControlName)));
                    }
                }
            }
        }

        public void Generate(string sAssemblyName, string sAssemblyVersion, string sAssemblyCulture, StylesInformation nStylesInformation, string sTelerikDllDirectory)
        {
            AssemblyGeneratorInformation nAssemblyGeneratorInformation = new AssemblyGeneratorInformation(sAssemblyName, sAssemblyVersion, sAssemblyCulture, nStylesInformation.StylesDirectory.FullName);

            try
            {
                OnProgressStatus(new ProgressStatusEventArg(PROCESS_CODE, ProgressStatusEventArg.StatusType.STATUS_INITIALIZED, nAssemblyGeneratorInformation));

                Assembly nTelerikWebUIAssembly = InitializeTelerikWebUIAssembly(sTelerikDllDirectory);

                AssemblyName nAssemblyName = InitializeAssemblyName(nAssemblyGeneratorInformation);
                AssemblyBuilder nAssemblyBuilder = InitializeAssemblyBuilder(nAssemblyName, nStylesInformation);

                // Define the module for the assembly
                ModuleBuilder nModuleBuilder = nAssemblyBuilder.DefineDynamicModule(nAssemblyGeneratorInformation.AssemblyFileName, nAssemblyGeneratorInformation.AssemblyFileName);

                ProcessStylesFilesIntoResourceManifest(ref nModuleBuilder, nAssemblyGeneratorInformation.AssemblyName, nStylesInformation);
                ProcessEmbeddedSkins(ref nTelerikWebUIAssembly, ref nModuleBuilder, nAssemblyGeneratorInformation.AssemblyName, nStylesInformation);

                // Create the telerik type definition so that the web resources will work.
                string sTelerikClassTypeName = string.Format("{0}.TelerikSkin", nAssemblyGeneratorInformation.AssemblyName);

                TypeBuilder nTypeBuilder = nModuleBuilder.DefineType(sTelerikClassTypeName, TypeAttributes.Public);
                nTypeBuilder.CreateType();

                // Finally save the assembly
                nAssemblyBuilder.Save(nAssemblyGeneratorInformation.AssemblyFileName);

                OnProgressStatus(new ProgressStatusEventArg(PROCESS_CODE, ProgressStatusEventArg.StatusType.STATUS_COMPLETED, nAssemblyGeneratorInformation));
            }
            catch
            {
                OnProgressStatus(new ProgressStatusEventArg(PROCESS_CODE, ProgressStatusEventArg.StatusType.STATUS_ERROR, nAssemblyGeneratorInformation));

                throw;
            }
        }
    }
}

/*
 * Copyright (c) 2015 acDevSoftware
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
using System.Runtime.InteropServices;

namespace acTelerikStylesAssembly
{
    public class MainConsole
    {
        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        private const string PROCESSING_ARGUMENT_HELP = "?";
        private const string PROCESSING_ARGUMENT_XMLSETTINGS = "X";
        private const string PROCESSING_ARGUMENT_ASSEMBLYNAME = "A";
        private const string PROCESSING_ARGUMENT_ASSEMBLYVERSION = "R";
        private const string PROCESSING_ARGUMENT_STYLESFILES = "S";
        private const string PROCESSING_ARGUMENT_BROWSEOUTPUTDIRECTORY = "O";
        private const string PROCESSING_ARGUMENT_BROWSETELERIKDLLDIRECTORY = "T";
        private const string PROCESSING_ARGUMENT_VERBOSE = "V";

        private bool VerboseLog { get; set; }

        public MainConsole()
        {
            SetupConsole();
            SetupInternal();
        }

        private void SetupConsole()
        {
             AttachConsole(ATTACH_PARENT_PROCESS);
        }

        private void SetupInternal()
        {
            Program.Application.StylesProcessing.ProgressProcessing += new StylesProcessing.ProgressProcessingHandler(ProgressProcessing_OnProgress);
            Program.Application.StylesProcessing.ProgressStatus += new StylesProcessing.ProgressStatusHandler(ProgressProcessing_OnStatus);
            Program.Application.AssemblyGenerator.ProgressProcessing += new AssemblyGenerator.ProgressProcessingHandler(ProgressProcessing_OnProgress);
            Program.Application.AssemblyGenerator.ProgressStatus += new AssemblyGenerator.ProgressStatusHandler(ProgressProcessing_OnStatus);
        }

        private ProcessingArguments GetProcessingArguments(string[] nArguments)
        {
            if (nArguments.Length > 0)
            {
                MultiValueDictionary<string, string> nArgumentValues = (new ConsoleArguments()).ParseArguments(nArguments);

                if (!nArgumentValues.ContainsKey(PROCESSING_ARGUMENT_HELP))
                {
                    ProcessingArguments nProcessingArguments = new ProcessingArguments();

                    if (!nArgumentValues.IsEmptyValue(PROCESSING_ARGUMENT_XMLSETTINGS))
                    {
                        Program.Application.XmlSettings = XmlUtilities.LoadObject<XmlSettings>(nArgumentValues.GetValue(PROCESSING_ARGUMENT_XMLSETTINGS, string.Empty));

                        nProcessingArguments.AssemblyName = Program.Application.XmlSettings.AssemblyName;
                        nProcessingArguments.AssemblyVersion = Program.Application.XmlSettings.AssemblyVersion;
                        nProcessingArguments.StylesFiles = Program.Application.XmlSettings.StylesFileNames;
                        nProcessingArguments.BrowseOutputDirectory = Program.Application.XmlSettings.OutputDirectory;
                        nProcessingArguments.BrowseTelerikDllDirectory = Program.Application.XmlSettings.TelerikWebUIPath;
                    }
                    else
                    {
                        nProcessingArguments.AssemblyName = nArgumentValues.GetValue(PROCESSING_ARGUMENT_ASSEMBLYNAME, string.Empty);
                        nProcessingArguments.AssemblyVersion = nArgumentValues.GetValue(PROCESSING_ARGUMENT_ASSEMBLYVERSION, string.Empty);
                        nProcessingArguments.StylesFiles = nArgumentValues.GetValues(PROCESSING_ARGUMENT_STYLESFILES, true).ToList();
                        nProcessingArguments.BrowseOutputDirectory = nArgumentValues.GetValue(PROCESSING_ARGUMENT_BROWSEOUTPUTDIRECTORY, string.Empty);
                        nProcessingArguments.BrowseTelerikDllDirectory = nArgumentValues.GetValue(PROCESSING_ARGUMENT_BROWSETELERIKDLLDIRECTORY, string.Empty);
                    }
                    nProcessingArguments.Verbose = nArgumentValues.ContainsKey(PROCESSING_ARGUMENT_VERBOSE);

                    return nProcessingArguments;
                }
            }

            return null;
        }

        private void ValidateProcessingArguments(ProcessingArguments nProcessingArguments)
        {
            if (!ProcessingArgumentsUtilities.ValidateAssemblyName(nProcessingArguments.AssemblyName))
                throw new ArgumentException("Assembly Name cannot be empty", PROCESSING_ARGUMENT_ASSEMBLYNAME);

            if (!ProcessingArgumentsUtilities.ValidateAssemblyVersion(nProcessingArguments.AssemblyVersion))
                throw new ArgumentException("Assembly Version has an incorrect format, must be (x.x.x.x)", PROCESSING_ARGUMENT_ASSEMBLYVERSION);

            if (!ProcessingArgumentsUtilities.ValidateStylesFiles(nProcessingArguments.StylesFiles))
                throw new ArgumentException("Invalid Styles files (missing or not zipped file)", PROCESSING_ARGUMENT_STYLESFILES);

            if (!ProcessingArgumentsUtilities.ValidateBrowseOutputDirectory(nProcessingArguments.BrowseOutputDirectory))
                throw new ArgumentException("Output Directory cannot be empty", PROCESSING_ARGUMENT_BROWSEOUTPUTDIRECTORY);
        }

        private void LogException(Exception nException)
        {
            Console.WriteLine();
            Console.WriteLine(nException.Message);
        }

        private void LogText(string sText)
        {
            if (!VerboseLog)
                Console.WriteLine(sText);
        }

        private void DisplayHelp()
        {
            Console.WriteLine();
            Console.WriteLine(string.Format("acTelerikStylesAssembly ({0} {1}): Usage from console", Program.Application.AssemblyVersion, Program.Application.AssemblyDate));
            Console.WriteLine();
            Console.WriteLine(string.Format("acTelerikStylesAssembly [/{0}] /{1}XmlSettingsFile /{2}AssemblyName [/{3}AssemblyVersion] /{4}StylesFilePath [/{4}StylesFilePath2...] /{5}BrowseOutputDirectory [/{6}BrowseTelerikDllDirectory] [/{7}]",
                                            PROCESSING_ARGUMENT_HELP, PROCESSING_ARGUMENT_XMLSETTINGS, PROCESSING_ARGUMENT_ASSEMBLYNAME, PROCESSING_ARGUMENT_ASSEMBLYVERSION, PROCESSING_ARGUMENT_STYLESFILES, PROCESSING_ARGUMENT_BROWSEOUTPUTDIRECTORY, PROCESSING_ARGUMENT_BROWSETELERIKDLLDIRECTORY, PROCESSING_ARGUMENT_VERBOSE));
            Console.WriteLine();
            Console.WriteLine(string.Format("/{0} : This help", PROCESSING_ARGUMENT_HELP));
            Console.WriteLine(string.Format("/{0}XmlSettingsFile : The path name of a Xml settings file, override all other command line arguments", PROCESSING_ARGUMENT_XMLSETTINGS));
            Console.WriteLine(string.Format("/{0}AssemblyName : The name of the assembly to generate, without dll extension", PROCESSING_ARGUMENT_ASSEMBLYNAME));
            Console.WriteLine(string.Format("/{0}AssemblyVersion : The version of the assembly to generate [x.x.x.x]", PROCESSING_ARGUMENT_ASSEMBLYVERSION));
            Console.WriteLine(string.Format("/{0}StylesFilePath : The path name of a zipped Telerik skin file", PROCESSING_ARGUMENT_STYLESFILES));
            Console.WriteLine(string.Format("/{0}BrowseOutputDirectory : The directory name where to save the generated assembly", PROCESSING_ARGUMENT_BROWSEOUTPUTDIRECTORY));
            Console.WriteLine(string.Format("/{0}BrowseTelerikDllDirectory : The directory name where to find the Telerik.Web.UI.dll", PROCESSING_ARGUMENT_BROWSETELERIKDLLDIRECTORY));
            Console.WriteLine(string.Format("/{0} : Verbose log, no log output except error and help", PROCESSING_ARGUMENT_VERBOSE));
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine();
            Console.WriteLine(string.Format("acTelerikStylesAssembly /{0}MyAssembly /{1}12.1.1.1 /{2}\"C:\\My SecondTest\\Test.zip\" /{2}\"C:\\My SecondTest\\Test2.zip\" /{3}C:\\MyTest /{4}\"C:\\My Test\" /V",
                                            PROCESSING_ARGUMENT_ASSEMBLYNAME, PROCESSING_ARGUMENT_ASSEMBLYVERSION, PROCESSING_ARGUMENT_STYLESFILES, PROCESSING_ARGUMENT_BROWSEOUTPUTDIRECTORY, PROCESSING_ARGUMENT_VERBOSE));
            Console.WriteLine();
            Console.WriteLine(string.Format("acTelerikStylesAssembly /{0}\"C:\\My SecondTest\\acTelerikStylesAssembly.xml\"", PROCESSING_ARGUMENT_XMLSETTINGS));
        }

        public int Run(string[] nArguments)
        {
            try
            {
                ProcessingArguments nProcessingArguments = GetProcessingArguments(nArguments);
                if (nProcessingArguments != null)
                {
                    ValidateProcessingArguments(nProcessingArguments);

                    VerboseLog = nProcessingArguments.Verbose;

                    try
                    {
                        StylesInformation nStylesGenerate = Program.Application.StylesProcessing.Process(nProcessingArguments.AssemblyName, nProcessingArguments.BrowseOutputDirectory, nProcessingArguments.StylesFiles);
                        string sAssemblyCulture = Program.Application.ApplicationSettings.AssemblyCulture;

                        Program.Application.AssemblyGenerator.Generate(nProcessingArguments.AssemblyName, nProcessingArguments.AssemblyVersion, sAssemblyCulture, nStylesGenerate, nProcessingArguments.BrowseTelerikDllDirectory);
                    }
                    catch (Exception nException)
                    {
                        LogException(nException);

                        return 5;
                    }
                }
                else
                {
                    DisplayHelp();

                    return 0;
                }
            }
            catch (Exception nException)
            {
                LogException(nException);

                return 5;
            }

            return 0;
        }

        private void ProgressProcessing_OnProgress(object sender, ProgressProcessingEventArg e)
        {
            LogText(e.ProgressInfo);
        }

        private void ProgressProcessing_OnStatus(object sender, ProgressStatusEventArg e)
        {
            string sStatusText = string.Empty;

            if (string.Compare(e.Code, StylesProcessing.PROCESS_CODE, true) == 0)
            {
                switch (e.Status)
                {
                    case ProgressStatusEventArg.StatusType.STATUS_INITIALIZED:
                        sStatusText = "Process of Zip File(s) initialized";
                        break;
                    case ProgressStatusEventArg.StatusType.STATUS_ERROR:
                        sStatusText = "Process of Zip File(s) aborted on error";
                        break;
                    case ProgressStatusEventArg.StatusType.STATUS_COMPLETED:
                        sStatusText = string.Format("Process of Zip File(s) completed");
                        break;
                }
            }
            else if (string.Compare(e.Code, AssemblyGenerator.PROCESS_CODE, true) == 0)
            {
                AssemblyGenerator.AssemblyGeneratorInformation nAssemblyGeneratorInformation = e.ProgressStatusInformation as AssemblyGenerator.AssemblyGeneratorInformation;

                switch (e.Status)
                {
                    case ProgressStatusEventArg.StatusType.STATUS_INITIALIZED:
                        sStatusText = "Generation of assembly initialized";
                        break;
                    case ProgressStatusEventArg.StatusType.STATUS_ERROR:
                        sStatusText = "Generation of assembly aborted on error";
                        break;
                    case ProgressStatusEventArg.StatusType.STATUS_COMPLETED:
                        sStatusText = string.Format("Generation of assembly completed, assembly {0} {1} generated", nAssemblyGeneratorInformation.AssemblyFileName, nAssemblyGeneratorInformation.AssemblyVersion);
                        break;
                }
            }

            LogText(sStatusText);
        }
    }
}

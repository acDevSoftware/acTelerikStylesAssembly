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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace acTelerikStylesAssembly
{
    public partial class MainForm : Form
    {
        private string m_sApplicationSettingsSaveFilename = XmlSettings.FILENAME;
        private string m_sApplicationSettingsLoadFilename = XmlSettings.FILENAME;

        public MainForm()
        {
            InitializeComponent();

            SetupInternal();
            SetupWindow();
            SetupControls();
        }

        private void SetupInternal()
        {
            Program.Application.StylesProcessing.ProgressProcessing += new StylesProcessing.ProgressProcessingHandler(ProgressProcessing_OnProgress);
            Program.Application.StylesProcessing.ProgressStatus += new StylesProcessing.ProgressStatusHandler(ProgressProcessing_OnStatus);
            Program.Application.AssemblyGenerator.ProgressProcessing += new AssemblyGenerator.ProgressProcessingHandler(ProgressProcessing_OnProgress);
            Program.Application.AssemblyGenerator.ProgressStatus += new AssemblyGenerator.ProgressStatusHandler(ProgressProcessing_OnStatus);
        }

        private void SetupWindow()
        {
            this.Text = string.Format("{0} v{1}", Program.Application.AssemblyTitle, Program.Application.AssemblyVersion);
        }

        private void SetupControls()
        {
            LoadSettingsToControls();
            HandleEnableControls();
        }

        private void LoadSettingsToControls()
        {
            try
            {
                lbxStylesFiles.Items.Clear();

                string[] nStyleFileNames = Program.Application.XmlSettings.StylesFileNames.ToArray();
                if (nStyleFileNames.Length > 0)
                    lbxStylesFiles.Items.AddRange(nStyleFileNames);

                txtBrowseOutputDirectory.Text = Program.Application.XmlSettings.OutputDirectory;
                txtAssemblyName.Text = Program.Application.XmlSettings.AssemblyName;
                txtAssemblyVersion.Text = Program.Application.XmlSettings.AssemblyVersion;
                txtBrowseTelerikDllDirectory.Text = Program.Application.XmlSettings.TelerikWebUIPath;
            }
            catch
            {
            }
        }

        private void SaveSettingsFromControls()
        {
            try
            {
                Program.Application.XmlSettings.StylesFileNames.Clear();
                Program.Application.XmlSettings.StylesFileNames.AddRange(lbxStylesFiles.Items.Cast<string>());

                Program.Application.XmlSettings.OutputDirectory = txtBrowseOutputDirectory.Text;
                Program.Application.XmlSettings.AssemblyName = txtAssemblyName.Text;
                Program.Application.XmlSettings.AssemblyVersion = txtAssemblyVersion.Text;

                Program.Application.XmlSettings.TelerikWebUIPath = txtBrowseTelerikDllDirectory.Text;
            }
            catch
            {
            }
        }

        private void HandleEnableControls()
        {
            btnDeleteStylesFiles.Enabled = (lbxStylesFiles.Items.Count > 0);
        }

        private void LogText(string sText)
        {
            txtLogger.Text = string.Concat(txtLogger.Text, string.Format("{0}\r\n", sText));
            UIUtilities.TextBox_ScrollToBottom(txtLogger);
        }

        private void LogException(Exception nException)
        {
            LogText(string.Format("ERROR: {0}", nException.Message));
        }

        private void ClearLog()
        {
            txtLogger.Clear();
        }

        private void ClearStatus()
        {
            tslStatus.Text = string.Empty;
        }

        private void btnBrowseStylesFiles_Click(object sender, EventArgs e)
        {
            ofdStylesFiles.FileName = string.Empty;

            if (ofdStylesFiles.ShowDialog() == DialogResult.OK)
            {
                lbxStylesFiles.Items.AddRange(ofdStylesFiles.FileNames);

                HandleEnableControls();
            }
        }

        private void btnDeleteStylesFiles_Click(object sender, EventArgs e)
        {
            for (int iIndex = lbxStylesFiles.SelectedIndices.Count - 1; iIndex >= 0; --iIndex)
            {
                lbxStylesFiles.Items.RemoveAt(lbxStylesFiles.SelectedIndices[iIndex]);
            }

            HandleEnableControls();
        }

        private void btnBrowseOutputDirectory_Click(object sender, EventArgs e)
        {
            fbdOutputDirectory.SelectedPath = txtBrowseOutputDirectory.Text;

            if (fbdOutputDirectory.ShowDialog() == DialogResult.OK)
            {
                txtBrowseOutputDirectory.Text = fbdOutputDirectory.SelectedPath;
            }
        }

        private void btnBrowseTelerikDllDirectory_Click(object sender, EventArgs e)
        {
            fbdTelerikDllDirectory.SelectedPath = txtBrowseTelerikDllDirectory.Text;

            if (fbdTelerikDllDirectory.ShowDialog() == DialogResult.OK)
            {
                txtBrowseTelerikDllDirectory.Text = fbdTelerikDllDirectory.SelectedPath;
            }
        }

        private void lbxStylesFiles_Validating(object sender, CancelEventArgs e)
        {
            if (!ProcessingArgumentsUtilities.ValidateStylesFiles(lbxStylesFiles.Items))
            {
                errGenerateAssembly.SetError(lbxStylesFiles, "Styles Files cannot be empty");

                e.Cancel = true;
            }
            else
                errGenerateAssembly.SetError(lbxStylesFiles, string.Empty);
        }

        private void txtBrowseOutputDirectory_Validating(object sender, CancelEventArgs e)
        {
            if (!ProcessingArgumentsUtilities.ValidateBrowseOutputDirectory(txtBrowseOutputDirectory.Text))
            {
                errGenerateAssembly.SetError(txtBrowseOutputDirectory, "Output Directory cannot be empty");

                e.Cancel = true;
            }
            else
                errGenerateAssembly.SetError(txtBrowseOutputDirectory, string.Empty);
        }

        private void txtAssemblyName_Validating(object sender, CancelEventArgs e)
        {
            if (!ProcessingArgumentsUtilities.ValidateAssemblyName(txtAssemblyName.Text))
            {
                errGenerateAssembly.SetError(txtAssemblyName, "Assembly Name cannot be empty");

                e.Cancel = true;
            }
            else
                errGenerateAssembly.SetError(txtAssemblyName, string.Empty);
        }

        private void txtAssemblyVersion_Validating(object sender, CancelEventArgs e)
        {
            if (!ProcessingArgumentsUtilities.ValidateAssemblyVersion(txtAssemblyVersion.Text))
            {
                errGenerateAssembly.SetError(txtAssemblyVersion, "Assembly Version has an incorrect format, must be (x.x.x.x)");

                e.Cancel = true;
            }
            else
                errGenerateAssembly.SetError(txtAssemblyVersion, string.Empty);
        }

        private void btnGenerateAssembly_Click(object sender, EventArgs e)
        {
            btnGenerateAssembly.Enabled = false;

            try
            {
                if (ValidateChildren())
                {
                    try
                    {
                        StylesInformation nStylesGenerate = Program.Application.StylesProcessing.Process(txtAssemblyName.Text, txtBrowseOutputDirectory.Text, lbxStylesFiles.Items.Cast<string>().ToList());
                        string sAssemblyCulture = Program.Application.ApplicationSettings.AssemblyCulture;

                        Program.Application.AssemblyGenerator.Generate(txtAssemblyName.Text, txtAssemblyVersion.Text, sAssemblyCulture, nStylesGenerate, txtBrowseTelerikDllDirectory.Text);
                    }
                    catch(Exception nException)
                    {
                        LogException(nException);
                    }
                }
            }
            catch (Exception nException)
            {
                LogException(nException);
            }
            finally
            {
                btnGenerateAssembly.Enabled = true;
            }
        }

        private void ProgressProcessing_OnProgress(object sender, ProgressProcessingEventArg e)
        {
            LogText(e.ProgressInfo);

            Application.DoEvents();
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

            tslStatus.Text = sStatusText;

            Application.DoEvents();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutBoxForm nAboutBoxForm = new AboutBoxForm();
            nAboutBoxForm.ShowDialog();
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                SaveSettingsFromControls();
                
                sfdSettings.FileName = m_sApplicationSettingsSaveFilename;

                if (sfdSettings.ShowDialog() == DialogResult.OK)
                {
                    m_sApplicationSettingsSaveFilename = sfdSettings.FileName;

                    XmlUtilities.SaveObject(sfdSettings.FileName, Program.Application.XmlSettings);
                }
            }
            catch (Exception nException)
            {
                LogException(nException);
            }
        }

        private void btnLoadSettings_Click(object sender, EventArgs e)
        {
            try
            {
                ofdSettings.FileName = m_sApplicationSettingsLoadFilename;

                if (ofdSettings.ShowDialog() == DialogResult.OK)
                {
                    m_sApplicationSettingsLoadFilename = ofdSettings.FileName;

                    Program.Application.XmlSettings = XmlUtilities.LoadObject<XmlSettings>(ofdSettings.FileName);

                    SetupControls();
                }
            }
            catch (Exception nException)
            {
                LogException(nException);
            }
        }

        private void lbxStylesFiles_DoubleClick(object sender, EventArgs e)
        {
            string sPathFilename = lbxStylesFiles.SelectedItem as string;
            
            ofdStylesFiles.FileName = Path.GetFileName(sPathFilename);
            ofdStylesFiles.InitialDirectory = Path.GetDirectoryName(sPathFilename);

            if (ofdStylesFiles.ShowDialog() == DialogResult.OK)
            {
                lbxStylesFiles.Items.AddRange(ofdStylesFiles.FileNames);

                HandleEnableControls();
            }
        }

        private void btnClearLogging_Click(object sender, EventArgs e)
        {
            try
            {
                ClearLog();
                ClearStatus();
            }
            catch
            {
            }
        }
    }
}

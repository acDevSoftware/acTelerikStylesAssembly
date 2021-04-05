namespace acTelerikStylesAssembly
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lbxStylesFiles = new System.Windows.Forms.ListBox();
            this.lblStyles = new System.Windows.Forms.Label();
            this.btnDeleteStylesFiles = new System.Windows.Forms.Button();
            this.btnBrowseOutputDirectory = new System.Windows.Forms.Button();
            this.txtBrowseOutputDirectory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAssemblyName = new System.Windows.Forms.TextBox();
            this.btnGenerateAssembly = new System.Windows.Forms.Button();
            this.ofdStylesFiles = new System.Windows.Forms.OpenFileDialog();
            this.btnBrowseStylesFiles = new System.Windows.Forms.Button();
            this.fbdOutputDirectory = new System.Windows.Forms.FolderBrowserDialog();
            this.errGenerateAssembly = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtLogger = new System.Windows.Forms.TextBox();
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.tslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAbout = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAssemblyVersion = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnLoadSettings = new System.Windows.Forms.Button();
            this.ofdSettings = new System.Windows.Forms.OpenFileDialog();
            this.sfdSettings = new System.Windows.Forms.SaveFileDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.btnBrowseTelerikDllDirectory = new System.Windows.Forms.Button();
            this.txtBrowseTelerikDllDirectory = new System.Windows.Forms.TextBox();
            this.fbdTelerikDllDirectory = new System.Windows.Forms.FolderBrowserDialog();
            this.btnClearLogging = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errGenerateAssembly)).BeginInit();
            this.stsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbxStylesFiles
            // 
            this.lbxStylesFiles.FormattingEnabled = true;
            this.lbxStylesFiles.Location = new System.Drawing.Point(105, 18);
            this.lbxStylesFiles.Name = "lbxStylesFiles";
            this.lbxStylesFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbxStylesFiles.Size = new System.Drawing.Size(722, 147);
            this.lbxStylesFiles.TabIndex = 1;
            this.lbxStylesFiles.DoubleClick += new System.EventHandler(this.lbxStylesFiles_DoubleClick);
            this.lbxStylesFiles.Validating += new System.ComponentModel.CancelEventHandler(this.lbxStylesFiles_Validating);
            // 
            // lblStyles
            // 
            this.lblStyles.CausesValidation = false;
            this.lblStyles.Location = new System.Drawing.Point(13, 18);
            this.lblStyles.Name = "lblStyles";
            this.lblStyles.Size = new System.Drawing.Size(86, 18);
            this.lblStyles.TabIndex = 0;
            this.lblStyles.Text = "Styles &Files:";
            this.lblStyles.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnDeleteStylesFiles
            // 
            this.btnDeleteStylesFiles.CausesValidation = false;
            this.btnDeleteStylesFiles.Enabled = false;
            this.btnDeleteStylesFiles.Location = new System.Drawing.Point(243, 170);
            this.btnDeleteStylesFiles.Name = "btnDeleteStylesFiles";
            this.btnDeleteStylesFiles.Size = new System.Drawing.Size(134, 23);
            this.btnDeleteStylesFiles.TabIndex = 3;
            this.btnDeleteStylesFiles.Text = "&Delete Styles Files";
            this.btnDeleteStylesFiles.UseVisualStyleBackColor = true;
            this.btnDeleteStylesFiles.Click += new System.EventHandler(this.btnDeleteStylesFiles_Click);
            // 
            // btnBrowseOutputDirectory
            // 
            this.btnBrowseOutputDirectory.CausesValidation = false;
            this.btnBrowseOutputDirectory.Location = new System.Drawing.Point(798, 204);
            this.btnBrowseOutputDirectory.Name = "btnBrowseOutputDirectory";
            this.btnBrowseOutputDirectory.Size = new System.Drawing.Size(29, 23);
            this.btnBrowseOutputDirectory.TabIndex = 6;
            this.btnBrowseOutputDirectory.Text = "...";
            this.btnBrowseOutputDirectory.UseVisualStyleBackColor = true;
            this.btnBrowseOutputDirectory.Click += new System.EventHandler(this.btnBrowseOutputDirectory_Click);
            // 
            // txtBrowseOutputDirectory
            // 
            this.txtBrowseOutputDirectory.Location = new System.Drawing.Point(105, 205);
            this.txtBrowseOutputDirectory.Name = "txtBrowseOutputDirectory";
            this.txtBrowseOutputDirectory.Size = new System.Drawing.Size(697, 20);
            this.txtBrowseOutputDirectory.TabIndex = 5;
            this.txtBrowseOutputDirectory.Validating += new System.ComponentModel.CancelEventHandler(this.txtBrowseOutputDirectory_Validating);
            // 
            // label1
            // 
            this.label1.CausesValidation = false;
            this.label1.Location = new System.Drawing.Point(13, 205);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "&Output path:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.CausesValidation = false;
            this.label2.Location = new System.Drawing.Point(13, 253);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Assembly &Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAssemblyName
            // 
            this.txtAssemblyName.Location = new System.Drawing.Point(105, 253);
            this.txtAssemblyName.Name = "txtAssemblyName";
            this.txtAssemblyName.Size = new System.Drawing.Size(305, 20);
            this.txtAssemblyName.TabIndex = 11;
            this.txtAssemblyName.Validating += new System.ComponentModel.CancelEventHandler(this.txtAssemblyName_Validating);
            // 
            // btnGenerateAssembly
            // 
            this.btnGenerateAssembly.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateAssembly.Location = new System.Drawing.Point(105, 280);
            this.btnGenerateAssembly.Name = "btnGenerateAssembly";
            this.btnGenerateAssembly.Size = new System.Drawing.Size(138, 23);
            this.btnGenerateAssembly.TabIndex = 15;
            this.btnGenerateAssembly.Text = "&Generate Assembly";
            this.btnGenerateAssembly.UseVisualStyleBackColor = true;
            this.btnGenerateAssembly.Click += new System.EventHandler(this.btnGenerateAssembly_Click);
            // 
            // ofdStylesFiles
            // 
            this.ofdStylesFiles.DefaultExt = "zip";
            this.ofdStylesFiles.Filter = "Styles Files (*.zip) | *.zip";
            this.ofdStylesFiles.Multiselect = true;
            // 
            // btnBrowseStylesFiles
            // 
            this.btnBrowseStylesFiles.CausesValidation = false;
            this.btnBrowseStylesFiles.Location = new System.Drawing.Point(105, 170);
            this.btnBrowseStylesFiles.Name = "btnBrowseStylesFiles";
            this.btnBrowseStylesFiles.Size = new System.Drawing.Size(132, 23);
            this.btnBrowseStylesFiles.TabIndex = 2;
            this.btnBrowseStylesFiles.Text = "&Add Styles Files...";
            this.btnBrowseStylesFiles.UseVisualStyleBackColor = true;
            this.btnBrowseStylesFiles.Click += new System.EventHandler(this.btnBrowseStylesFiles_Click);
            // 
            // errGenerateAssembly
            // 
            this.errGenerateAssembly.ContainerControl = this;
            // 
            // txtLogger
            // 
            this.txtLogger.CausesValidation = false;
            this.txtLogger.Location = new System.Drawing.Point(105, 310);
            this.txtLogger.Multiline = true;
            this.txtLogger.Name = "txtLogger";
            this.txtLogger.ReadOnly = true;
            this.txtLogger.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLogger.Size = new System.Drawing.Size(722, 127);
            this.txtLogger.TabIndex = 20;
            this.txtLogger.TabStop = false;
            // 
            // stsMain
            // 
            this.stsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslStatus});
            this.stsMain.Location = new System.Drawing.Point(0, 440);
            this.stsMain.Name = "stsMain";
            this.stsMain.Size = new System.Drawing.Size(839, 22);
            this.stsMain.SizingGrip = false;
            this.stsMain.TabIndex = 21;
            // 
            // tslStatus
            // 
            this.tslStatus.AutoSize = false;
            this.tslStatus.Name = "tslStatus";
            this.tslStatus.Size = new System.Drawing.Size(600, 17);
            this.tslStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.CausesValidation = false;
            this.label3.Location = new System.Drawing.Point(13, 310);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 20);
            this.label3.TabIndex = 19;
            this.label3.Text = "Logging:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnAbout
            // 
            this.btnAbout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbout.Location = new System.Drawing.Point(762, 281);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(65, 23);
            this.btnAbout.TabIndex = 18;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // label4
            // 
            this.label4.CausesValidation = false;
            this.label4.Location = new System.Drawing.Point(432, 253);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "Assembly &version:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAssemblyVersion
            // 
            this.txtAssemblyVersion.Location = new System.Drawing.Point(549, 252);
            this.txtAssemblyVersion.Name = "txtAssemblyVersion";
            this.txtAssemblyVersion.Size = new System.Drawing.Size(100, 20);
            this.txtAssemblyVersion.TabIndex = 13;
            this.txtAssemblyVersion.WordWrap = false;
            this.txtAssemblyVersion.Validating += new System.ComponentModel.CancelEventHandler(this.txtAssemblyVersion_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(655, 255);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "(x.x.x.x)";
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveSettings.Location = new System.Drawing.Point(549, 281);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(100, 23);
            this.btnSaveSettings.TabIndex = 16;
            this.btnSaveSettings.Text = "&Save Settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnLoadSettings
            // 
            this.btnLoadSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadSettings.Location = new System.Drawing.Point(655, 281);
            this.btnLoadSettings.Name = "btnLoadSettings";
            this.btnLoadSettings.Size = new System.Drawing.Size(101, 23);
            this.btnLoadSettings.TabIndex = 17;
            this.btnLoadSettings.Text = "&Load Settings";
            this.btnLoadSettings.UseVisualStyleBackColor = true;
            this.btnLoadSettings.Click += new System.EventHandler(this.btnLoadSettings_Click);
            // 
            // ofdSettings
            // 
            this.ofdSettings.FileName = "ApplicationSettings.xml";
            this.ofdSettings.Filter = "Settings Files|*.xml";
            // 
            // sfdSettings
            // 
            this.sfdSettings.Filter = "Settings Files|*.xml";
            // 
            // label6
            // 
            this.label6.CausesValidation = false;
            this.label6.Location = new System.Drawing.Point(13, 229);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 20);
            this.label6.TabIndex = 7;
            this.label6.Text = "&Telerik Dll path:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnBrowseTelerikDllDirectory
            // 
            this.btnBrowseTelerikDllDirectory.CausesValidation = false;
            this.btnBrowseTelerikDllDirectory.Location = new System.Drawing.Point(798, 228);
            this.btnBrowseTelerikDllDirectory.Name = "btnBrowseTelerikDllDirectory";
            this.btnBrowseTelerikDllDirectory.Size = new System.Drawing.Size(29, 23);
            this.btnBrowseTelerikDllDirectory.TabIndex = 9;
            this.btnBrowseTelerikDllDirectory.Text = "...";
            this.btnBrowseTelerikDllDirectory.UseVisualStyleBackColor = true;
            this.btnBrowseTelerikDllDirectory.Click += new System.EventHandler(this.btnBrowseTelerikDllDirectory_Click);
            // 
            // txtBrowseTelerikDllDirectory
            // 
            this.txtBrowseTelerikDllDirectory.Location = new System.Drawing.Point(105, 229);
            this.txtBrowseTelerikDllDirectory.Name = "txtBrowseTelerikDllDirectory";
            this.txtBrowseTelerikDllDirectory.Size = new System.Drawing.Size(697, 20);
            this.txtBrowseTelerikDllDirectory.TabIndex = 8;
            // 
            // btnClearLogging
            // 
            this.btnClearLogging.Location = new System.Drawing.Point(249, 280);
            this.btnClearLogging.Name = "btnClearLogging";
            this.btnClearLogging.Size = new System.Drawing.Size(91, 23);
            this.btnClearLogging.TabIndex = 22;
            this.btnClearLogging.Text = "Clear Logging";
            this.btnClearLogging.UseVisualStyleBackColor = true;
            this.btnClearLogging.Click += new System.EventHandler(this.btnClearLogging_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ClientSize = new System.Drawing.Size(839, 462);
            this.Controls.Add(this.btnClearLogging);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnBrowseTelerikDllDirectory);
            this.Controls.Add(this.txtBrowseTelerikDllDirectory);
            this.Controls.Add(this.btnLoadSettings);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtAssemblyVersion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.stsMain);
            this.Controls.Add(this.txtLogger);
            this.Controls.Add(this.btnBrowseStylesFiles);
            this.Controls.Add(this.btnGenerateAssembly);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAssemblyName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBrowseOutputDirectory);
            this.Controls.Add(this.txtBrowseOutputDirectory);
            this.Controls.Add(this.btnDeleteStylesFiles);
            this.Controls.Add(this.lblStyles);
            this.Controls.Add(this.lbxStylesFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.errGenerateAssembly)).EndInit();
            this.stsMain.ResumeLayout(false);
            this.stsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbxStylesFiles;
        private System.Windows.Forms.Label lblStyles;
        private System.Windows.Forms.Button btnDeleteStylesFiles;
        private System.Windows.Forms.Button btnBrowseOutputDirectory;
        private System.Windows.Forms.TextBox txtBrowseOutputDirectory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAssemblyName;
        private System.Windows.Forms.Button btnGenerateAssembly;
        private System.Windows.Forms.OpenFileDialog ofdStylesFiles;
        private System.Windows.Forms.Button btnBrowseStylesFiles;
        private System.Windows.Forms.FolderBrowserDialog fbdOutputDirectory;
        private System.Windows.Forms.ErrorProvider errGenerateAssembly;
        private System.Windows.Forms.TextBox txtLogger;
        private System.Windows.Forms.StatusStrip stsMain;
        private System.Windows.Forms.ToolStripStatusLabel tslStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAssemblyVersion;
        private System.Windows.Forms.Button btnLoadSettings;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.OpenFileDialog ofdSettings;
        private System.Windows.Forms.SaveFileDialog sfdSettings;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnBrowseTelerikDllDirectory;
        private System.Windows.Forms.TextBox txtBrowseTelerikDllDirectory;
        private System.Windows.Forms.FolderBrowserDialog fbdTelerikDllDirectory;
        private System.Windows.Forms.Button btnClearLogging;

    }
}


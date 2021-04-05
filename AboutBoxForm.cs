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
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace acTelerikStylesAssembly
{
    partial class AboutBoxForm : Form
    {
        public AboutBoxForm()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", Program.Application.AssemblyTitle);
            this.lblProductName.Text = Program.Application.AssemblyProduct;
            this.lblVersion.Text = String.Format("Version {0} ({1})", Program.Application.AssemblyVersion, Program.Application.AssemblyDate);
            this.lblCopyright.Text = Program.Application.AssemblyCopyright;
            this.lblCompanyName.Text = Program.Application.AssemblyCompany;
            this.txtDescription.Text = Program.Application.AssemblyDescription;
        }
    }
}

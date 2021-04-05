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
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace acTelerikStylesAssembly
{
    static class Program
    {
        private static acTelerikStylesAssemblyApplication _acTelerikStylesAssemblyApplication = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] nArguments)
        {
            try
            {
                _acTelerikStylesAssemblyApplication = new acTelerikStylesAssemblyApplication();
                _acTelerikStylesAssemblyApplication.Initialize();

                // Test if it is running from console or GUI
                if (Console.OpenStandardInput(1) == Stream.Null && nArguments.Length == 0)
                {
                    _acTelerikStylesAssemblyApplication.RunFormApp();

                    return 0;
                } 
                else
                    return _acTelerikStylesAssemblyApplication.RunConsoleApp(nArguments);
            }
            catch (Exception)
            {
                return 5;
            }
            finally
            {
                try
                {
                    if (_acTelerikStylesAssemblyApplication != null)
                        _acTelerikStylesAssemblyApplication.Dispose();
                }
                catch
                {
                }
            }            
        }

        public static acTelerikStylesAssemblyApplication Application
        {
            get { return _acTelerikStylesAssemblyApplication; }
        }
    }
}

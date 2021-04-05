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
using System.Text.RegularExpressions;
using System.Collections;

namespace acTelerikStylesAssembly
{
    public static class ProcessingArgumentsUtilities
    {
        public static bool ValidateAssemblyName(string sAssemblyName)
        {
            return !string.IsNullOrEmpty(sAssemblyName);
        }

        public static bool ValidateAssemblyVersion(string sAssemblyVersion)
        {
            Regex nValidation = new Regex(@"(?<major>\d*)\.(?<minor>\d*)(\.(?<build>\d*)(\.(?<revision>\d*))?)?");

            return string.IsNullOrEmpty(sAssemblyVersion) || (!string.IsNullOrEmpty(sAssemblyVersion) && nValidation.IsMatch(sAssemblyVersion));
        }

        public static bool ValidateBrowseOutputDirectory(string sBrowseOutputDirectory)
        {
            return !string.IsNullOrEmpty(sBrowseOutputDirectory);
        }

        public static bool ValidateStylesFiles(IList nStylesFiles)
        {
            return nStylesFiles != null && nStylesFiles.Count > 0 && nStylesFiles.Cast<string>().All(file => file.EndsWith(".zip"));
        }
    }
}

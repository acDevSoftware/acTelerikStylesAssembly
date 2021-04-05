using System;
using System.Collections.Generic;
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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace acTelerikStylesAssembly
{
    public class ConsoleArguments
    {
        private Regex m_nRegularExpressionArguments = new Regex(@"(-{1,2}|/)((?<key>\w{1})|(?<key>\?))([:=]?((['""](?<value>.*?)['""])|(?<value>\S+)))?");

        public MultiValueDictionary<string, string> ParseArguments(string[] nArguments)
        {
            MatchCollection nMatches = m_nRegularExpressionArguments.Matches(Environment.CommandLine);

            return nMatches.Cast<Match>().ToMultiValueDictionary(nMatch => nMatch.Groups["key"].Value, nMatch => nMatch.Groups["value"].Success ? nMatch.Groups["value"].Value : null);
        }
    }
}

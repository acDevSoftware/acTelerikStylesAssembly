using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace acTelerikStylesAssembly
{
    public class ProcessingArguments
    {
        public string AssemblyName { get; set; }
        public string AssemblyVersion { get; set; }
        public List<string> StylesFiles { get; set; }
        public string BrowseOutputDirectory { get; set; }
        public string BrowseTelerikDllDirectory { get; set; }
        public bool Verbose { get; set; }
    }
}

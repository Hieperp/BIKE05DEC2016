using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCClient.ViewModels.Menus
{
    public class ModuleViewModel
    {
        public int ModuleID { get; set; }
        public string Description { get; set; }
        public string DescriptionEN { get; set; }
        public int SerialID { get; set; }
        public string ImageIndex { get; set; }
        public int InActive { get; set; }
    }
}
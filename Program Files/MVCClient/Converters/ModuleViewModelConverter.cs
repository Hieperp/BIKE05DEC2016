using MVCClient.ViewModels.Menus;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCClient.Converters
{
    public class ModuleViewModelConverter : IConverter<ModuleViewModel, Module>
    {
        public ModuleViewModel Convert(Module module)
        {
            return new ModuleViewModel
            {
                ModuleID = module.ModuleID,
                Description = module.Description,
                DescriptionEN = module.DescriptionEN,
                SerialID = module.SerialID,
                ImageIndex = module.ImageIndex,
                InActive = module.InActive
            };
        }
    }
}
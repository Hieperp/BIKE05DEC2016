using MVCClient.ViewModels.Menus;
using MVCModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCClient.Converters
{
    public class ModuleDetailViewModelConverter : IConverter<ModuleDetailViewModel, ModuleDetail>
    {
        public ModuleDetailViewModel Convert(ModuleDetail moduleDetail)
        {
            return new ModuleDetailViewModel
            {
                TaskID = moduleDetail.TaskID,
                ModuleID = moduleDetail.ModuleID,
                SoftDescription = moduleDetail.SoftDescription,
                Description = moduleDetail.Description,
                DescriptionEN = moduleDetail.DescriptionEN,
                Actions = moduleDetail.Actions,
                Controller = moduleDetail.Controller,
                LastOpen = moduleDetail.LastOpen,
                SerialID = moduleDetail.SerialID,
                ImageIndex = moduleDetail.ImageIndex,
                InActive = moduleDetail.InActive
            };
        }
    }
}
using System.Web.Mvc;
using System.Collections.Generic;

using MVCModel.Models;
using MVCClient.ViewModels.StockTasks;

namespace MVCClient.Builders.StockTasks
{
    public interface IGoodsReceiptViewModelSelectListBuilder : IViewModelSelectListBuilder<GoodsReceiptViewModel> 
    {
    }
}
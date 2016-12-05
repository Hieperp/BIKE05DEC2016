using MVCClient.ViewModels.Helpers;

namespace MVCClient.Builders
{
    public interface IViewModelSelectListBuilder<TBaseViewModel>
        where TBaseViewModel: ISimpleViewModel
    {
        void BuildSelectLists(TBaseViewModel baseViewModel);
    }
}

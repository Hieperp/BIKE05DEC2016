using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Mvc.Html;

namespace MVCClient
{
    public static class EnumExtensions
    {
        public static string EnumToJson(this Type type)
        {
            if (!type.IsEnum)
                throw new InvalidOperationException("Enum expected");

            var dictionary = Enum.GetValues(type).Cast<object>().ToDictionary(enumValue => enumValue.ToString(), enumValue => (int)enumValue);

            return new JavaScriptSerializer().Serialize(dictionary);
        }
    }

    public static class GridBoundColumnBuilderHelpers
    {
        public static GridBoundColumnBuilder<TModel> DisplayNameTitle<TModel>(this GridBoundColumnBuilder<TModel> builder) where TModel : class, new()
        {
            // Create an adapter to access the typed grid column (which contains the Expression)
            Type adapterType = typeof(GridBoundColumnAdapter<,>).MakeGenericType(typeof(TModel), builder.Column.MemberType);
            IGridBoundColumnAdapter adapter = (IGridBoundColumnAdapter)Activator.CreateInstance(adapterType);

            // Use the adapter to get the title and set it
            return builder.Title(adapter.GetDisplayName(builder.Column));
        }

        private interface IGridBoundColumnAdapter
        {
            string GetDisplayName(IGridBoundColumn column);
        }

        private class GridBoundColumnAdapter<TModel, TValue> : IGridBoundColumnAdapter where TModel : class, new()
        {
            public string GetDisplayName(IGridBoundColumn column)
            {
                // Get the typed bound column
                GridBoundColumn<TModel, TValue> boundColumn = column as GridBoundColumn<TModel, TValue>;
                if (boundColumn == null) return String.Empty;

                // Create the appropriate HtmlHelper and use it to get the display name
                HtmlHelper<TModel> helper = HtmlHelpers.For<TModel>(boundColumn.Grid.ViewContext, boundColumn.Grid.ViewData, new RouteCollection());
                return helper.DisplayNameFor(boundColumn.Expression).ToString();
            }
        }
    }


    public static class HtmlHelpers
    {
        public static HtmlHelper<TModel> For<TModel>(this HtmlHelper helper) where TModel : class, new()
        {
            return For<TModel>(helper.ViewContext, helper.ViewDataContainer.ViewData, helper.RouteCollection);
        }

        public static HtmlHelper<TModel> For<TModel>(this HtmlHelper helper, TModel model)
        {
            return For<TModel>(helper.ViewContext, helper.ViewDataContainer.ViewData, helper.RouteCollection, model);
        }

        public static HtmlHelper<TModel> For<TModel>(ViewContext viewContext, ViewDataDictionary viewData, RouteCollection routeCollection) where TModel : class, new()
        {
            TModel model = new TModel();
            return For<TModel>(viewContext, viewData, routeCollection, model);
        }

        public static HtmlHelper<TModel> For<TModel>(ViewContext viewContext, ViewDataDictionary viewData, RouteCollection routeCollection, TModel model)
        {
            var newViewData = new ViewDataDictionary(viewData) { Model = model };
            ViewContext newViewContext = new ViewContext(
                viewContext.Controller.ControllerContext,
                viewContext.View,
                newViewData,
                viewContext.TempData,
                viewContext.Writer);
            var viewDataContainer = new ViewDataContainer(newViewContext.ViewData);
            return new HtmlHelper<TModel>(newViewContext, viewDataContainer, routeCollection);
        }

        private class ViewDataContainer : System.Web.Mvc.IViewDataContainer
        {
            public System.Web.Mvc.ViewDataDictionary ViewData { get; set; }

            public ViewDataContainer(System.Web.Mvc.ViewDataDictionary viewData)
            {
                ViewData = viewData;
            }
        }
    }


}
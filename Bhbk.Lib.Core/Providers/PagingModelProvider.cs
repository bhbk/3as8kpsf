using Bhbk.Lib.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Bhbk.Lib.Core.Providers
{
    //https://www.dotnetcurry.com/aspnet-mvc/1368/aspnet-core-mvc-custom-model-binding
    public class PagingModelBinder : IModelBinder
    {
        private readonly IModelBinder _binder;
        private string _errorMsg = string.Empty;
        private string _orderBy = "orderBy";
        private string _pageSize = "pageSize";
        private string _pageNumber = "pageNumber";

        public PagingModelBinder() { }

        public PagingModelBinder(IModelBinder binder)
        {
            if (binder == null)
                throw new ArgumentNullException(nameof(binder));

            _binder = binder;
        }

        public Task BindModelAsync(ModelBindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var orderBy = context.ValueProvider.GetValue(_orderBy);

            if (orderBy == ValueProviderResult.None)
                context.ModelState.AddModelError(_orderBy, _errorMsg);

            var pageSize = context.ValueProvider.GetValue(_pageSize);

            if (pageSize == ValueProviderResult.None)
                context.ModelState.AddModelError(_pageSize, _errorMsg);

            var pageNumber = context.ValueProvider.GetValue(_pageNumber);

            if (pageNumber == ValueProviderResult.None)
                context.ModelState.AddModelError(_pageNumber, _errorMsg);

            if (context.ModelState.ErrorCount == 0)
                context.Result = ModelBindingResult.Success(
                    new PagingModel(orderBy.FirstValue, Convert.ToInt32(pageSize.FirstValue), Convert.ToInt32(pageNumber.FirstValue)));

            else if (context.ModelState.ErrorCount > 0)
                context.Result = ModelBindingResult.Failed();

            return Task.CompletedTask;
        }
    }

    public class ModelBinderProvider : IModelBinderProvider
    {
        private readonly IModelBinderProvider _binder;

        public ModelBinderProvider() { }

        public ModelBinderProvider(IModelBinderProvider binder)
        {
            if (binder == null)
                throw new ArgumentNullException(nameof(binder));

            _binder = binder;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.IsComplexType && context.Metadata.ModelType == typeof(PagingModel))
                return new PagingModelBinder();

            //if (context.Metadata.IsComplexType && context.Metadata.ModelType == typeof(CustomPagingModel))
            //    return new CustomPagingModelBinder(new ComplexTypeModelBinder(context.Metadata.Properties.ToDictionary(x => x, context.CreateBinder)));

            return null;
        }
    }

    public static class MvcOptionsExtensions
    {
        public static void UseMyModelBinderProvider(this MvcOptions options)
        {
            var binder = options.ModelBinderProviders.FirstOrDefault(x => x.GetType() == typeof(ComplexTypeModelBinderProvider));

            if (binder == null)
                return;

            var index = options.ModelBinderProviders.IndexOf(binder);

            options.ModelBinderProviders.Insert(index, new ModelBinderProvider());
        }
    }
}

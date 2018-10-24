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
    public class PagingBinder : IModelBinder
    {
        private readonly IModelBinder _binder;
        private string _errorMsg = string.Empty;
        private string _orderBy = "orderBy";
        private string _skip = "skip";
        private string _take = "take";

        public PagingBinder() { }

        public PagingBinder(IModelBinder binder)
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

            var skip = context.ValueProvider.GetValue(_skip);

            if (skip == ValueProviderResult.None)
                context.ModelState.AddModelError(_skip, _errorMsg);

            var take = context.ValueProvider.GetValue(_take);

            if (take == ValueProviderResult.None)
                context.ModelState.AddModelError(_take, _errorMsg);

            if (context.ModelState.ErrorCount == 0)
                context.Result = ModelBindingResult.Success(
                    new Paging(orderBy.FirstValue, Convert.ToInt32(skip.FirstValue), Convert.ToInt32(take.FirstValue)));

            else if (context.ModelState.ErrorCount > 0)
                context.Result = ModelBindingResult.Failed();

            return Task.CompletedTask;
        }
    }

    public class PagingBinderProvider : IModelBinderProvider
    {
        private readonly IModelBinderProvider _binder;

        public PagingBinderProvider() { }

        public PagingBinderProvider(IModelBinderProvider binder)
        {
            if (binder == null)
                throw new ArgumentNullException(nameof(binder));

            _binder = binder;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.IsComplexType && context.Metadata.ModelType == typeof(Paging))
                return new PagingBinder();

            //if (context.Metadata.IsComplexType && context.Metadata.ModelType == typeof(Paging))
            //    return new PagingBinder(new ComplexTypeModelBinder(context.Metadata.Properties.ToDictionary(x => x, context.CreateBinder)));

            return null;
        }
    }

    public static class MvcOptionsExtensions
    {
        public static void UseBhbkPagingBinderProvider(this MvcOptions options)
        {
            var binder = options.ModelBinderProviders.FirstOrDefault(x => x.GetType() == typeof(ComplexTypeModelBinderProvider));

            if (binder == null)
                return;

            var index = options.ModelBinderProviders.IndexOf(binder);

            options.ModelBinderProviders.Insert(index, new PagingBinderProvider());
        }
    }
}

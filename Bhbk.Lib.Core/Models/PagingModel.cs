using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.Core.Models
{
    //https://www.dotnetcurry.com/aspnet-mvc/1368/aspnet-core-mvc-custom-model-binding
    public class PagingModel
    {
        private int _pageSizeMax = 100;

        [Required]
        public string OrderBy { get; private set; }

        [Required]
        public int? PageNumber { get; private set; }

        [Required]
        public int? PageSize { get; private set; }

        public PagingModel(string orderBy, int pageSize, int pageNumber)
        {
            PageNumber = pageNumber;

            if (PageSize > _pageSizeMax)
                PageSize = _pageSizeMax;
            else
                PageSize = pageSize;

            OrderBy = orderBy;
        }
    }
}

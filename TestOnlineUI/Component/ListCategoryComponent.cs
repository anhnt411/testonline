using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineBusiness.Interface;


namespace TestOnlineUI.Component
{
    public class ListCategoryComponent:ViewComponent
    {
        private readonly ICategoryDomain _category;
        private readonly ILogger<ListCategoryComponent> _logger;
        public ListCategoryComponent(ICategoryDomain category,ILogger<ListCategoryComponent> logger)
        {
            this._category = category;
            this._logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync(FilterModel model)
        {
            try
            {
                

                return View();
            }
            catch (Exception)
            {
                return View("Error.cshtml");
            }
            
        }
    }
}

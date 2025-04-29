using AskIt.Models.ViewModels.ForumViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AskIt.ViewComponents.ForumViewComponents
{
    public class PaginationViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int currentPage, int totalPages)
        {
            var model = new PaginationViewModel
            {
                CurrentPage = currentPage,
                TotalPages = totalPages
            };

            return View(model);
        }
    }
}

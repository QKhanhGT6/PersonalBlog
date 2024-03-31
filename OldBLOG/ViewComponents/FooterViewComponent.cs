using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OldBLOG.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        // Look for a ViewComponent names Footer
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.Factory.StartNew(() => { return View(); });               
        }
    }
}

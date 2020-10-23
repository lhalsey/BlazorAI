using BlazorAI.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorAI.Client.Pages
{
    public class PageBase : ComponentBase
    {
        [Inject]
        protected ISolverService SolverService { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        public async Task ScrollToTop()
        {
            await JSRuntime.InvokeVoidAsync("scrollToElement", "solver-anchor");
        }
    }
}
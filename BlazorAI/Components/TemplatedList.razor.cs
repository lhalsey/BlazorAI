using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorAI.Client.Components
{
    public partial class TemplatedList<TItem>
    {
        [Parameter]
        public IEnumerable<TItem> Items { get; set; }

        [Parameter]
        public RenderFragment<TItem> ChildContent { get; set; }
    }
}

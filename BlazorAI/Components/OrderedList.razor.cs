using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorAI.Client.Components
{
    public partial class OrderedList<TItem>
    {
        [Parameter]
        public IEnumerable<TItem> Items { get; set; }
    }
}

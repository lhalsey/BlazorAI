using Microsoft.AspNetCore.Components;

namespace BlazorAI.Client.ProblemComponents
{
    public partial class GeniusSquareGrid
    {
        [Parameter]
        public int[] Values { get; set; }
    }
}

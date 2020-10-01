using Microsoft.AspNetCore.Components;

namespace BlazorAI.Client.ProblemComponents
{
    public partial class PasswordGrid
    {
        [Parameter]
        public string Solution { get; set; }

        [Parameter]
        public int RowLength { get; set; }
    }
}

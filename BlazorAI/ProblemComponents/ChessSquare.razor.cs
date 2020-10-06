using Microsoft.AspNetCore.Components;

namespace BlazorAI.Client.ProblemComponents
{
    public partial class ChessSquare
    {
        [Parameter]
        public int Row { get; set; }

        [Parameter]
        public int Column { get; set; }

        [Parameter]
        public bool HasQueen { get; set; }

        [Parameter]
        public bool IsInvalid { get; set; }

        private string FillColour => (Row + Column) % 2 == 0 ? "#cccccc" : "white";

        private string ShadowColour => (Row + Column) % 2 == 0 ? "url(#black-shadow)" : "url(#white-shadow)";
    }
}

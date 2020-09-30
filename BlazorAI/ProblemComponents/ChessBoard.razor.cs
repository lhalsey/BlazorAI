using BlazorAI.Shared.Solvers;
using Microsoft.AspNetCore.Components;

namespace BlazorAI.Client.ProblemComponents
{
    public partial class ChessBoard
    {
        [Parameter]
        public Queen[] Queens { get; set; }

        public int NumQueens => Queens.Length;
    }
}

using BlazorAI.Shared.Types;

namespace BlazorAI.Shared.Solvers
{
    public class Queen
    {
        public Point Location { get; set; }
        public bool IsInvalid { get; set; }
    }

    public class EightQueensSolution
    {
        public Queen[] Queens { get; set; }
    }
}

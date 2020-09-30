using BlazorAI.Shared.Solvers;
using System;

namespace BlazorAI.Shared.Types
{
    public class Rule
    {
        public string Description { get; set; }

        public Func<FiveHousesSolution, int> Score { get; set; }
    }
}

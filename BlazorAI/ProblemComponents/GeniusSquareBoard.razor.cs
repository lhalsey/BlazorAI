using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAI.Client.ProblemComponents
{
    public partial class GeniusSquareBoard
    {
        [Parameter]
        public int[] Values { get; set; }

        [Parameter]
        public string[] Dice { get; set; }

        public int[] MissingValues1 =>
            missingValues1
            .Select(x => UniqueValues.Contains(x) ? 0 : x)
            .ToArray();

        public int[] MissingValues2 =>
            missingValues2
            .Select(x => UniqueValues.Contains(x) ? 0 : x)
            .ToArray();

        private HashSet<int> UniqueValues => Values.Distinct().ToHashSet();

        // Show shapes that have not been placed in two columns
        // Please excuse the magic numbers :)
        private int[] missingValues1 =
           new int[]
           { 0, 0,
             6, 6, 6, 0, 6, 0, 0, 0, // Big L
             8, 8, 8, 0, 0, 0,       // Small L
             5, 0, 5, 5, 5, 0, 0, 0, // T
             0, 4, 4, 4, 4, 0,       // S
             0, 0, 0, 0, 0, 0 };

        private int[] missingValues2 =
            new int[]
            { 0, 0,
              3, 0, 3, 0, 3, 0, 3, 0, 0, 0, // Line 4
              7, 0, 7, 0, 7, 0, 0, 0,       // Line 3
              9, 0, 9, 0, 0, 0,             // Line 2
              2, 2, 2, 2, 0, 0,             // Square
              10, 0, 0, 0 };                // Single
    }
}

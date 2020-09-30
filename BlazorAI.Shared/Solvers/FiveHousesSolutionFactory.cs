using System.Collections.Generic;
using System.Linq;

namespace BlazorAI.Shared.Solvers
{
    public class FiveHousesSolutionFactory
    {
        public FiveHousesSolutionFactory()
        {
            AllTraits = new List<Trait[]>
            {
                new[] { Trait.Red, Trait.Green, Trait.Ivory, Trait.Yellow, Trait.Blue },
                new[] { Trait.Zebra, Trait.Dog, Trait.HedgeHog, Trait.Fox, Trait.Horse },
                new[] { Trait.Porsche, Trait.Chevy, Trait.Toyota, Trait.Ford, Trait.Subaru },
                new[] { Trait.Coffee, Trait.Tea, Trait.OJ, Trait.Milk, Trait.Chocolate },
                new[] { Trait.Spanish, Trait.English, Trait.Croatian, Trait.Japanese, Trait.Norwegian }
            };
        }

        List<Trait[]> AllTraits { get; }

        public FiveHousesSolution Create(List<int[]> traitGroups)
        {
            var traits =
                traitGroups
                .Zip(AllTraits, (indexes, traits) => indexes.Select(i => traits[i]))
                .SelectMany(x => x)
                .ToList();

            return new FiveHousesSolution
            {
                Traits = traits
            };
        }
    }
}

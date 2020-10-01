using BlazorAI.Shared.Utility;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAI.Shared.Solvers
{
    public class FiveHousesSolution
    {
        const int NumHouses = 5;

        public List<Trait> Traits { get; set; }

        public string[] Rules { get; set; }

        public int Matches(Trait trait1, Trait trait2)
        {
            var index1 = Traits.IndexOf(trait1) % NumHouses;
            var index2 = Traits.IndexOf(trait2) % NumHouses;

            return Math.Abs(index1 - index2);
        }

        public int HasIndex(Trait trait1, int index)
        {
            var index1 = Traits.IndexOf(trait1) % NumHouses;

            return Math.Abs(index1 - index);
        }

        public int IsRightOf(Trait trait1, Trait trait2)
        {
            var index1 = Traits.IndexOf(trait1) % NumHouses;
            var index2 = Traits.IndexOf(trait2) % NumHouses;

            return Math.Abs(index1 - index2 - 1);
        }

        public int IsNextTo(Trait trait1, Trait trait2)
        {
            var index1 = Traits.IndexOf(trait1) % NumHouses;
            var index2 = Traits.IndexOf(trait2) % NumHouses;

            return Math.Abs(Math.Abs(index1 - index2) - 1);
        }

        public List<Trait> GetHouse(int index) => Traits.Skip(index).TakeEvery(5).ToList();

        public IEnumerable<List<Trait>> Houses => 0.To(NumHouses - 1).Select(GetHouse);
    }

    public enum Trait
    {
        Red, Green, Ivory, Yellow, Blue,
        Zebra, Dog, HedgeHog, Horse, Fox,
        Porsche, Chevy, Ford, Toyota, Subaru,
        Coffee, Tea, OJ, Milk, Chocolate,
        English, Spanish, Croatian, Norwegian, Japanese
    };
}

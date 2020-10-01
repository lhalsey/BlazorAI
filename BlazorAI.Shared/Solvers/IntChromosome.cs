using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;
using MoreLinq;
using System;
using System.Linq;

namespace BlazorAI.Shared.Solvers
{
    /// <summary>
    /// Chromsome consisting of integer genes
    /// </summary>
    public class IntChromosome : ChromosomeBase
    {
        public IntChromosome(int[] inputGenes) : base(inputGenes.Length)
        {
            InputGenes = inputGenes;

            var genes = inputGenes.Shuffle().Select(x => new Gene(x)).ToArray();

            ReplaceGenes(0, genes);
        }

        int[] InputGenes { get; }

        public Func<int, int> GetGeneFromIndex { get; }

        public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(RandomizationProvider.Current.GetInt(0, InputGenes.Length));
        }

        public override IChromosome CreateNew() => new IntChromosome(InputGenes);
    }
}

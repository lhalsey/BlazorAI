using BlazorAI.Shared.Types;
using BlazorAI.Shared.Utility;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Randomizations;
using GeneticSharp.Domain.Selections;
using System;
using System.Collections.Generic;
using System.Linq;
using static MoreLinq.Extensions.ShuffleExtension;

namespace BlazorAI.Shared.Solvers
{
    public class FiveHousesSolver : Solver<FiveHousesSolution>
    {
        public FiveHousesSolver()
        {
            Rules = GetRules().ToArray();
            FitnessProvider = new FiveHousesFitness(Rules, new FiveHousesSolutionFactory());
        }

        FiveHousesFitness FitnessProvider { get; }

        protected override GeneticAlgorithm GetGA(SolverParameters parameters)
        {
            var population =
                new Population(parameters.Population, parameters.Population, new FiveHousesChromosome());

            return new GeneticAlgorithm(
                population,
                FitnessProvider,
                new TournamentSelection(),
                new UniformCrossover(),
                new TraitGroupMutation());
        }

        Rule[] Rules { get; set; }

        public IEnumerable<Rule> GetRules()
        {
            var rules = new List<Rule>
            {
                new Rule {
                    Description = "The Englishman lives in the red house. ",
                    Score = x => x.Matches(Trait.English, Trait.Red) },
                new Rule {
                    Description = "The Spaniard owns the dog.",
                    Score = x => x.Matches(Trait.Spanish, Trait.Dog) },
                new Rule {
                    Description = "Coffee is drunk in the green house.",
                    Score = x => x.Matches(Trait.Coffee, Trait.Green) },
                new Rule {
                    Description = "The Croatian drinks tea.",
                    Score = x => x.Matches(Trait.Croatian, Trait.Tea) },
                new Rule {
                    Description = "The green house is immediately to the right of the ivory house.",
                    Score = x => x.IsRightOf(Trait.Green, Trait.Ivory) },
                new Rule {
                    Description = "The Ford driver owns the hedgehog.",
                    Score = x => x.Matches(Trait.Ford, Trait.HedgeHog) },
                new Rule {
                    Description = "A Toyota driver lives in the yellow house.",
                    Score = x => x.Matches(Trait.Toyota, Trait.Yellow) },
                new Rule {
                    Description = "Milk is drunk in the middle house.",
                    Score = x => x.HasIndex(Trait.Milk, 2) },
                new Rule {
                    Description = "The Norwegian lives in the first house to the left.",
                    Score = x => x.HasIndex(Trait.Norwegian, 0) },
                new Rule {
                    Description = "The Chevy driver lives next to the man with the fox.",
                    Score = x => x.IsNextTo(Trait.Chevy, Trait.Fox) },
                new Rule {
                    Description = "A Toyota is parked next to the house where the horse is kept.",
                    Score = x => x.IsNextTo(Trait.Toyota, Trait.Horse) },
                new Rule {
                    Description = "The Subaru owner drinks orange juice.",
                    Score = x => x.Matches(Trait.Subaru, Trait.OJ) },
                new Rule {
                    Description = "The Japanese owns a Porsche.",
                    Score = x => x.Matches(Trait.Japanese, Trait.Porsche) },
                new Rule {
                    Description = "The Norwegian lives next to the blue house.",
                    Score = x => x.IsNextTo(Trait.Norwegian, Trait.Blue) }
            };

            return rules.ToArray();
        }

        protected override FiveHousesSolution GetSolution(IChromosome best)
        {
            var solution = FitnessProvider.GetSolution(best);

            solution.Rules =
                Rules
                .Select((x, i) => x.Score(solution) == 0 ? $"{x.Description} ✔️" : $"{x.Description} ❌")
                .ToArray();

            return solution;
        }
    }

    public class FiveHousesFitness : IFitness
    {
        public FiveHousesFitness(Rule[] rules, FiveHousesSolutionFactory factory)
        {
            Rules = rules;
            Factory = factory;
        }

        Rule[] Rules { get; set; }

        FiveHousesSolutionFactory Factory { get; set; }

        public FiveHousesSolution GetSolution(IChromosome chromosome)
        {
            var genes = chromosome.GetGenes();

            return Factory.Create(genes.Select(x => (int[])x.Value).ToList());
        }

        public int GetRulesScore(IChromosome chromosome)
        {
            var solution = GetSolution(chromosome);

            return Rules.Sum(x => x.Score(solution));
            //return Rules.Sum(x => x.Score(solution) > 0 ? 1 : 0);
        }

        public double Evaluate(IChromosome chromosome)
        {
            int score = GetRulesScore(chromosome);

            return 1.0 - ((double)score / (Rules.Length * 4));
            //return 1.0 - ((double)score / Rules.Length);
        }
    }

    public class FiveHousesChromosome : ChromosomeBase
    {
        const int NumTraits = 5;
        const int NumHouses = 5;

        public FiveHousesChromosome() : base(NumTraits)
        {
            CreateGenes();
        }

        public override Gene GenerateGene(int geneIndex)
        {
            var values = 0.To(NumHouses - 1).Shuffle().ToArray();

            return new Gene(values);
        }

        public override IChromosome CreateNew() => new FiveHousesChromosome();
    }

    public class TraitGroupMutation : MutationBase
    {
        const int NumHouses = 5;

        protected override void PerformMutate(IChromosome chromosome, float probability)
        {
            if (RandomizationProvider.Current.GetDouble() <= probability)
            {
                var genes = chromosome.GetGenes().ToList();

                var geneIndex = RandomizationProvider.Current.GetInt(0, genes.Count);

                // Reverse sequence
                var i1 = RandomizationProvider.Current.GetInt(0, NumHouses);
                var i2 = RandomizationProvider.Current.GetInt(0, NumHouses);

                var (m1, m2) = i1 < i2 ? (i1, i2) : (i2, i1);

                var sequenceLength = m2 - m1 + 1;

                var currVal = (int[])genes[geneIndex].Value;

                //var start = currVal.Take(m1);
                //var mid = currVal.Skip(m1).Take(sequenceLength).Reverse();
                //var end = currVal.Skip(m2 + 1);

                //var newVal = start.Concat(mid).Concat(end).ToArray();

                int[] newVal = new int[5];
                Array.Copy(currVal, newVal, 5);

                newVal[m1] = currVal[m2];
                newVal[m2] = currVal[m1];

                chromosome.ReplaceGene(geneIndex, new Gene(newVal));
            }
        }
    }
}

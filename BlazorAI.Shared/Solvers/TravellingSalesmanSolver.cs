using BlazorAI.Shared.Types;
using BlazorAI.Shared.Utility;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using System;
using System.Linq;

using static MoreLinq.Extensions.PairwiseExtension;

namespace BlazorAI.Shared.Solvers
{
    public class TravellingSalesmanSolver : Solver<TravellingSalesmanSolution>
    {
        public TravellingSalesmanSolver(Point[] inputPoints)
        {
            NumPoints = inputPoints.Length;
            FitnessProvider = new TravellingSalesmanFitness(inputPoints);
        }

        int NumPoints { get; }

        TravellingSalesmanFitness FitnessProvider { get; }

        protected override GeneticAlgorithm GetGA(SolverParameters parameters)
        {
            // First node is fixed
            var adamChromosome = new IntChromosome(1.To(NumPoints - 1).ToArray());

            var population =
                new Population(parameters.Population, parameters.Population, adamChromosome);

            return new GeneticAlgorithm(
                population,
                FitnessProvider,
                new TournamentSelection(),
                new OrderedCrossover(),
                new ReverseSequenceMutation());
        }

        protected override TravellingSalesmanSolution GetSolution(IChromosome best)
        {
            return new TravellingSalesmanSolution
            {
                Points = FitnessProvider.GetPoints(best),
                Distance = Math.Round(FitnessProvider.GetTotalDistance(best), 2)
            };
        }
    }

    public class TravellingSalesmanFitness : IFitness
    {
        public TravellingSalesmanFitness(Point[] inputPoints)
        {
            InputPoints = inputPoints;

            var cycle = inputPoints.Append(inputPoints[0]).ToArray();
            InitialDistance = GetTotalDistance(cycle);
        }

        Point[] InputPoints { get; }

        double InitialDistance { get; }

        // Add fixed start point to beginning and end and apply permutation
        public Point[] GetPoints(IChromosome chromosome) =>
            chromosome.GetGenes()
            .Select(x => (int)x.Value)
            .Prepend(0)
            .Append(0)
            .Select(x => InputPoints[x])
            .ToArray();

        // Minimise distance between consecutive points
        // First (and last as is loop) point is fixed
        public double GetTotalDistance(IChromosome chromosome) =>
            GetTotalDistance(GetPoints(chromosome));

        double GetTotalDistance(Point[] points) =>
            points.Pairwise((x, y) => x.DistanceTo(y)).Sum();

        public double Evaluate(IChromosome chromosome) =>
            Math.Max(0, 1.0 - (GetTotalDistance(chromosome) / InitialDistance));
    }
}

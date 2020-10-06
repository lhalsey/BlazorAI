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

namespace BlazorAI.Shared.Solvers
{
    public record Queen(Point Location, bool IsInvalid);

    public record EightQueensSolution(Queen[] Queens);

    /// <summary>
    /// Solver for https://en.wikipedia.org/wiki/Eight_queens_puzzle
    /// The traditional problem has an 8 x 8 board with 8 queens, but we
    /// generalise to N (between 4 and 16) queens.
    /// Staring solution has all the queens on the main diagonal to satisfy row
    /// and column conditions. We then evolve better solutions to resolve the
    /// diagonal constraints by reordering the columns that the queens occupy.
    /// </summary>
    public class EightQueensSolver : Solver<EightQueensSolution>
    {
        public EightQueensSolver(int numQueens)
        {
            NumQueens = numQueens;
            FitnessProvider = new EightQueensFitness();
        }

        int NumQueens { get; }

        EightQueensFitness FitnessProvider { get; }

        protected override GeneticAlgorithm GetGA(SolverParameters parameters)
        {
            var adamChromosome = new IntChromosome(0.To(NumQueens - 1).ToArray());

            var population =
                new Population(parameters.Population, parameters.Population, adamChromosome);

            return new GeneticAlgorithm(
                population,
                FitnessProvider,
                new EliteSelection(),
                new OrderedCrossover(),
                new ReverseSequenceMutation());
        }

        protected override EightQueensSolution GetSolution(IChromosome best) =>
            new EightQueensSolution(Queens: FitnessProvider.GetSolution(best));
    }

    /// <summary>
    /// How many of the queens are unthreatened
    /// </summary>
    public class EightQueensFitness : IFitness
    {
        bool SameDiagonal(Point x, Point y) => Math.Abs(x.X - y.X) == Math.Abs(x.Y - y.Y);

        public Queen[] GetSolution(IChromosome chromosome)
        {
            var points =
                chromosome.GetGenes()
                .Select((x, i) => new Point { X = i, Y = (int)x.Value })
                .ToList();

            return
                points
                .Select(x => new Queen(
                    Location: x,
                    IsInvalid: points.Count(y => SameDiagonal(x, y)) != 1))
                .ToArray();
        }
        
        public double Evaluate(IChromosome chromosome)
        {
            var queens = GetSolution(chromosome);

            int valid = queens.Count(x => !x.IsInvalid);

            return (double)valid / queens.Length;
        }
    }
}

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
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAI.Shared.Solvers
{
    public record GeniusSquareSolution(int[] CellValues);

    /// <summary>
    /// Solver for Genius Square board game, https://www.happypuzzle.co.uk/30cubed/genius-square
    /// There are nine shapes that need to be placed, but we only encode eight of these in our
    /// chromsome as the single unit square can be dropped in place when one empty square remains.
    /// Each shape is encoded with 3 integers representing row, column and orientation.
    /// <see cref="GeniusSquareSolutionFactory.shapes"/>
    /// </summary>
    public class GeniusSquareSolver : Solver<GeniusSquareSolution>
    {
        public GeniusSquareSolver(int[] blockers)
        {
            FitnessProvider = new GeniusSquareFitness(blockers);
        }

        public const int Shapes = 7; //8; // Don't include single unit square as we just place in the last empty space
        public const int GenesPerShape = 3; // Orientation, Column, Row
        public const int MaxValue = 360; // This number gives a good range and is divisible by many numbers

        GeniusSquareFitness FitnessProvider { get; }

        protected override GeneticAlgorithm GetGA(SolverParameters parameters)
        {
            var adamChromosome = new GeniusSquareChromosome();

            var population =
                new Population(parameters.Population, parameters.Population, adamChromosome);

            return new GeneticAlgorithm(
                population,
                FitnessProvider,
                new EliteSelection(),
                new UniformCrossover(),
                new IntMutation());
        }

        protected override GeniusSquareSolution GetSolution(IChromosome chromosome) =>
            new GeniusSquareSolution(CellValues: FitnessProvider.GetSolution(chromosome));
    }

    public class GeniusSquareFitness : IFitness
    {
        const int GridHeight = 6;
        const int GridWidth = 6;
        const int GridCells = GridHeight * GridWidth;
        
        const int GenesPerShape = 3; // Orientation, Column, Row
        const int ShapeStartIndex = 2; // Blocker is 1, shapes start from 2
        const int DoubleUnitShapeIndex = 9;
        const int SingleUnitShapeIndex = 10;

        public GeniusSquareFitness(int[] blockers)
        {
            SolutionFactory = new GeniusSquareSolutionFactory();

            // Array representing 6 x 6 grid with blockers that we can add shapes to
            EmptySolutionWithBlockers =
                0.To(GridCells - 1)
                .Select(x => blockers.Contains(x) ? 1 : 0);
        }

        GeniusSquareSolutionFactory SolutionFactory { get; }

        IEnumerable<int> EmptySolutionWithBlockers { get; }

        public int[] GetSolution(IChromosome chromosome)
        {
            var shapes =
                chromosome.GetGenes()
                .Select(x => (int)x.Value)
                .Batch(GenesPerShape, x => x.ToArray())
                .Index();

            // Determine which grid cells each shape would occupy if placed
            var shapeCells =
                shapes
                .Select(x => SolutionFactory.GetIndexes(
                    shapeId: x.Key,
                    shapeValue: x.Value[0],
                    xValue: x.Value[1],
                    yValue: x.Value[2]))
                .ToArray();

            int[] solution = EmptySolutionWithBlockers.ToArray();

            int placedShapes = 0;

            // Try to place each shape if no part of the shape would cover another (already-placed)
            // shape or blocker. We try to place the more "difficult" shapes first.
            // Note: Originally overlapping was allowed, but the UI looked a mess and performance
            // was no better than without overlaps
            for (int i = 0; i < shapeCells.Length; i++)
            {
                if (shapeCells[i].All(x => solution[x] == 0))
                {
                    placedShapes++;

                    foreach (int index in shapeCells[i])
                    {
                        solution[index] = i + ShapeStartIndex;
                    }
                }
            }

            bool IsAdjacent(int i1, int i2)
            {
                var (r1, c1) = (i1 / GridWidth, i1 % GridWidth);
                var (r2, c2) = (i2 / GridWidth, i2 % GridWidth);

                return (Math.Abs(r1 - r2) + Math.Abs(c1 - c2) == 1);
            }

            // The single and double unit shapes are the easiest to place
            // so handle these last. It's more efficient to find the gaps and
            // place them there than to wait for mutation to move them to the
            // correct place.
            if (placedShapes == GeniusSquareSolver.Shapes)
            {
                var empty =
                    solution
                    .Index()
                    .Where(x => x.Value == 0)
                    .Select(x => x.Key)
                    .ToList();

                var sol = empty.Permutations().FirstOrDefault(x => IsAdjacent(x[0], x[1]));

                if (sol != null)
                {
                    solution[sol[0]] = DoubleUnitShapeIndex;
                    solution[sol[1]] = DoubleUnitShapeIndex;
                    solution[sol[2]] = SingleUnitShapeIndex;
                }
            }

            return solution;
        }

        public double Evaluate(IChromosome chromosome)
        {
            const double PenaltyWeight = 0.25; // Trial & error

            int[] solution = GetSolution(chromosome);

            var emptyCells = solution.Index().Where(x => x.Value == 0).ToList();

            double GetDistanceFromCentre(int cellIndex)
            {
                const double MidPoint = 2.5; // (0 + 5) / 2

                int col = cellIndex % GridWidth;
                int row = cellIndex / GridWidth;

                return Math.Abs(col - MidPoint) + Math.Abs(row - MidPoint);
            }

            // Penalise empty cells the further they are from the centre
            // We have a greater chance of fitting a shape into the gaps if the gaps are close
            var penalty = emptyCells.Sum(x => GetDistanceFromCentre(x.Key));

            bool HasEmptyAdjacent(int index)
            {
                var (row, col) = (index / GridWidth, index % GridWidth);

                return
                    row > 0 && solution[index - GridWidth] == 0 || // Up
                    row < GridHeight - 1 && solution[index + GridWidth] == 0 || // Down
                    col > 0 && solution[index - 1] == 0 || // Left
                    col < GridWidth - 1 && solution[index + 1] == 0; // Right
            }

            // Penalise empty cells with no other empty cells surrounding them
            penalty += emptyCells.Count(x => !HasEmptyAdjacent(x.Key)) * 2;

            return Math.Max(0.0, 1.0 - ((emptyCells.Count() + penalty * PenaltyWeight) / GridCells));
        }
    }

    public class GeniusSquareChromosome : ChromosomeBase
    {
        const int ChromosomeSize = GeniusSquareSolver.Shapes * GeniusSquareSolver.GenesPerShape;

        public GeniusSquareChromosome() : base(ChromosomeSize) => CreateGenes();

        public override IChromosome CreateNew() => new GeniusSquareChromosome();

        public override Gene GenerateGene(int geneIndex) =>
            new Gene(RandomizationProvider.Current.GetInt(0, GeniusSquareSolver.MaxValue));
    }

    public class IntMutation : MutationBase
    {
        // Select random gene and increase/decrease it's value by a random amount
        // while ensuring it stays within the valid range of values
        protected override void PerformMutate(IChromosome chromosome, float probability)
        {
            const int NumMutations = 6; // Allow up to 25% of genes to be mutated (trial & error)

            for (int i = 0; i <= NumMutations; i++)
            {
                if (RandomizationProvider.Current.GetDouble() <= probability)
                {
                    // Select gene at random and change to random value in valid range
                    // Note: Previously mutation added or subtracted a random percentage
                    // of the range, but allowing mutation across the whole range seems
                    // to work better
                    var index = RandomizationProvider.Current.GetInt(0, chromosome.Length);

                    var newGene = RandomizationProvider.Current.GetInt(0, GeniusSquareSolver.MaxValue);

                    chromosome.ReplaceGene(index, new Gene(newGene));
                }
            }
        }
    }
}

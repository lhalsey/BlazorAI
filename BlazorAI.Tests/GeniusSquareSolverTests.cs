using BlazorAI.Shared.Solvers;
using BlazorAI.Shared.Types;
using FluentAssertions;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BlazorAI.Tests
{
    public class GeniusSquareSolverTests :
       SolverTests<GeniusSquareSolver, GeniusSquareSolution>
    {
        const int Square = 0;
        const int Line4 = 1;
        const int S_Shape = 2;
        const int T_Shape = 3;
        const int LongL = 4;
        const int Line3 = 5;
        const int ShortL = 6;
        const int Line2 = 7;

        const int Min = 0;
        const int Mid = 179;
        const int Max = 359;

        const int Horizontal = 0;
        const int Vertical = 180;

        const int Orientation_1_Of_4 = 0;
        const int Orientation_2_Of_4 = 90;
        const int Orientation_3_Of_4 = 180;
        const int Orientation_4_Of_4 = 270;

        const int Orientation_8_Of_8 = 315;

        protected override Solver<GeniusSquareSolution> GetSolver()
        {
            var blockers = new int[] { 0, 4, 5, 6, 16, 20, 29 }; // Seed 85

            return new GeniusSquareSolver(blockers);
        }

        protected override void AssertCrossover(GeneticAlgorithm ga)
        {
            ga.Crossover.Should().BeOfType<UniformCrossover>();
        }

        protected override void AssertMutation(GeneticAlgorithm ga)
        {
            ga.Mutation.Should().BeOfType<IntMutation>();
        }

        protected override void AssertResult(GeniusSquareSolution solution)
        {
            solution.CellValues.Should().HaveCount(36);
        }

        // Indexes for 6 x 6 grid
        // 00 01 02 03 04 05
        // 06 07 08 09 10 11
        // 12 13 14 15 16 17
        // 18 19 20 21 22 23
        // 24 25 26 27 28 29
        // 30 31 32 33 34 35
        // TODO: Exhaustive test cases for each shape & orientation if you can be bothered!
        // In the meantime Solver_TestGetIndexes_CompleteGridCoverage will suffice
        [Theory]
        [InlineData(Square, 0, Min, Min, new int[] { 0, 1, 6, 7 })]
        [InlineData(Square, 0, Mid, Mid, new int[] { 14, 15, 20, 21 })]
        [InlineData(Square, 0, Max, Max, new int[] { 28, 29, 34, 35 })]
        [InlineData(Line4, Horizontal, Min, Min, new int[] { 0, 1, 2, 3 })]
        [InlineData(Line4, Horizontal, Mid, Mid, new int[] { 13, 14, 15, 16 })]
        [InlineData(Line4, Horizontal, Max, Max, new int[] { 32, 33, 34, 35 })]
        [InlineData(Line4, Vertical, Min, Min, new int[] { 0, 6, 12, 18 })]
        [InlineData(Line4, Vertical, Mid, Mid, new int[] { 8, 14, 20, 26 })]
        [InlineData(Line4, Vertical, Max, Max, new int[] { 17, 23, 29, 35 })]
        [InlineData(S_Shape, Orientation_1_Of_4, Min, Min, new int[] { 0, 6, 7, 13 })]
        [InlineData(S_Shape, Orientation_1_Of_4, Mid, Mid, new int[] { 8, 14, 15, 21 })]
        [InlineData(S_Shape, Orientation_1_Of_4, Max, Max, new int[] { 22, 28, 29, 35 })]
        [InlineData(S_Shape, Orientation_2_Of_4, Min, Min, new int[] { 1, 2, 6, 7 })]
        [InlineData(S_Shape, Orientation_2_Of_4, Mid, Mid, new int[] { 14, 15, 19, 20 })]
        [InlineData(S_Shape, Orientation_2_Of_4, Max, Max, new int[] { 28, 29, 33, 34 })]
        [InlineData(S_Shape, Orientation_3_Of_4, Min, Min, new int[] { 1, 6, 7, 12 })]
        [InlineData(S_Shape, Orientation_3_Of_4, Mid, Mid, new int[] { 9, 14, 15, 20 })]
        [InlineData(S_Shape, Orientation_3_Of_4, Max, Max, new int[] { 23, 28, 29, 34 })]
        [InlineData(S_Shape, Orientation_4_Of_4, Min, Min, new int[] { 0, 1, 7, 8 })]
        [InlineData(S_Shape, Orientation_4_Of_4, Mid, Mid, new int[] { 13, 14, 20, 21 })]
        [InlineData(S_Shape, Orientation_4_Of_4, Max, Max, new int[] { 27, 28, 34, 35 })]
        public void Solver_TestGetIndexes_ShapesPositionedCorrectly
            (int shapeId, int shapeValue, int xValue, int yValue, int[] expected)
        {
            // Arrange
            var factory = new GeniusSquareSolutionFactory();

            // Act
            var indexes = factory.GetIndexes(shapeId, shapeValue, xValue, yValue);

            // Assert
            indexes.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(Square, 4)]
        [InlineData(Line4, 4)]
        [InlineData(S_Shape, 4)]
        [InlineData(T_Shape, 4)]
        [InlineData(LongL, 4)]
        [InlineData(Line3, 3)]
        [InlineData(ShortL, 3)]
        public void Solver_TestGetIndexes_CompleteGridCoverage(int shapeId, int shapeSize)
        {
            // Arrange
            var factory = new GeniusSquareSolutionFactory();

            int[] cells = new int[36];

            // Act
            // Try to test shape in all permutations to ensure it can cover each grid cell
            for (int o = 0; o < Max; o += 45) // 8 (potential) orientations
            {
                for (int x = 0; x < Max; x += 60) // 6 (potential) columns
                {
                    for (int y = 0; y < Max; y += 60) // 6 (potential) rows
                    {
                        var indexes = factory.GetIndexes(shapeId, shapeValue: o, xValue: x, yValue: y);

                        foreach (int i in indexes)
                        {
                            cells[i]++;
                        }
                    }
                }
            }

            // Assert
            int emptyCells = cells.Count(x => x == 0);

            emptyCells.Should().Be(0);

            cells.Sum().Should().Be(8 * 6 * 6 * shapeSize);
        }

        [Fact]
        public void Solver_EmptySolution_FitnessEvaluatesToZero()
        {
            // Arrange
            var (fitness, chromosome) = GetCorrectSolution();

            const int NumGenes = GeniusSquareSolver.GenesPerShape * GeniusSquareSolver.Shapes;

            // Locate all shapes on first cell which has blocker so all will be unplaced
            var genes = Enumerable.Repeat(0, NumGenes).Select(x => new Gene(x)).ToArray();

            chromosome.ReplaceGenes(0, genes);

            // Act
            double score = fitness.Evaluate(chromosome);

            // Assert
            score.Should().Be(0.0);
        }

        [Fact]
        public void Solver_CompleteSolution_FitnessEvaluatesToOne()
        {
            // Arrange
            var (fitness, chromosome) = GetCorrectSolution();

            // Act
            double score = fitness.Evaluate(chromosome);

            // Assert
            score.Should().Be(1.0);
        }

        [Fact]
        public void Solver_NearCompleteSolution_FitnessEvaluatesToGoodScore()
        {
            for (int i = 0; i < 100_000; i++)
            {
                // Arrange
                var (fitness, chromosome) = GetCorrectSolution();

                // Shift square to far left on top of blocker
                // This leaves 4 empty cells for the square and one for the single unit
                // shape which is no longer placed
                // The distance penalty sums to 11 and is multiplied by the penalty weight (0.5)
                // to give 1.0 - ((5 + 5.5) / 36)
                chromosome.ReplaceGene(1, new Gene(0));

                // Act
                double score = fitness.Evaluate(chromosome);

                // Assert
                score.Should().BeApproximately(0.7153, precision: 0.0001);
            }
        }

        [Fact(Skip = "Research only - long running")]
        public async Task Solver_SolvesEveryTime()
        {
            const int NumRuns = 100;
            string timestamp = DateTime.Now.ToLongTimeString().Replace(":", "_");

            string path = $"Output_{timestamp}.csv";

            for (int i = 0; i <= 255; i++)
            {
                var blockers = GetBlockers(i);
                var solver = new GeniusSquareSolver(blockers);

                var solves = await GetSolveRate(solver, NumRuns);

                string output = $"{i},{solves}\n";

                File.AppendAllText(path, output);
            }
        }

        private async Task<double> GetSolveRate(GeniusSquareSolver solver, int runs)
        {
            int solves = 0;

            // Arrange
            for (int i = 0; i < runs; i++)
            {
                var parameters = new SolverParameters
                {
                    Generations = 1_000,
                    Population = 100,
                    Selection = SolverParameters.SolverSelection.Elite,
                    CrossoverProbability = 0.5f,
                    MutationProbability = 0.5f
                };

                // Act
                var results = await solver.Solve(parameters).ToListAsync();

                // Assert
                double fitness = results.Last().Fitness;
                solves += fitness == 100.0 ? 1 : 0;
            }

            return (double)solves / runs;
        }


        private (GeniusSquareFitness, GeniusSquareChromosome) GetCorrectSolution()
        {
            // From random seed 85
            int[] blockers = new[] { 0, 4, 5, 6, 16, 20, 29 };

            var fitness = new GeniusSquareFitness(blockers);

            var chromosome = new GeniusSquareChromosome();

            // X 2 0 0 X X
            // X 2 0 0 6 6
            // 2 2 7 7 X 6
            // 5 3 X 8 2 2
            // 5 3 3 2 2 X
            // 5 3 1 1 1 1

            // Note: Last two shapes are placed automatically so do not form
            // part of the solution 
            var genes =
                new[]
                {
                    0, 144, 0, // 0 = Square
                    Horizontal, 240, 300, // 1 = Line4
                    Orientation_2_Of_4, 270, 216, // 2 = S_Shape
                    Orientation_1_Of_4, 72, 270, // 3 = T_Shape
                    Orientation_8_Of_8, 0, 0, // 4 = LongL
                    Vertical, 0, 270, // 5 = Line3
                    Orientation_2_Of_4, 288, 72, // 6 = ShortL
                    // Horizontal, 144, 120 // 7 = Line2
                    // 8 = Unit block
                }
                .Select(x => new Gene(x)).ToArray();

            chromosome.ReplaceGenes(0, genes);

            return (fitness, chromosome);
        }

        private int[] GetBlockers(int randomSeed)
        {
            const int GridSize = 6;
            var r = new Random(randomSeed);

            var diceValues =
                new string[]
                {
                    "A1 C1 D1 D2 E2 F3",
                    "A2 B2 C2 A3 B1 B3",
                    "C3 D3 E3 B4 C4 D4",
                    "E1 F2 F2 B6 A5 A5",
                    "A4 B5 C6 C5 D6 F6",
                    "E4 F4 E5 F5 D5 E6",
                    "F1 F1 F1 A6 A6 A6"
                };

            int GetCellIndex(string cellValue)
            {
                var row = cellValue[0] - 'A';
                var col = cellValue[1] - '1';

                return row * GridSize + col;
            }

            string RollDice(string values)
            {
                var roll = r.Next(0, GridSize);

                return values.Split(' ')[roll];
            }

            var dice = diceValues.Select(RollDice).OrderBy(x => x).ToArray();

            return dice.Select(GetCellIndex).ToArray();
        }
    }
}

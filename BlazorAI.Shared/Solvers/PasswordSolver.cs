using BlazorAI.Shared.Types;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Randomizations;
using GeneticSharp.Domain.Selections;
using System;
using System.Linq;

namespace BlazorAI.Shared.Solvers
{
    public record PasswordSolution(string Password);

    /// <summary>
    /// Solver for theoretical password system which (for some reason!)
    /// provides feedback on how close a password is and allows unlimited guesses.
    /// Chromsome is simply a string which has it's genes (chars) crossed over
    /// and mutated within the fixed character range.
    /// </summary>
    public class PasswordSolver : Solver<PasswordSolution>
    {
        // Allow characters from Space to Tilde which includes
        // numbers, upper & lower case letters & punctuation
        public const int CharLowerBound = 32; // ' '
        public const int CharUpperBound = 127; // '~'

        public PasswordSolver(string password)
        {
            PasswordLength = password.Length;
            FitnessProvider = new PasswordFitness(password);
        }

        int PasswordLength { get; }

        PasswordFitness FitnessProvider { get; }

        protected override GeneticAlgorithm GetGA(SolverParameters parameters)
        {
            var adamChromosome = new PasswordChromosome(PasswordLength);

            var population =
                new Population(parameters.Population, parameters.Population, adamChromosome);

            return new GeneticAlgorithm(
                population,
                FitnessProvider,
                new EliteSelection(),
                new UniformCrossover(),
                new StringMutation());
        }

        protected override PasswordSolution GetSolution(IChromosome chromosome) =>
           new PasswordSolution(Password: FitnessProvider.GetSolution(chromosome));
    }

    public class PasswordFitness : IFitness
    {
        public PasswordFitness(string password) => Password = password;

        string Password { get; }

        public string GetSolution(IChromosome chromosome) =>
            new string(chromosome.GetGenes().Select(x => (char)x.Value).ToArray());

        public double Evaluate(IChromosome chromosome)
        {
            var diff =
                chromosome.GetGenes()
                .Zip(Password, (x, y) => Math.Abs((char)x.Value - y))
                .Sum(x => x == 0 ? 0 : x + 10); // Reward exact match

            return Math.Max(0, 1.0 - (diff / (Password.Length * 50.0)));
        }
    }

    /// <summary>
    /// Char array to represent our guess
    /// This seems to perform way better than when using FloatingPointChromosome
    /// </summary>
    public class PasswordChromosome : ChromosomeBase
    {
        public PasswordChromosome(int passwordLength) : base(passwordLength)
        {
            PasswordLength = passwordLength;

            CreateGenes();
        }

        public int PasswordLength { get; set; }

        public override Gene GenerateGene(int geneIndex)
        {
            int index = RandomizationProvider.Current.GetInt(
                PasswordSolver.CharLowerBound, PasswordSolver.CharUpperBound);

            return new Gene((char)index);
        }

        public override IChromosome CreateNew() => new PasswordChromosome(PasswordLength);
    }

    public class StringMutation : MutationBase
    {
        // Select random char and increase/decrease it's (ASCII) value by a random amount
        // while ensuring it stays within the valid range of char values
        protected override void PerformMutate(IChromosome chromosome, float probability)
        {
            if (RandomizationProvider.Current.GetDouble() <= probability)
            {
                // ~10% of range from lower to upper seems to work well
                const int MaxMutationAmount = 10; 

                var index = RandomizationProvider.Current.GetInt(0, chromosome.Length);

                int currVal = (char)chromosome.GetGene(index).Value;

                var newGene = currVal +
                    RandomizationProvider.Current.GetInt(-MaxMutationAmount, MaxMutationAmount + 1);

                newGene = Math.Min(newGene, PasswordSolver.CharUpperBound);
                newGene = Math.Max(newGene, PasswordSolver.CharLowerBound);

                chromosome.ReplaceGene(index, new Gene((char)newGene));
            }
        }
    }
}

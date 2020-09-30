using System;

namespace BlazorAI.Shared.Types
{
    public class Result<T>
    {
        public Result() { }

        public Result(T solution, int generation, double fitness, TimeSpan timeEvolving)
        {
            Solution = solution;
            Generation = generation;
            Fitness = fitness;
            TimeEvolving = timeEvolving;
        }

        public T Solution { get; set; }
        public int Generation { get; set; }
        public double Fitness { get; set; }
        public TimeSpan TimeEvolving { get; set; }
    }
}

namespace BlazorAI.Shared.Types
{
    public class SolverParameters
    {
        public enum SolverSelection { Elite, Tournament, Roulette };

        public int Generations { get; set; }
        public int Population { get; set; }
        public SolverSelection Selection { get; set; }
        public float CrossoverProbability { get; set; }
        public float MutationProbability { get; set; }
    }
}

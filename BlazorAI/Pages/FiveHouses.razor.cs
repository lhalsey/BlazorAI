﻿using BlazorAI.Shared.Solvers;
using BlazorAI.Shared.Types;
using System.Collections.Generic;
using System.Threading;

namespace BlazorAI.Client.Pages
{
    public class FiveHousesBase : PageBase, ISolverPage<FiveHousesSolution>
    {
        public SolverParameters DefaultParameters =>
            new SolverParameters
            {
                Generations = 200,
                Population = 50,
                Selection = SolverParameters.SolverSelection.Tournament,
                CrossoverProbability = 0.8f,
                MutationProbability = 0.8f
            };

        public IAsyncEnumerable<Result<FiveHousesSolution>> GetResults(
            CancellationTokenSource token,
            SolverParameters parameters) =>
            SolverService.GetFiveHousesSolution(token, parameters);

        public void HandleResult(Result<FiveHousesSolution> result)
        {
            houses = result.Solution.Houses;
            rules = result.Solution.Rules;
        }

        protected string[] rules;
        protected IEnumerable<List<Trait>> houses;
    }
}

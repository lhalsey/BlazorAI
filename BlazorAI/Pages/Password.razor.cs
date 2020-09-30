using BlazorAI.Client.Components;
using BlazorAI.Client.Services;
using BlazorAI.Shared.Solvers;
using BlazorAI.Shared.Types;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorAI.Client.Pages
{
    public class PasswordBase : ComponentBase, ISolverPage<PasswordSolution>
    {
        [Inject]
        public ISolverService SolverService { get; set; }

        public SolverParameters DefaultParameters =>
            new SolverParameters
            {
                Generations = 200,
                Population = 100,
                Selection = SolverParameters.SolverSelection.Elite,
                CrossoverProbability = 0.8f,
                MutationProbability = 0.8f
            };

        public IAsyncEnumerable<Result<PasswordSolution>> GetResults(
            CancellationTokenSource token,
            SolverParameters parameters) =>
           SolverService.GetPasswordSolution(token, password, parameters);

        public void HandleResult(Result<PasswordSolution> result) =>
            solution = result.Solution.Password;

        protected SolverLayoutBase<PasswordSolution> solverControl;

        protected string solution;
        protected int problemNumSetting = 1;
        protected int rowLength;

        // TODO: Get more passwords and put in file
        protected (string, int)[] passwords = new[] {
            ("DHARMAInitiative", 0),
            ("YouHadMeAtPassword", 0),
            ("StrongLikeBull, SmartLikeTractor", 16),
            ("P@55w0rd123!", 0),
            ("FoundAUnicornNamedItFluffy", 13),
            ("TheCakeIsALie", 0),
            ("NeverGoingToGiveYouOutNeverGoingToWriteYouDownNeverGoingToRunAroundAndReuseYou", 13)
        };

        private string password;
        private int problemNum = 1;

        protected override void OnInitialized() => Initialise();

        protected async Task Update()
        {
            Initialise();

            await solverControl.Initialise();
        }
        private void Initialise()
        {
            problemNum = problemNumSetting;

            (password, rowLength) = passwords[problemNum - 1];
            rowLength = rowLength == 0 ? password.Length : rowLength;

            solution = new string('?', password.Length);
        }
    }
}

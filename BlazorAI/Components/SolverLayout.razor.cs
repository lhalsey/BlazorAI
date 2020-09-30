using BlazorAI.Client.Pages;
using BlazorAI.Shared.Types;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorAI.Client.Components
{
    public class SolverLayoutBase<TResult> : ComponentBase
    {
        [Parameter]
        public RenderFragment ParameterSection { get; set; }

        [Parameter]
        public RenderFragment ProblemSection { get; set; }

        [Parameter]
        public RenderFragment OutputSection { get; set; }

        [Parameter]
        public ISolverPage<TResult> Solver { get; set; }

        [Parameter]
        public int SelectedNavIndex { get; set; }

        protected int Generation { get; set; }
        protected double Fitness { get; set; }
        protected double ElapsedSeconds { get; set; }
        protected string FitnessStr => Fitness == 100 ? $"{Fitness:0.00}% ✔️" : $"{Fitness:0.00}% ";

        protected SolverParameters Parameters { get; set; }

        protected bool IsSolving { get; set; }

        protected FitnessChart fitnessChart;

        protected string cssClass = "m-hidden";

        protected string error;
        protected bool showError => !string.IsNullOrEmpty(error);

        private CancellationTokenSource token;
        private Stopwatch stopwatch = new Stopwatch();

        protected override async Task OnInitializedAsync()
        {
            Parameters = Solver.DefaultParameters;

            await Initialise();
        }

        public async Task Initialise()
        {
            // Just want to get back gen 0 so we can see the problem
            var param0 = new SolverParameters { Generations = 0, Population = 2 };

            await RunSolver(param0);

            if (cssClass == "m-hidden")
            {
                cssClass = "m-fadeIn";
            }
        }

        private async Task RunSolver(SolverParameters parameters)
        {
            StopSolver();

            if (fitnessChart != null)
            {
                await fitnessChart.Clear();
            }

            token = new CancellationTokenSource();

            stopwatch.Restart();
            IsSolving = true;

            try
            {
                var results = Solver.GetResults(token, parameters).WithCancellation(token.Token);

                await foreach (var result in results)
                {
                    HandleResult(result);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Task cancelled by user");
            }
            catch (Exception e)
            {
                error = e.Message;

                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                IsSolving = false;
                stopwatch.Stop();

                StateHasChanged();
            }
        }

        protected async Task RunSolver() => await RunSolver(Parameters);

        protected void StopSolver()
        {
            token?.Cancel();

            IsSolving = false;
        }

        protected void HandleResult(Result<TResult> result)
        {
            Generation = result.Generation;
            Fitness = result.Fitness;
            ElapsedSeconds = stopwatch.ElapsedMilliseconds / 1000.0;

            // Time Evolving is less than Elapsed time as we stop and start
            // to give time to UI thread. I think it's more meaningful to show
            // elapsed time at present. With Signal R server or Azure functions,
            // I would expect these values to be very close.
            //ElapsedSeconds = result.TimeEvolving.TotalMilliseconds / 1000.0;

            var point = new Point { X = Generation, Y = (int)Fitness };
            fitnessChart?.AddData(point, Generation);

            Solver.HandleResult(result);
            StateHasChanged();
        }
    }
}

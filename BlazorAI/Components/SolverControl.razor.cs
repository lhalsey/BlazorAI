using BlazorAI.Shared.Types;
using Blazorise.Icons.Material;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorAI.Client.Components
{
    public partial class SolverControl
    {
        [Parameter]
        public bool IsSolving { get; set; }

        [Parameter]
        public EventCallback OnPlay { get; set; }

        [Parameter]
        public EventCallback OnStop { get; set; }

        [Parameter]
        public SolverParameters SolverParameters { get; set; }

        private string PlayIcon => IsSolving ? MaterialIcons.Stop : MaterialIcons.PlayArrow;

        private bool isShowingSettings;

        private string SettingsIcon =>
            isShowingSettings ? MaterialIcons.RemoveCircleOutline : MaterialIcons.AddCircleOutline;

        private string selectorClass => isShowingSettings ? "d-inline" : "d-sm-inline d-none";

        private List<SelectItem<int>> generationChoices =
            SelectHelper.GetItems(100, 200, 500, 1_000, 2_000, 5_000);

        private List<SelectItem<int>> populationChoices =
            SelectHelper.GetItems(10, 25, 50, 100, 200);

        private List<SelectItem<SolverParameters.SolverSelection>> selectionChoices =
            SelectHelper.GetItems(
                SolverParameters.SolverSelection.Elite,
                SolverParameters.SolverSelection.Tournament,
                SolverParameters.SolverSelection.Roulette);

        // Hard-coded values rather than Enumerable.Range with multiplier as that leads to
        // rounding errors, e.g. 0.6500001
        private static float[] range =
            new[] { 0.0f, 0.05f, 0.1f, 0.15f, 0.2f, 0.25f, 0.3f, 0.35f, 0.4f, 0.45f, 0.5f,
                    0.55f, 0.6f, 0.65f, 0.7f, 0.75f, 0.8f, 0.85f, 0.9f, 0.95f, 1.0f };

        private List<SelectItem<float>> crossoverChoices = SelectHelper.GetItems(range);

        private List<SelectItem<float>> mutationChoices = SelectHelper.GetItems(range);

        private void ToggleSolve()
        {
            if (IsSolving)
            {
                OnStop.InvokeAsync(null);
            }
            else
            {
                OnPlay.InvokeAsync(null);
            }

            IsSolving = !IsSolving;

            StateHasChanged();
        }

        private void ToggleSettings()
        {
            isShowingSettings = !isShowingSettings;

            StateHasChanged();
        }
    }
}

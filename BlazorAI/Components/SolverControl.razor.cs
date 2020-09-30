using BlazorAI.Shared.Types;
using BlazorAI.Shared.Utility;
using Blazorise.Icons.Material;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

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

        private List<SelectItem<float>> crossoverChoices =
            SelectHelper.GetItems(0.To(20).Select(x => x * 0.05f).ToArray());

        private List<SelectItem<float>> mutationChoices =
            SelectHelper.GetItems(0.To(20).Select(x => x * 0.05f).ToArray());

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

using BlazorAI.Shared.Types;
using BlazorAI.Shared.Utility;
using Blazorise.Charts;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAI.Client.Components
{
    public partial class FitnessChart : ComponentBase
    {
        [Parameter]
        public int Generations { get; set; }

        [Parameter]
        public int NumPoints { get; set; }

        protected LineChart<Point> lineChart;

        protected LineChartOptions lineChartOptions = new()
        {
            MaintainAspectRatio = true,
            Animation = new ChartAnimation { Duration = 0 },
            AspectRatio = 3.5,
            Plugins = new ChartPlugins
            {
                Legend = new ChartLegend
                {
                    Display = false
                }
            }
        };

        private const int StepSize = 5;

        private int RequiredStep => Generations / NumPoints;
        private int ActualStep => Math.Max(StepSize, RequiredStep);
        private int ActualPoints => Generations / ActualStep;

        public void AddData(Point value, int generation)
        {
            if (generation % ActualStep == 0 || value.Y == 100)
            {
                lineChart.AddData(0, value);
                lineChart.Update();
            }
        }

        public async Task Clear()
        {
            await HandleRedraw();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await HandleRedraw();
            }
        }

        protected async Task HandleRedraw()
        {
            await lineChart.Clear();

            var labels = 0.To(ActualPoints).Select(x => (x * ActualStep).ToString()).ToArray();

            await lineChart.AddLabelsDatasetsAndUpdate(labels, GetLineChartDataset());
        }

        private static LineChartDataset<Point> GetLineChartDataset()
        {
            List<string> backgroundColors = new() { ChartColor.FromRgba(195, 171, 214, 0.2f) };
            List<string> borderColors = new() { ChartColor.FromRgba(195, 171, 214, 0.8f) };

            return new LineChartDataset<Point>
            {
                //Label = "Fitness",
                Data = new List<Point>() { },
                BackgroundColor = backgroundColors,
                BorderColor = borderColors,
                Fill = true,
                PointRadius = 1,
                BorderDash = new List<int> { }
            };
        }
    }
}

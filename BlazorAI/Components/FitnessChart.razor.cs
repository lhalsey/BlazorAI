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
    public class FitnessChartBase : ComponentBase
    {
        [Parameter]
        public int Generations { get; set; }

        [Parameter]
        public int NumPoints { get; set; }

        protected LineChart<Point> lineChart;

        private const int StepSize = 5;

        private int RequiredStep => Generations / NumPoints;
        private int ActualStep => Math.Max(StepSize, RequiredStep);
        private int ActualPoints => Generations / ActualStep;

        public void AddData(Point value, int generation)
        {
            if (generation % ActualStep == 0 || value.Y == 100)
            {
                lineChart.AddData(0, new Point[] { value });
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

            await lineChart.SetOptions(new LineChartOptions
            {
                Legend = new Legend { Display = false },
                ResponsiveAnimationDuration = 0,
                Animation = new Animation { Duration = 0 },
                MaintainAspectRatio = true,
                AspectRatio = 3.5
            });
        }

        private LineChartDataset<Point> GetLineChartDataset()
        {
            List<string> backgroundColors = new List<string> { ChartColor.FromRgba(195, 171, 214, 0.2f) };
            List<string> borderColors = new List<string> { ChartColor.FromRgba(195, 171, 214, 0.8f) };

            return new LineChartDataset<Point>
            {
                Data = new List<Point>() { },
                BackgroundColor = backgroundColors,
                BorderColor = borderColors,
                Fill = true,
                PointRadius = 2,
                BorderDash = new List<int> { }
            };
        }
    }
}

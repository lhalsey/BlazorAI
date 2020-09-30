using Microsoft.AspNetCore.Components;

namespace BlazorAI.Client.ProblemComponents
{
    public class TravellingSalesmanGridBase : ComponentBase
    {
        [Parameter]
        public string Path { get; set; }

        [Parameter]
        public int Width { get; set; }

        [Parameter]
        public int Height { get; set; }

        [Parameter]
        public double Distance { get; set; }

        protected double backgroundOpacity = 0.65;
        protected double overlayOpacity = 0.3;
    }
}

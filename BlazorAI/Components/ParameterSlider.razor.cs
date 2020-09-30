using Microsoft.AspNetCore.Components;

namespace BlazorAI.Client.Components
{
    public partial class ParameterSlider
    {
        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public int Value
        {
            get => sliderValue;
            set
            {
                if (sliderValue != value)
                {
                    sliderValue = value;
                    ValueChanged.InvokeAsync(value);
                }
            }
        }

        [Parameter]
        public int Min { get; set; }

        [Parameter]
        public int Max { get; set; }

        [Parameter]
        public EventCallback<int> ValueChanged { get; set; }

        private int sliderValue;
    }
}

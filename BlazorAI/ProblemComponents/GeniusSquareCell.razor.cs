using Microsoft.AspNetCore.Components;

namespace BlazorAI.Client.ProblemComponents
{
    public partial class GeniusSquareCell
    {
        [Parameter]
        public int Value { get; set; }

        protected double Opacity => Value <= 1 ? 0 : OpacityValue;

        protected double BlockerOpacity => Value == 1 ? OpacityValue : 0;

        protected string Color => Value == 0 ? shapeColors[0] : shapeColors[Value - 1];

        private string[] shapeColors = new[]
         { "transparent", "darkgreen", "grey", "red", "#e391e3",
            "#5cbcdb",  "orange", "purple", "brown", "#545fbf" };

        private const double OpacityValue = 0.35;
    }
}
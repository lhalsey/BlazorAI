using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using BlazorAI.Shared.Solvers;

namespace BlazorAI.Client.ProblemComponents
{
    public partial class House : ComponentBase
    {
        [Parameter]
        public List<Trait> Traits { get; set; }

        const string Blank = "/images/problems/fivehouses/none.svg";

        // Set initial values to set image size and make ease-in animation smoother
        private string colour = "rgba(50,50,50,0)";
        private string colourText;
        private string nationality = Blank;
        private string nationalityText;
        private string pet = Blank;
        private string petText;
        private string drink = Blank;
        private string drinkText;
        private string car = Blank;
        private string carText;

        (string, string) GetValue(Trait trait)
        {
            string ToUrl(string image) => $"/images/problems/fivehouses/{image}.svg";

            var (value, text) =
               trait switch
               {
                   Trait.Green => ("#a4c777", "Green"),
                   Trait.Red => ("#ed8e90", "Red"),
                   Trait.Blue => ("#87c5e0", "Blue"),
                   Trait.Yellow => ("#f5f293", "Yellow"),
                   Trait.Ivory => ("#fafafa", "Ivory"),
                   Trait.English => (ToUrl("england"), "English"),
                   Trait.Norwegian => (ToUrl("norway"), "Norwegian"),
                   Trait.Japanese => (ToUrl("japan"), "Japanese"),
                   Trait.Spanish => (ToUrl("spain"), "Spanish"),
                   Trait.Croatian => (ToUrl("croatia"), "Croatian"),
                   Trait.Coffee => (ToUrl("coffee-cup"), "Coffee"),
                   Trait.Tea => (ToUrl("tea-cup"), "Tea"),
                   Trait.Chocolate => (ToUrl("chocolate"), "Chocolate"),
                   Trait.Milk => (ToUrl("milk"), "Milk"),
                   Trait.OJ => (ToUrl("orange-juice"), "Orange Juice"),
                   Trait.Dog => (ToUrl("dog"), "Dog"),
                   Trait.Horse => (ToUrl("horse"), "Horse"),
                   Trait.Fox => (ToUrl("fox"), "Fox"),
                   Trait.Zebra => (ToUrl("zebra"), "Zebra"),
                   Trait.HedgeHog => (ToUrl("hedgehog"), "Hedgehog"),
                   Trait.Ford => (ToUrl("ford"), "Ford"),
                   Trait.Toyota => (ToUrl("toyota"), "Toyota"),
                   Trait.Subaru => (ToUrl("subaru"), "Subaru"),
                   Trait.Porsche => (ToUrl("porsche"), "Porsche"),
                   Trait.Chevy => (ToUrl("chevrolet"), "Chevy"),
                   _ => throw new InvalidOperationException("Unknown trait")
               };

            return (value, text);
        }

        // TODO: Investigate whether this is optimal way to display
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            (colour, colourText) = GetValue(Traits[0]);
            (pet, petText) = GetValue(Traits[1]);
            (car, carText) = GetValue(Traits[2]);
            (drink, drinkText) = GetValue(Traits[3]);
            (nationality, nationalityText) = GetValue(Traits[4]);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using BlazorAI.Shared.Utility;

namespace BlazorAI.Client.Components
{
    public partial class NavPanel : ComponentBase
    {
        const int NumButtons = 5;

        [Parameter]
        public int SelectedIndex { get; set; }

        string[] buttonClasses;

        string GetClass(bool isSelected) =>
            isSelected ? "nav-button nav-button-selected" : "nav-button";

        protected override void OnInitialized()
        {
            buttonClasses =
                1.To(NumButtons)
                .Select(x => GetClass(x == SelectedIndex))
                .ToArray();
        }
    }
}

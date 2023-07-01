using System;
using Microsoft.AspNetCore.Components;

namespace BlazorAI.Client.Components
{
    public partial class ErrorSnackbar
    {
        [Parameter]
        public string Error { get; set; }

        private bool ShowError => !string.IsNullOrEmpty(Error);
    }
}

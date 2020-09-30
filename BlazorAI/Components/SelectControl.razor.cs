using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAI.Client.Components
{
    public partial class SelectControl<TItem>
    {
        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public List<SelectItem<TItem>> Choices { get; set; }

        [Parameter]
        public TItem Value
        {
            get => selectValue;
            set
            {
                if (!selectValue.Equals(value))
                {
                    selectValue = value;
                    ValueChanged.InvokeAsync(value);
                }
            }
        }

        [Parameter]
        public EventCallback<TItem> ValueChanged { get; set; }

        private TItem selectValue;
    }

    public class SelectItem<T>
    {
        public string ItemLabel { get; set; }
        public T ItemValue { get; set; }
    }

    public static class SelectHelper
    {
        public static List<SelectItem<int>> GetItems(params int[] values) =>
            values
            .Select(x => new SelectItem<int> { ItemLabel = x.ToString("N0"), ItemValue = x })
            .ToList();

        public static List<SelectItem<float>> GetItems(params float[] values) =>
            values
            .Select(x => new SelectItem<float> { ItemLabel = x.ToString("N2"), ItemValue = x })
            .ToList();

        public static List<SelectItem<T>> GetItems<T>(params T[] values) =>
            values
            .Select(x => new SelectItem<T> { ItemLabel = x.ToString(), ItemValue = x })
            .ToList();
    }
}

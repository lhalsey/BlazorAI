using Blazorise;

namespace BlazorAI.Client
{
    public partial class App
    {
        private Theme theme = new Theme
        {
            BreakpointOptions = new ThemeBreakpointOptions
            {
            },
            BarOptions = new ThemeBarOptions
            {
                DarkColors = new ThemeBarColorOptions
                {
                    BackgroundColor = "#333377",
                    Color = "#ffffff"
                }
            },
            ColorOptions = new ThemeColorOptions
            {
                Primary = "#0288D1",
                //Secondary = "#A65529",
                //Success = "#23C02E",
                //Info = "#9BD8FE",
                //Warning = "#F8B86C",
                //Danger = "#F95741",
                //Light = "#F0F0F0",
                //Dark = "#535353",
            },
            BackgroundOptions = new ThemeBackgroundOptions
            {
                Primary = "#0288D1",
            },
            TextColorOptions = new ThemeTextColorOptions
            {
            },
            //ButtonOptions = new ThemeButtonOptions
            //{ 
            //    LargeBorderRadius = "#FF0000",
            //    SmallBorderRadius = "#FFFF00"
            //},
            //DropdownOptions = new ThemeDropdownOptions { },
            InputOptions = new ThemeInputOptions
            {
                CheckColor = "#0288D1",
            },
            //CardOptions = new ThemeCardOptions { },
            //ModalOptions = new ThemeModalOptions { },
            //TabsOptions = new ThemeTabsOptions { },
            //ProgressOptions = new ThemeProgressOptions { },
            //AlertOptions = new ThemeAlertOptions { },
            //BreadcrumbOptions = new ThemeBreadcrumbOptions { },
            //BadgeOptions = new ThemeBadgeOptions { },
            //PaginationOptions = new ThemePaginationOptions { },
            //TooltipOptions = new ThemeTooltipOptions
            //{
            //    BackgroundColor = "#22c8ce",
            //    BackgroundOpacity = .7f,
            //    Color = "#ff0000",
            //},
            //SnackbarOptions = new ThemeSnackbarOptions {}
        };
    }
}

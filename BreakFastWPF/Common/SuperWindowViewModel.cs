using System.Configuration;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;
using System;

namespace BreakFastWPF.Common
{
    public class SuperWindowViewModel
    {
        public SuperWindowViewModel()
        {
            AppItems = new[]
            {
                new AppItem("Setup", new SettingWindow(), new []
                    {
                        DocumentationLink.AppPageLink<SettingWindow>(),
                        DocumentationLink.StyleLink("SettingWindow"),
                        DocumentationLink.ApiLink<SettingWindow>()
                    })
                {
                    VerticalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto
                },
            };
        }

        public AppItem[] AppItems { get; }
    }
}
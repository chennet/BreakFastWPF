using System.Configuration;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;
using System;

namespace BreakFastWPF.Common
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel(ISnackbarMessageQueue snackbarMessageQueue)
        {
            if (snackbarMessageQueue == null) throw new ArgumentNullException(nameof(snackbarMessageQueue));

            AppItems = new[]
            {
                new AppItem("Order", new Order { DataContext = new DialogsViewModel() },
                    new []
                    {
                        new DocumentationLink(DocumentationLinkType.Wiki, $"{ConfigurationManager.AppSettings["GitHub"]}/wiki", "WIKI"),
                        DocumentationLink.AppPageLink<Order>()
                    }
                ),
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
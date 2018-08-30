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
                //new AppItem("Order", new Order { DataContext = new DialogsViewModel() },
                //new AppItem("Order", new Order(),
                //    new []
                //    {
                //        //new DocumentationLink(DocumentationLinkType.Wiki, $"{ConfigurationManager.AppSettings["GitHub"]}/wiki", "WIKI"),
                //        DocumentationLink.AppPageLink<Order>()
                //    }
                //),
                new AppItem("All", new MenuOrder(), new []
                    {
                        DocumentationLink.ApiLink<MenuOrder>()
                    })
                {
                    VerticalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto
                },
                new AppItem("Bread", new MenuOrder("Bread"), new []
                    {
                        DocumentationLink.ApiLink<MenuOrder>()
                    })
                {
                    VerticalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto
                },
                new AppItem("Rice", new MenuOrder("Rice"), new []
                    {
                        DocumentationLink.ApiLink<MenuOrder>()
                    })
                {
                    VerticalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto
                },
                new AppItem("Drink", new MenuOrder("Drink"), new []
                    {
                        DocumentationLink.ApiLink<MenuOrder>()
                    })
                {
                    VerticalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto
                }
            };
        }

        public AppItem[] AppItems { get; }
    }
}
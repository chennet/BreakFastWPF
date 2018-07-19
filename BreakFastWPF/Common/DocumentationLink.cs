using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BreakFastWPF.Common
{
    public class DocumentationLink
    {
        public DocumentationLink(DocumentationLinkType type, string url) : this(type, url, null)
        {
        }

        public DocumentationLink(DocumentationLinkType type, string url, string label)
        {
            Label = label ?? type.ToString();
            Url = url;
            Type = type;
            Open = new AnotherCommandImplementation(Execute);
        }

        public static DocumentationLink WikiLink(string page, string label)
        {
            return new DocumentationLink(DocumentationLinkType.Wiki,
                $"{ConfigurationManager.AppSettings["GitHub"]}/wiki/" + page, label);
        }

        public static DocumentationLink StyleLink(string nameChunk)
        {
            return new DocumentationLink(
                DocumentationLinkType.StyleSource,
                $"{ConfigurationManager.AppSettings["GitHub"]}/blob/master/MaterialDesignThemes.Wpf/Themes/MaterialDesignTheme.{nameChunk}.xaml",
                nameChunk);
        }

        public static DocumentationLink ApiLink<TClass>(string subNamespace)
        {
            var typeName = typeof(TClass).Name;

            return new DocumentationLink(
                DocumentationLinkType.ControlSource,
                $"{ConfigurationManager.AppSettings["GitHub"]}/blob/master/MaterialDesignThemes.Wpf/{subNamespace}/{typeName}.cs",
                typeName);
        }


        public static DocumentationLink ApiLink<TClass>()
        {
            var typeName = typeof(TClass).Name;

            return new DocumentationLink(
                DocumentationLinkType.ControlSource,
                $"{ConfigurationManager.AppSettings["GitHub"]}/blob/master/MaterialDesignThemes.Wpf/{typeName}.cs",
                typeName);
        }

        public static DocumentationLink AppPageLink<TAppPage>()
        {
            return AppPageLink<TAppPage>(null);
        }

        public static DocumentationLink AppPageLink<TAppPage>(string label)
        {
            return AppPageLink<TAppPage>(label, null);
        }

        public static DocumentationLink AppPageLink<TAppPage>(string label, string nameSpace)
        {
            var ext = typeof(UserControl).IsAssignableFrom(typeof(TAppPage))
                ? "xaml"
                : "cs";


            return new DocumentationLink(
                DocumentationLinkType.AppPageSource,
                $"{ConfigurationManager.AppSettings["GitHub"]}BreakFastWPF/{(string.IsNullOrWhiteSpace(nameSpace) ? "" : ("/" + nameSpace + "/"))}{typeof(TAppPage).Name}.{ext}",
                label ?? typeof(TAppPage).Name);
        }

        public string Label { get; }

        public string Url { get; }

        public DocumentationLinkType Type { get; }

        public ICommand Open { get; }

        private void Execute(object o)
        {
            System.Diagnostics.Process.Start(Url);
        }
    }
}
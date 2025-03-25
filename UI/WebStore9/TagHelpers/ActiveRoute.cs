using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebStore9.TagHelpers
{
    [HtmlTargetElement(Attributes = AttributeName)]
    public class ActiveRoute : TagHelper
    {
        private const string AttributeName = "ws-is-active-route";
        private const string IgnoreAction = "ws-ignore-action";

        [HtmlAttributeName("asp-controller")]
        public string controller { get; set; }

        [HtmlAttributeName("asp-action")]
        public string action { get; set; }

        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public Dictionary<string, string> routeValuesDictionary { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext viewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var isIgnoreAction = output.Attributes.RemoveAll(IgnoreAction);

            if (IsActive(isIgnoreAction))
                MakeActive(output);

            output.Attributes.RemoveAll(AttributeName);
        }

        private bool IsActive(bool ignoreAction)
        {
            var routeValues = viewContext.RouteData.Values;
            var routeController = routeValues["controller"]?.ToString();
            var routeAction = routeValues["action"]?.ToString();

            if (!ignoreAction && this.action is { Length: > 0 } action && !string.Equals(action, routeAction))
                return false;

            if (this.controller is { Length: > 0 } controller && !string.Equals(controller, routeController))
                return false;

            foreach (var (key, value) in routeValuesDictionary)
            {
                if (!routeValues.ContainsKey(key) || routeValues[key]?.ToString() != value)
                    return false;
            }

            return true;
        }

        private void MakeActive(TagHelperOutput output)
        {
            var classAttribute = output.Attributes.FirstOrDefault(attr => attr.Name == "class");

            if (classAttribute is null)
            {
               output.Attributes.Add("class","active"); 
            }
            else
            {
                if (classAttribute.Value?.ToString().Contains("active") ?? false)
                    return;

                output.Attributes.SetAttribute("class", classAttribute.Value + " active");
            }
        }
    }
}

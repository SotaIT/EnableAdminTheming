using Microsoft.AspNetCore.Mvc.Razor;
using Nop.Web.Framework;
using System.Collections.Generic;
using System.Linq;
using Nop.Web.Framework.Themes;

namespace Nop.Plugin.Admin.EnableAdminTheming
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        private const string THEME_KEY = "nop.admintheming.themename";
        private const string IS_ADMIN_AREA = "nop.admintheming.isadminarea";

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            if (context.AreaName?.Equals(AreaNames.Admin) ?? false)
                context.Values[IS_ADMIN_AREA] = "1";

            var themeContext =
                (IThemeContext)context.ActionContext.HttpContext.RequestServices.GetService(typeof(IThemeContext));
            context.Values[THEME_KEY] = themeContext.WorkingThemeName;
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {

            if (context.Values.TryGetValue(THEME_KEY, out var theme)
                && context.Values.TryGetValue(IS_ADMIN_AREA, out var isAdmin)
                && isAdmin == "1")
                viewLocations = new[]
                    {
                        $"/Themes/{theme}/AdminViews/{{1}}/{{0}}.cshtml"
                    }
                    .Concat(viewLocations);

            return viewLocations;
        }
    }
}
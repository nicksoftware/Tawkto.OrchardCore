using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.Admin;
using OrchardCore.Entities;
using OrchardCore.ResourceManagement;
using OrchardCore.Settings;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Tawkto.OrchardCore.Settings;
using System.Text;

namespace Tawkto.OrchardCore.Filters
{
    public class TawkToFilter : IAsyncResultFilter
    {
        private readonly IResourceManager _resourceManager;
        private readonly ISiteService _siteService;

        public TawkToFilter(
            IResourceManager resourceManager,
            ISiteService siteService)
        {
            _resourceManager = resourceManager;
            _siteService = siteService;
        }


        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            //Only inject in client not admin
            if ((context.Result is ViewResult || context.Result is PageResult)
                && !AdminAttribute.IsApplied(context.HttpContext))
            {
                var settings = (await _siteService.GetSiteSettingsAsync())
                    .As<TawktoSettings>();


                if (!string.IsNullOrEmpty(settings.WidgetName) &&
                    !string.IsNullOrEmpty(settings.TokenKey))
                {

                    StringBuilder scriptBuilder = new StringBuilder();

                    scriptBuilder
                        .Append("<script type=\"text / javascript\">")
                        .AppendLine("var Tawk_API=Tawk_API||{}, Tawk_LoadStart=new Date();")
                        .AppendLine("(function(){")
                        .AppendLine("var s1=document.createElement(\"script\"),s0=document.getElementsByTagName(\"script\")[0];")
                        .AppendLine("s1.async=true;")
                        .Append("s1.src='https://embed.tawk.to/"+ settings.TokenKey+"/"+ settings.WidgetName+ "';")
                        .AppendLine()
                        .AppendLine("s1.charset='UTF-8';")
                        .AppendLine("s1.setAttribute('crossorigin','*');")
                        .AppendLine("s0.parentNode.insertBefore(s1,s0);")
                        .AppendLine("})();")
                        .AppendLine("</script>");


                    HtmlString htmlString = new HtmlString(scriptBuilder.ToString());

                    _resourceManager.RegisterFootScript(htmlString);
                }
            }

            await next.Invoke();
        }
    }
}   

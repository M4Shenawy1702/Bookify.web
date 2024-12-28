using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Bookify.web.Filters
{
    public class AjaxOnly : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            var Request = routeContext.HttpContext.Request;
            var IsAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            return IsAjax;
        }
    }
}

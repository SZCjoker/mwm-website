using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Phoenixnet.Extensions.Web
{
    public class RequiredFromQueryActionConstraint : IActionConstraint
    {
        private readonly string _parameter;

        public RequiredFromQueryActionConstraint(string parameter)
        {
            _parameter = parameter;
        }

        public int Order => 999;

        public bool Accept(ActionConstraintContext context)
        {
            if (!context.RouteContext.HttpContext.Request.Query.ContainsKey(_parameter))
            {
                return false;
            }

            string stringValue = context.RouteContext.HttpContext.Request.Query[_parameter];
            if (string.IsNullOrEmpty(stringValue))
            {
                return false;
            }

            return true;
        }
    }
}
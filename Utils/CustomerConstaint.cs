
namespace asp.net_tuto_02.Utils
{
    public class CustomerConstaint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {

            if (!values.ContainsKey(routeKey)) return false;
            if (values[routeKey] is null) return false;
            if (values[routeKey].ToString().Equals("silver",StringComparison.OrdinalIgnoreCase) ||
                values[routeKey].ToString().Equals("gold",StringComparison.OrdinalIgnoreCase) )
            {
                return true; 
            }
            return false;
            
        }
    }
}

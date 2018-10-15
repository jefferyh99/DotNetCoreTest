using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangFireTest
{

    //https://dotnetthoughts.net/integrate-hangfire-with-aspnet-core/

    // By default Hangfire allows access to Dashboard pages only for local requests.In order to give appropriate rights for production use. You need to implement IDashboardAuthorizationFilter interface to configure authorization.
    //Here is the minimal IDashboardAuthorizationFilter, which will allows all the authenticated users to view the dashboard.(Note : This approach is not recommended for production.)
    public class CustomAuthorizeFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            //var httpcontext = context.GetHttpContext();
            //return httpcontext.User.Identity.IsAuthenticated;
            return true;
        }
    }
}

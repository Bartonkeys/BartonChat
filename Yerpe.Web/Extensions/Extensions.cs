using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Microsoft.Owin;

namespace Yerpe.Web.Extensions
{
    public static class Extensions
    {
        public static String GetNameFromEmail(this string email)
        {
            return email.Split('@')[0];
        }

        public static IOwinContext GetOwinContext(this HttpRequestMessage request)
        {
            var context = request.Properties["MS_HttpContext"] as HttpContextWrapper;
            if (context != null)
            {
                return HttpContextBaseExtensions.GetOwinContext(context.Request);
            }
            return null;
        }
    }
}
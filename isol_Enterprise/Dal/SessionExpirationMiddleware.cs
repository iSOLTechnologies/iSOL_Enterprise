using System;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;

namespace iSOL_Enterprise.Dal
{
    public class SessionExpirationMiddleware
    {
        

        //public static  string Email = "";
        //public  static  DateTime? SessionTimeout = null;
        private readonly RequestDelegate _next;

        public SessionExpirationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
       

        public async Task InvokeAsync(HttpContext context)
        {
            var session = context.Session;
            string Email = session.GetString("Email");
            DateTime? SessionTimeout = Convert.ToDateTime(session.GetString("SessionTimeout"));
            session.SetString("LastAccessTime", DateTime.Now.ToString());
            // Check if the session has expired
            if (session.IsAvailable && session.TryGetValue("LastAccessTime", out var lastAccessTimeObj))
            {
                //var lastAccessTimeJson = Encoding.UTF8.GetString(lastAccessTimeObj);
                //var lastAccessTime = Convert.ToDateTime((lastAccessTimeJson));

                var currentTime = DateTime.Now;

                //var sessionTimeoutMinutes = GetSessionTimeoutMinutes(context);
                
                if (currentTime > SessionTimeout)
                {
                   new LoginDal().ReomveSession(Email);
                   // Perform your desired tasks when the session expires
                   // For example, log the event, clean up resources, or perform any other necessary tasks
                   // Your custom logic here


                   // Clear the session to ensure it is fully expired
                   session.Clear();
                }
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }

       
    }
}

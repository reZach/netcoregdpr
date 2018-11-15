using GDPR.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace GDPR.MaxMind
{
    public class GDPRFilter : ActionFilterAttribute
    {
        private readonly IMaxMindService _iMaxMindService;
        public GDPRFilter()
        {
        }
        public GDPRFilter(IMaxMindService iMaxMindService)
        {
            _iMaxMindService = iMaxMindService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Grab the client's ip address
            IPAddress clientIP = context.HttpContext.Connection.RemoteIpAddress;

            // Localhost woes  
            if (clientIP.ToString() == "::1" ||
              clientIP.ToString() == "127.0.0.1")
            {
                (context.Controller as Controller).ViewBag.isEU = "False";

                // Un-comment me to test redirect on localhost
                if (!(context.Controller is UnavailableController))
                {
                    context.Result = new RedirectToActionResult("Index", "Unavailable", null);
                }                
            }
            else
            {
                // See if the client is from the EU
                Dictionary<string, object> raw = _iMaxMindService.GetData(clientIP.ToString());
                var continentData = (raw["continent"]);
                var json = JsonConvert.SerializeObject(continentData);
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                if (dictionary["code"].ToString() == "EU")
                {
                    (context.Controller as Controller).ViewBag.isEU = "True";
                    if (!(context.Controller is UnavailableController))
                    {
                        // Redirect to our not available controller
                        context.Result = new RedirectToActionResult("Index", "Unavailable", null);
                    }
                }
                else
                {
                    (context.Controller as Controller).ViewBag.isEU = "False";
                }
            }
        }
    }
}
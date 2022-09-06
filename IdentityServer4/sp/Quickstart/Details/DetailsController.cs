using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using IdentityServer4.Events;
using IdentityServer4.Extensions;

namespace IdentityServer4.Quickstart.UI
{
    /// <summary>
    /// This sample controller allows a user to revoke grants given to clients
    /// </summary>
    [SecurityHeaders]
    [Authorize]
    public class DetailsController : Controller
    {
        public DetailsController()
        {
            
        }

        /// <summary>
        /// Show list of grants
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
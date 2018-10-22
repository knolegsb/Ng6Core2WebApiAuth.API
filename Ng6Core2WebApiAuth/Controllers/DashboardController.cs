using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ng6Core2WebApiAuth.Data;
using Ng6Core2WebApiAuth.Models.Entities;

namespace Ng6Core2WebApiAuth.Controllers
{
    //[Produces("application/json")]
    //[Route("api/Dashboard")]
    [Authorize(Policy = "ApiUser")]
    [Route("api/[controller]/[action]")]
    public class DashboardController : Controller
    {
        private readonly ClaimsPrincipal _claims;
        private readonly AppDbContext _appDbContext;

        public DashboardController(
            UserManager<AppUser> userManager,
            AppDbContext appDbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _claims = httpContextAccessor.HttpContext.User;
            _appDbContext = appDbContext;
        }

        // GET api/dashboard/home
        [HttpGet]
        public async Task<IActionResult> Home()
        {
            // retrieve the user info
            // HttpContext.User
            var userId = _claims.Claims.Single(c => c.Type == "id");
            var customer = await _appDbContext.Customers.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);

            return new OkObjectResult(new
            {
                Message = "this is secure API and user data!",
                customer.Identity.FirstName,
                customer.Identity.LastName,
                customer.Identity.PicturUrl,
                customer.Identity.FacebookId,
                customer.Location,
                customer.Locale,
                customer.Gender
            });
        }
    }
}
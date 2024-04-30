using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace ShoppingStationery.Controllers
{
	public class AuthorizeUser : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var user = context.HttpContext.Session.GetString("User");
			if (user == null)
			{
				context.Result = new RedirectToActionResult("Login", "Home", null);
			}
			base.OnActionExecuting(context);
		}
	}
}
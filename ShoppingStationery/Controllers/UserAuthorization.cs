using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using ShoppingStationery.Models;

namespace ShoppingStationery.Controllers
{
	public class UserAuthorization : ActionFilterAttribute
	{
		private readonly int[] allowedRoles;

		public UserAuthorization(params int[] roles)
		{
			allowedRoles = roles;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var userString = context.HttpContext.Session.GetString("User");
			// Deserialize đối tượng NguoiDung từ Session
			var nguoiDung = JsonConvert.DeserializeObject<NguoiDung>(userString);
			if (nguoiDung == null)
			{
				// Nếu không deserialize được, chuyển hướng đến trang Error
				context.Result = new RedirectToActionResult("Forbidden", "Home", null);
				return;
			}

			// Lấy MaCv từ đối tượng nguoiDung
			int? maCv = nguoiDung.MaCv;
			// Kiểm tra xem người dùng có quyền truy cập Action này không
			var isAuthorized = false;
			if (allowedRoles.Length == 0 || allowedRoles.Contains(maCv.Value))
			{
				isAuthorized = true;
			}

			if (!isAuthorized)
			{
				// Nếu không có quyền, chuyển hướng đến trang Error
				context.Result = new RedirectToActionResult("Forbidden", "Home", null);
				return;
			}

			base.OnActionExecuting(context);
		}
	}
}

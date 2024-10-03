namespace RBN_FE
{
    public class AdminRoleMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminRoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;

            if (context.Session != null)
            {
                var userRole = context.Session.GetString("UserRole");

                // Kiểm tra đường dẫn UserManagement
                if (path.StartsWithSegments("/Admin/UserManagement"))
                {
                    // Cho phép cả Admin và Staff truy cập
                    if (userRole != "Admin" && userRole != "Staff")
                    {
                        context.Response.Redirect("/Index");
                        return;
                    }
                }
                // Các trang khác trong /Admin chỉ cho phép Admin
                else if (path.StartsWithSegments("/Admin"))
                {
                    if (userRole != "Admin")
                    {
                        context.Response.Redirect("/Index");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }



}

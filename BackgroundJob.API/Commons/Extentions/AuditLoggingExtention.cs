using BackgroundJob.API.Commons.Enums;
using BackgroundJob.API.Commons.Utils;
using BackgroundJob.API.Models;

namespace BackgroundJob.API.Commons.Extentions
{
    public class AuditLoggingExtention
    {
        private readonly RequestDelegate _next;

        public AuditLoggingExtention(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context, ProductDbContext productDbContext)
        {
            var request = context.Request;
            var method = request.Method;

            if (method == HttpMethods.Post || method == HttpMethods.Put || method == HttpMethods.Delete)
            {
                // Truy xuất thông tin route để lấy tên controller
                var routeData = context.GetRouteData();
                var controllerName = routeData.Values["controller"]?.ToString();
                var actionName = routeData.Values["action"]?.ToString();

                // Truy xuất thông tin người dùng và lưu lại log hành động
                var userName = context.User.Identity?.Name ?? "???";

                var auditType = method switch
                {
                    "POST" => AuditType.Create,
                    "PUT" => AuditType.Update,
                    "DELETE" => AuditType.Delete,
                };

                var auditEntry = new AuditEntry
                {
                    UserId = userName,
                    AuditType = auditType,
                    TableName = $"Controller: {controllerName} - Action: {actionName}",
                };

                // Lưu auditLog vào CSDL hoặc hệ thống logging
                productDbContext.AuditLogs.Add(auditEntry.ToAudit());
                productDbContext.SaveChanges();
            }

            await _next(context);
        }
    }
}

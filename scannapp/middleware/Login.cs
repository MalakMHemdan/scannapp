namespace scannapp.middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 🟢 Log Request
            _logger.LogInformation($"Incoming Request: {context.Request.Method} {context.Request.Path}");

            // 🟢 Copy Original Response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            // 🟢 Log Response
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation($"Response Status: {context.Response.StatusCode}, Body: {text}");

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}

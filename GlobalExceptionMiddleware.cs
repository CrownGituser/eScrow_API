namespace SMFG_API_New
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error: {ex.Message}");

                // Return a 500 response
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
            }
        }
    }
}

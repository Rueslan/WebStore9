namespace WebStore9.Infrastructure.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<TestMiddleware> _Logger;

        public TestMiddleware(RequestDelegate next, ILogger<TestMiddleware> Logger)
        {
            _Next = next;
            _Logger = Logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var processing = _Next(context);

            await processing;
        }
    }
}

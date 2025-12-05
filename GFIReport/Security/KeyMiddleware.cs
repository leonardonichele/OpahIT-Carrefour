public class KeyMiddleware : IMiddleware
{
    private readonly string _apiKey;

    public KeyMiddleware(IConfiguration config)
    {
        _apiKey = config["ApiKey"]!;
    }

    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
            return next(context);
        
        return next(context);
    }

}
namespace Backend.API.Filters;

public sealed class RequireSessionFilter(Session session) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!session.IsConnected)
        {
            context.Result = new UnauthorizedObjectResult(AuthErrors.NotConnected.ToResponse());
            return;
        }

        if (!session.IsAuthenticated)
        {
            context.Result = new UnauthorizedObjectResult(AuthErrors.NotAuthenticated.ToResponse());
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}

namespace DealMatcher.Backend.Web;

public sealed class CommandLogger<TCommand, TResult>(ILogger<TCommand> logger) :
    ICommandMiddleware<TCommand, TResult>
    where TCommand : FastEndpoints.ICommand<TResult>
{
    public async Task<TResult> ExecuteAsync(TCommand command,
        CommandDelegate<TResult> next,
        CancellationToken ct)
    {
        var commandName = command.GetType().Name;

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Handling {RequestName}", commandName);

            // Reflection! Could be a performance concern
            var myType = command.GetType();
            var props = new List<PropertyInfo>(myType.GetProperties());
            foreach (var prop in props)
            {
                var propValue = prop?.GetValue(command, null);
                logger.LogInformation("Property {Property} : {@Value}", prop?.Name, propValue);
            }
        }

        var sw = Stopwatch.StartNew();

        var result = await next();

        logger.LogInformation("Handled {CommandName} with {Result} in {ms} ms", commandName, result,
            sw.ElapsedMilliseconds);
        sw.Stop();

        return result;
    }
}

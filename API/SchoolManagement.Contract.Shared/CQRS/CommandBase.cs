using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace SchoolManagement.Shared.CQRS;

public enum Status
{
    Success,
    Error,
    Pending,
}

public class DynamicCommand : IRequest<DynamicCommandResponse>
{
    public DynamicCommand() 
    {
        FieldValues = new();
    }
    
    public bool WaitForResponse { get; set; }
    public string Name { get; set; }
    public string Api { get; set; }
    public Dictionary<string, object> FieldValues { get; set; }

    public DynamicCommand SetValue(string key, object value)
    {
        if (FieldValues.ContainsKey(key))
        {
            FieldValues[key] = value;
        } 
        else
        {
            FieldValues.Add(key, value);
        }
        return this;
    }

    public T? GetValue<T>(string key)
    {
        FieldValues.TryGetValue(key, out var value);

        if (value is not null) return (T?)value;
        
        return (T?)value;
    }

    public DynamicCommandResponse CreateResponse()
    {
        return new DynamicCommandResponse 
        { 
            Name = Name,
            Api = Api,
            Status = Status.Success,
        };
    }
}

public class DynamicCommandResponse
{
    public string Name { get; set; }
    public string Api { get; set; }
    public Status Status { get; set; }
    public Dictionary<string, object> FieldValues { get; set; }

    public DynamicCommandResponse SetValue(string key, object value)
    {
        if (FieldValues.ContainsKey(key))
        {
            FieldValues[key] = value;
        }
        else
        {
            FieldValues.Add(key, value);
        }
        return this;
    }

    public T? GetValue<T>(string key)
    {
        FieldValues.TryGetValue(key, out var value);

        if (value is not null) return (T?)value;

        return (T?)value;
    }

    public DynamicCommandResponse()
    {
        FieldValues = new();
    }
}

public interface IDynamicCommandConsumer
{
    Task<DynamicCommandResponse> DoExecuteAsync(DynamicCommand command, ConsumeContext<DynamicCommand> context = null);
}

public abstract class ADynamicCommandConsumer : ACommandConsumer<DynamicCommand, DynamicCommandResponse>, IDynamicCommandConsumer
{
    public async Task<DynamicCommandResponse> DoExecuteAsync(DynamicCommand command, ConsumeContext<DynamicCommand> context = null)
    {
        return await ExecuteAsync(command, context);
    }
}


public interface ICommandService
{
    Task<DynamicCommandResponse> ExecuteAsync(DynamicCommand command);
}

public class CommandService : ICommandService
{
    public IBus _bus;
    
    public CommandService(IBus bus)
    {
        _bus = bus;
    }

    public async Task<DynamicCommandResponse> ExecuteAsync(DynamicCommand command)
    {
        if (command.WaitForResponse)
        {
            var consumer = IocContainer.Instance.Resolve<IDynamicCommandConsumer>(command.Name + "Consumer");

            if (consumer != null)
            {
                return await consumer.DoExecuteAsync(command);
            }
        }

        await _bus.Publish(command);

        var response = command.CreateResponse();
        response.Status = Status.Pending;

        return response;
    }

}


public sealed class IocContainer
{
private static object _lockObject = new();
private static IocContainer _instance = null;

private IocContainer() {}

public static IocContainer Instance 
{ 
    get 
    { 
        if (_instance is null)
        {
            lock(_lockObject)
            {
                if (_instance is null)
                {
                    _instance = new IocContainer();
                }
            }
        }
        return _instance;
    } 
}


public IServiceProvider? ServiceProvider { get; private set; } // root service provider

public void SetServiceProvider(IServiceProvider serviceProvider)
{
    ServiceProvider = serviceProvider;
}

public T? Resolve<T>(string name)
{
    return ServiceProvider.GetKeyedService<T>(name);
}

public T? Resolve<T>()
{
    return ServiceProvider.GetService<T>();
}

public List<T>? ResolveMany<T>()
{
    return ServiceProvider.GetServices<T>().ToList();
}
}

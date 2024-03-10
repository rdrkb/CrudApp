using MassTransit;
using MediatR;

namespace SchoolManagement.Shared.CQRS;

public abstract class ACommandConsumer<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>, IConsumer<TCommand> 
    where TCommand : class, IRequest<TResponse>
{
    public async Task Consume(ConsumeContext<TCommand> context)
    {
        await ExecuteAsync(context.Message, context);
    }

    public async Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteAsync(request);
    }

    protected abstract Task<TResponse> ExecuteAsync(TCommand command, ConsumeContext<TCommand> context = null);
}

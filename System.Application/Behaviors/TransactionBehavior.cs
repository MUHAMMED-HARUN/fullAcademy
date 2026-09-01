using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions;
using System.Application.Abstractions.UOW;
using System.Domain.Entities.BaseClass;

namespace System.Application.Behaviors
{
    public sealed class TransactionBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPublisher _publisher;
        public TransactionBehavior(IUnitOfWork context, IPublisher publisher)
        {
            _uow = context;
            _publisher = publisher;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // 1️⃣ الحالة الأولى: IQuery (قراءة فقط - أسرع أداء بدون أي حفظ أو معاملات)
            if (request is IQuery)
            {
                return await next();
            }

            // 2️⃣ الحالة الثانية: ITransaction (تعديل معقد يقتضي Explicit Transaction)
            if (request is ITransaction)
            {
                try
                {
                    await _uow.BeginTransactionAsync();

                    var response = await next();

                    var domainEntities = _uow.GetEntitiesWithDomainEvents().ToList();
                    var domainEvents = domainEntities
                       .SelectMany(x => x.DomainEvents)
                       .ToList();

                    domainEntities.ForEach(x => x.ClearDomainEvents());

                    await _uow.SaveChangesAsync(cancellationToken);

                    foreach (var domainEvent in domainEvents)
                    {
                        await _publisher.Publish(domainEvent, cancellationToken);
                    }
                    await _uow.CommitAsync();

                    return response;
                }
                catch
                {
                    await _uow.RollbackAsync();
                    throw;
                }
            }

            // 3️⃣ الحالة الثالثة: ICommand (تعديل بسيط - SaveChangesAsync فقط)
            if (request is ICommand)
            {
                var response = await next();

                await _uow.SaveChangesAsync(cancellationToken);

                return response;
            }

            // افتراضي لأي طلب لا ينتمي للواجهات السابقة
            return await next();
        }
    }
}
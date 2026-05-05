using DealMatcher.Backend.Core.Aggregates.Payment.DTOs;

namespace DealMatcher.Backend.UseCases.Features.Purchase.PaymentMethods;

public sealed class GetPaymentMethodsQueryHandler(
    IReadRepository<PaymentMethod> methodsRepository,
    IMapper mapper) :
    IQueryHandler<GetPaymentMethodsQuery, Result<List<PaymentMethodDTO>>>
{
    public async Task<Result<List<PaymentMethodDTO>>> Handle(
        GetPaymentMethodsQuery request,
        CancellationToken cancellationToken)
    {
        var methods = await methodsRepository.ListAsync(cancellationToken);

        var methodDTOs = mapper.Map<List<PaymentMethodDTO>>(methods);

        return Result.Success(methodDTOs);
    }
}

namespace DealMatcher.Backend.UseCases.Features.Purchase.DeliveryMethods;

public sealed class GetDeliveryMethodsQueryHandler(
    IReadRepository<DeliveryMethod> methodsRepository,
    IMapper mapper) :
    IQueryHandler<GetDeliveryMethodsQuery, Result<List<DeliveryMethodDTO>>>
{
    public async Task<Result<List<DeliveryMethodDTO>>> Handle(
        GetDeliveryMethodsQuery request,
        CancellationToken cancellationToken)
    {
        var methods = await methodsRepository.ListAsync(cancellationToken);

        var methodDTOs = mapper.Map<List<DeliveryMethodDTO>>(methods);

        return Result.Success(methodDTOs);
    }
}

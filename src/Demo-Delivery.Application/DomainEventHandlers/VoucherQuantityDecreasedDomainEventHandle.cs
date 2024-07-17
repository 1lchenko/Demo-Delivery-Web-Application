using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Domain.Entities.VoucherAggregate;
using Demo_Delivery.Domain.Events;
using MediatR;

namespace Demo_Delivery.Application.DomainEventHandlers;

public class VoucherQuantityDecreasedDomainEventHandle : INotificationHandler<VoucherAppliyedToOrderEvent>
{
    private readonly IRepository<Voucher> _voucherRepository;

    public VoucherQuantityDecreasedDomainEventHandle(IRepository<Voucher> voucherRepository)
    {
        _voucherRepository = voucherRepository;
    }

    public async Task Handle(VoucherAppliyedToOrderEvent domainEvent, CancellationToken cancellationToken)
    {
        domainEvent.Voucher.UseUnit();
        await _voucherRepository.UpdateAsync(domainEvent.Voucher, cancellationToken);
        // reposit.SaveChanges ???
    }
}
using Demo_Delivery.Domain.Entities.VoucherAggregate;
using MediatR;

namespace Demo_Delivery.Domain.Events;

public record VoucherAppliyedToOrderEvent(Voucher Voucher) : INotification;
 
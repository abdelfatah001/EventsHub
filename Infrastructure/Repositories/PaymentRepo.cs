using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly AppDbContext _context; 

        public PaymentRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            ArgumentNullException.ThrowIfNull(payment, nameof(payment));

            _context.Payments.Add(payment);
            return await _context.SaveChangesAsync() > 0 ? payment : null;
        }

        public async Task<Payment?> GetPaymentByIdAsync(int paymentId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(paymentId, nameof(paymentId));

            return await _context.Payments
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == paymentId);
        }
        public async Task<Payment?> GetPaymentByProviderReferenceAsync(string providerReference)
        {
            ArgumentNullException.ThrowIfNull(providerReference, nameof(providerReference));

            return await _context.Payments
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProviderReference == providerReference);
        }

        public async Task<IEnumerable<Payment>?> GetPaymentsByOrderIdAsync(int orderId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(orderId, nameof(orderId));
            
            return await _context.Payments
                .AsNoTracking()
                .Where(p => p.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<bool> UpdatePaymentStatusAsync(int paymentId, enPaymentStatus status)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(paymentId, nameof(paymentId));

            var affectedRows = await _context.Payments
                .Where(p => p.Id == paymentId)
                .ExecuteUpdateAsync(setters => 
                    setters.SetProperty(p => p.Status, status));

            return affectedRows > 0;
        }
    }
}

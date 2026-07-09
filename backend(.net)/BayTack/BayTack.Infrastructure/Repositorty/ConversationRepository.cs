using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Features.Messages.Common;
using BayTack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using BayTack.Domain.Entities;
using BayTack.Domain.Entities.Messaging; // adjust to your actual namespace

namespace BayTack.Infrastructure.Repositorty
{
    public sealed class ConversationRepository : IConversationRepository
    {
        private readonly AppDbContext _context;

        public ConversationRepository(AppDbContext context) => _context = context;

        public async Task<List<ConversationSummaryResponse>> GetConversationsForCustomerAsync(
            string customerId, CancellationToken ct = default)
        {
            var rows = await _context.Set<Conversation>()
                .AsNoTracking() 
                .Where(c => c.CustomerId == customerId)
                .OrderByDescending(c => c.LastMessageAt)
                .Select(c => new
                {
                    c.Id,
                    c.ProviderId,
                    ProviderName = _context.Users
                        .Where(u => u.Id == c.ProviderId)
                        .Select(u => u.FullName)
                        .FirstOrDefault(),
                    LastMessage = c.Messages
                        .OrderByDescending(m => m.SentAt)
                        .Select(m => m.Text)
                        .FirstOrDefault(),
                    c.LastMessageAt,
                    UnreadCount = c.Messages.Count(m => m.SenderId != customerId && !m.IsRead)
                })
                .ToListAsync(ct);

            // NOTE: avatar isn't a real column on AppUser yet (same gap flagged in
            // NotificationResponse for icons) - returning null until that column exists.
            return rows
                .Select(r => new ConversationSummaryResponse(
                    r.Id,
                    r.ProviderId,
                    r.ProviderName ?? "Unknown provider",
                    null,
                    r.LastMessage,
                    r.LastMessageAt,
                    r.UnreadCount))
                .ToList();
        }

        public async Task<string?> GetDisplayNameAsync(string userId, CancellationToken ct = default) =>
            await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.FullName)
                .FirstOrDefaultAsync(ct);
    }
}
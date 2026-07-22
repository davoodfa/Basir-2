using Basir.Application.Globalization.DTOs;
using Basir.Domain.Common;

namespace Basir.Application.Globalization.Interfaces;

public interface IUserPreferenceService
{
    Task<UserPreferenceDto?> GetAsync(Guid userId, CancellationToken ct = default);
    Task<Result<UserPreferenceDto>> UpdateAsync(Guid userId, UserPreferenceDto dto, CancellationToken ct = default);
    Task<Result> CreateDefaultAsync(Guid userId, CancellationToken ct = default);
}

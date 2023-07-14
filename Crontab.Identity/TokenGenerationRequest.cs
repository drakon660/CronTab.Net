namespace Crontab.Identity;

public record TokenGenerationRequest(string Email, Guid UserId);
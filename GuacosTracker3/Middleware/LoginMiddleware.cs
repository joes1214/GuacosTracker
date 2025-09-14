using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GuacosTracker3.Models;
using GuacosTracker3.Areas.Identity.Data;
using GuacosTracker3.Data;

namespace GuacosTracker3.Middleware
{
    public class LoginMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoginMiddleware> _logger;

        public LoginMiddleware(RequestDelegate next, ILogger<LoginMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }

            try
            {
                Console.WriteLine("=== ALL CLAIMS ===");
                foreach (var claim in context.User.Claims)
                {
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }
                Console.WriteLine("=================");
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var email = context.User.FindFirst("email")?.Value;
                var name = context.User.FindFirst("nickname")?.Value;

                _logger.LogInformation("Auth0 User Sync - ID: {UserId}, Email: {Email}", userId, email);

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Authenticated user has no NameIdentifier claim");
                    await _next(context);
                    return;
                }

                var user = await userManager.Users
                    .FirstOrDefaultAsync(u => u.Auth0Id == userId);

                // If not found by Auth0Id, try by email
                if (user == null && !string.IsNullOrEmpty(email))
                {
                    user = await userManager.FindByEmailAsync(email);
                }

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        Auth0Id = userId,
                        UserName = name,
                        Email = email,
                        CreatedAt = DateTime.UtcNow,
                    };

                    var createResult = await userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                    {
                        _logger.LogError("Failed to create user: {Errors}",
                            string.Join(", ", createResult.Errors.Select(e => e.Description)));
                        await _next(context);
                        return;
                    }

                    _logger.LogInformation("Created new user for Auth0 ID: {Auth0Id}", userId);
                }
                else
                {
                    if (user.Auth0Id != userId)
                    {
                        user.Auth0Id = userId;
                    }

                    if (user.Email != email)
                    {
                        user.Email = email;
                        user.EmailConfirmed = true;

                        var updateResult = await userManager.UpdateAsync(user);
                        if (!updateResult.Succeeded)
                        {
                            _logger.LogError("Failed to update user: {Errors}",
                                string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                        }
                        else
                        {
                            _logger.LogInformation("Updated user record for Auth0 ID: {Auth0Id}", userId);
                        }
                    }
                }

                context.Items["CurrentUser"] = user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LoginMiddleware");
            }

            await _next(context);
        }
    }
}
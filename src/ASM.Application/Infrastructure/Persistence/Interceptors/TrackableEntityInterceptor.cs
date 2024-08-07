﻿using System.Security.Claims;
using ASM.Application.Common.SeedWorks;
using ASM.Application.Domain.IdentityAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ASM.Application.Infrastructure.Persistence.Interceptors;

public sealed class TrackableEntityInterceptor(IHttpContextAccessor accessor) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context is null) return;

        var userId = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null) return;

        var staffId = context.Set<Staff>()
            .Where(staff => staff.Users!.Any(user => user.Id == userId))
            .Select(staff => staff.Id)
            .First();

        foreach (var entry in context.ChangeTracker.Entries<TrackableEntityBase>())
        {
            if (entry.State is not (EntityState.Added or EntityState.Modified) && !entry.HasChangedOwnedEntities())
                continue;

            if (entry.State == EntityState.Added) entry.Entity.CreatedBy = staffId;

            entry.Entity.UpdatedBy = staffId;
        }
    }
}

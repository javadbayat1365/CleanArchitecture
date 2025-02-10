using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence.Extensions;

internal static class ModelBuilderExtensions
{
    public static void RegisterEntities<TEntityType>(this ModelBuilder builder,params Assembly[] assemblies)
    {
       var entityTypes = assemblies
            .SelectMany(s => s.ExportedTypes)
            .Where(s => s is { IsClass:true, IsAbstract:false, IsPublic:true } && typeof(TEntityType).IsAssignableTo(s));

        foreach (var entityType in entityTypes)
        {
            builder.Entity(entityType);
        }
    }

    public static void ApplyRestrictDeleteBehaviour(this ModelBuilder builder)
    {
        var cascadeForeignKeys = builder.Model.GetEntityTypes()
             .SelectMany(s => s.GetForeignKeys())
             .Where(s => !s.IsOwnership && s.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var cascadeForeignKey in cascadeForeignKeys)
        {
            cascadeForeignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}

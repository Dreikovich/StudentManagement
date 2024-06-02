using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace StudentManagementWebApi.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
public class SwaggerExcludeAttribute : Attribute
{
}

public class SwaggerExcludeFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var excludeProperties = context.Type.GetProperties()
            .Where(t => t.GetCustomAttribute<SwaggerExcludeAttribute>() != null);

        foreach (var excludedProperty in excludeProperties)
        {
            var propertyToRemove =
                schema.Properties.Keys.SingleOrDefault(x => x.ToLower() == excludedProperty.Name.ToLower());
            if (propertyToRemove != null) schema.Properties.Remove(propertyToRemove);
        }
    }
}
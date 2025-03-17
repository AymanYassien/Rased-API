using System.Linq.Expressions;

namespace Rased.Api.Controllers.Helper;


public static class ExpressionBuilder
{
    public static Expression<Func<T, bool>>[] ParseFilter<T>(string filter) where T : class
    {
        if (string.IsNullOrWhiteSpace(filter))
            return Array.Empty<Expression<Func<T, bool>>>();

        var filters = new List<Expression<Func<T, bool>>>();
        var parameter = Expression.Parameter(typeof(T), "e");

        // Split by comma for multiple conditions
        var conditions = filter.Split(',', StringSplitOptions.RemoveEmptyEntries)
                               .Select(c => c.Trim());

        foreach (var condition in conditions)
        {
            // Split condition into field, operator, and value (e.g., "Amount > 100")
            var parts = condition.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
                throw new ArgumentException($"Invalid filter condition: {condition}");

            var field = parts[0];
            var op = parts[1];
            var valueStr = parts[2];

            var property = Expression.Property(parameter, field);
            var value = ConvertValue(valueStr, property.Type);
            var constant = Expression.Constant(value);

            Expression body = op switch
            {
                "=" => Expression.Equal(property, constant),
                ">" => Expression.GreaterThan(property, constant),
                "<" => Expression.LessThan(property, constant),
                ">=" => Expression.GreaterThanOrEqual(property, constant),
                "<=" => Expression.LessThanOrEqual(property, constant),
                "!=" => Expression.NotEqual(property, constant),
                _ => throw new ArgumentException($"Unsupported operator: {op}")
            };

            filters.Add(Expression.Lambda<Func<T, bool>>(body, parameter));
        }

        return filters.ToArray();
    }

    private static object ConvertValue(string value, Type targetType)
    {
        try
        {
            if (targetType == typeof(decimal))
                return decimal.Parse(value);
            if (targetType == typeof(DateTime))
                return DateTime.Parse(value); // Adjust format as needed
            if (targetType == typeof(int))
                return int.Parse(value);
            if (targetType == typeof(bool))
                return bool.Parse(value);
            if (targetType == typeof(string))
                return value;

            return Convert.ChangeType(value, targetType);
        }
        catch (Exception ex)
        {
            
            
            throw new ArgumentException($"Cannot convert '{value}' to type {targetType.Name}: {ex.Message}");
        }
    }
}
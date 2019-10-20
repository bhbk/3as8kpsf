using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Bhbk.Lib.DataAccess.EFCore.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static IQueryable<TEntity> Compile<TEntity>(
            this IQueryable<TEntity> query, LambdaExpression lambda)
            where TEntity : class
        {
            if (lambda == null)
                return query;

            var result = lambda.Compile().DynamicInvoke(query);

            try
            {
                return (IQueryable<TEntity>)result;
            }
            catch (Exception)
            {
                throw new EntityFrameworkExtensionCastException(
                    $"The entity: \"{result.GetType().ToString()}\" can not be cast to: \"{typeof(IQueryable<TEntity>).ToString()}\".");
            }
        }

        public static IQueryable<TEntity> Include<TEntity>(
            this IQueryable<TEntity> query,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null)
            where TEntity : class
        {
            if (expressions != null)
                foreach (var expression in expressions)
                    query = query.Include(expression);

            return query;
        }

        //https://github.com/dotnet/efcore/issues/5224
        public static void ValidateEntities(
            this DbContext context)
        {
            var entities = context.ChangeTracker.Entries();

            foreach (var entry in entities)
            {
                var results = new List<ValidationResult>();
                var validationContext = new ValidationContext(entry.Entity);

                if (!Validator.TryValidateObject(entry.Entity, validationContext, results, true))
                {
                    var errors = results.Select(r => r.ErrorMessage).ToList()
                        .Aggregate((message, nextMessage) => message + ", " + nextMessage);

                    throw new EntityFrameworkExtensionValidationException(
                        $"The entity {entry.Entity.GetType().FullName} can not be saved due to error(s): {errors}");
                }
            }
        }
    }

    public class EntityFrameworkExtensionsException : Exception
    {
        public EntityFrameworkExtensionsException(string message)
            : base(message) { }

        public EntityFrameworkExtensionsException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class EntityFrameworkExtensionCastException : EntityFrameworkExtensionsException
    {
        public EntityFrameworkExtensionCastException(string message)
            : base(message) { }
    }

    public class EntityFrameworkExtensionValidationException : EntityFrameworkExtensionsException
    {
        public EntityFrameworkExtensionValidationException(string message)
            : base(message) { }
    }
}

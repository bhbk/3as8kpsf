using Bhbk.Lib.QueryExpression.Primitives;

namespace Bhbk.Lib.QueryExpression.Factories
{
    public static class MessageFactory
    {
        public static string MissingFieldExceptionMessage(string entity, string field)
            => $"The entity '{entity}' does not contain the field '{field}'";

        public static string OperatorExceptionMessage(Operator op)
            => $"The operator: '{op}' is invalid.";

        public static string OperatorNotMappedExceptionMessage(string op)
            => $"Operator not mapped - {op}";

        public static string SkipExceptionMessage(int skip)
            => $"The value for skip: '{skip}' is invalid.";

        public static string TakeExceptionMessage(int take)
            => $"The value for take: '{take}' is invalid.";
    }
}

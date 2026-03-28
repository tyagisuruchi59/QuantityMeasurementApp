namespace QuantityMeasurementAppModel.Models
{
    public enum OperationType
    {
        ADD,
        SUBTRACT,
        MULTIPLY,
        DIVIDE,
        COMPARE,
        CONVERT
    }

    public static class OperationTypeExtensions
    {
        public static string ToLowerString(this OperationType op)
            => op.ToString().ToLower();

        public static OperationType Parse(string value)
        {
            if (Enum.TryParse<OperationType>(value.ToUpper(), out var result))
                return result;
            throw new ArgumentException($"Invalid operation type: {value}");
        }
    }
}
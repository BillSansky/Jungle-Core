namespace Jungle.Conditions
{
    /// <summary>
    /// Defines logical operators for combining condition results.
    /// </summary>
    public enum LogicalOperator
    {
        And,        // All conditions must be true
        Or,         // At least one condition must be true
        Xor,        // Exactly one condition must be true
        Nand,       // Not all conditions are true (inverted AND)
        Nor         // None of the conditions are true (inverted OR)
    }
}

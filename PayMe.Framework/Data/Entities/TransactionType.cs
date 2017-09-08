namespace PayMe.Framework.Data.Entities
{
    public enum TransactionType
    {
        /// <summary>
        /// Credit / Someone owes me
        /// </summary>
        PositiveBalance = 1,
        /// <summary>
        /// Debit / Someone i owe
        /// </summary>
        NegativeBalance = 2
    }
}

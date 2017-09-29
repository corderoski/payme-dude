namespace PayMe.Apps.Data.Entities
{
    public enum TransactionType
    {
        /// <summary>
        /// Payback. Money someone owes me
        /// </summary>
        PositiveBalance = 1,
        /// <summary>
        /// Credit. Money i owe to someone
        /// </summary>
        NegativeBalance = 2
    }
}

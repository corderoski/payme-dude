using System;

namespace PayMe.Framework.Data.Context
{
    public interface IDatabaseContextTransaction
    {
        /// <summary>
        /// Enclose the persistent transaction
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="autoRetry">Given Online SLAs (like Azure) this manages whenever try or not</param>
        /// <returns></returns>
        T ExecTransation<T>(Func<T> action);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayMe.Exceptions
{

    //[System.Serializable]
    public class DataTransactionExecutionException : Exception
    {

        private readonly IEnumerable<object> _entries = null;
        private readonly string _message;

        public DataTransactionExecutionException() { }
        public DataTransactionExecutionException(string message) : base(message) { }
        public DataTransactionExecutionException(string message, Exception inner) : base(message, inner)
        {
            _message = message;
        }
        public DataTransactionExecutionException(IEnumerable<object> entries, string message, Exception inner) : base(message, inner)
        {
            _entries = entries;
            _message = message;
        }

        /// <summary>
        /// Derivated custom message
        /// </summary>
        public override string Message
        {
            get
            {
                var typeNames = _entries != null ? string.Join(",", _entries.Select(p => nameof(p))) : "<none>";
                string message = $"An error ocurried while updating against the data store. {_message}" +
                    Environment.NewLine +
                    "Data: {typeNames}";
                return message;
            }
        }

    }
}

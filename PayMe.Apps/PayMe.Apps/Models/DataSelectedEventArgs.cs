using System;

namespace PayMe.Apps.Models
{
    public class DataSelectedEventArgs : EventArgs
    {
        public object Data { get; }

        public DataSelectedEventArgs(object data)
        {
            Data = data;
        }
    }
}

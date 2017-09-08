using System.Collections.Generic;

namespace PayMe.Framework.Data.DTO
{
    public interface IBaseResult
    {
        bool Succeeded { get; set; }
        IEnumerable<string> Errors { get; set; }
    }
}

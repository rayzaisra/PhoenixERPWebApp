using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Shared.Libraries.SnowFlake
{
    public interface IIdGenerator<T> : IEnumerable<T>
    {
        /// <summary>
        /// Generates new identifier every time the method is called
        /// </summary>
        /// <returns>new identifier</returns>
        T GenerateId();
    }
}

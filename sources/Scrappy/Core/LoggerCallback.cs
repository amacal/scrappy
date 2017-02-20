using System.Collections.Generic;

namespace Scrappy.Core
{
    public interface LoggerCallback
    {
        void OnAll(IEnumerable<dynamic> entries);

        void OnNew(dynamic entry);

        void OnDisposed(dynamic entry);
    }
}
using System.Collections.Generic;

namespace GDPR.MaxMind
{
    public interface IMaxMindService
    {
        Dictionary<string, object> GetData(string IP);
    }
}

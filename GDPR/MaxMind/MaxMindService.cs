using MaxMind.Db;
using System;
using System.Collections.Generic;
using System.Net;

namespace GDPR.MaxMind
{
    public class MaxMindService : IMaxMindService, IDisposable
    {
        private Reader Reader { get; set; }
        
        /// <summary>
        /// Returns the Reader that use our DB file
        /// </summary>
        /// <returns></returns>
        private Reader GetReference()
        {
            if (Reader == null)
            {
                Reader = new Reader("MaxMind/GeoLite2-Country.mmdb");
            }
            return Reader;
        }

        /// <summary>
        /// Returns data of a given IP address
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetData(string IP)
        {
            return GetReference().Find<Dictionary<string, object>>(IPAddress.Parse(IP));
        }

        /// <summary>
        /// Disposes the object
        /// </summary>
        public void Dispose()
        {
            if (GetReference() != null)
            {
                Reader.Dispose();
            }
        }
    }
}
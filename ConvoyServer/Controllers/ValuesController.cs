using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ConvoyServer.Controllers
{
    public class IdController : ApiController
    {
        public static List<string> registeredIds = new List<string>();

        public List<string> Get()
        {
            return registeredIds;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
            if (!registeredIds.Contains(value)){
                registeredIds.Add(value);
            }
        }

        public void Delete()
        {
            registeredIds.Clear();
        }
    }
}

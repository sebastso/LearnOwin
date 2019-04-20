using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace OwinConsole
{
    public class ServiceListController : ApiController
    {
        // GET api/demo 
        public IEnumerable<ServiceList> Get()
        {
            var test = new ServiceList
            {
                id = 1,
                name = "Runner",
                description = "Runner is a ....",
                icon_url = " www.corelis.com/images/runner.png",
                is_active = 1
            };

            return new ServiceList[] { test };
        }

        // GET api/demo/5 
        public string Get(int id)
        {
            return "Hello, World!";
        }

        // POST api/demo 
        public void Post([FromBody]string value)
        {
        }

        // PUT api/demo/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/demo/5 
        public void Delete(int id)
        {
        }
    }
}

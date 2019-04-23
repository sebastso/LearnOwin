using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace OwinConsole
{

    public class DemoController : ApiController
    {
        // GET api/demo 
        [MyCustomAuthAttribute]
        public IEnumerable<string> Get()
        {
            var headerToken = Request.Headers;
            var token = headerToken.Authorization;

            S3Access s3 = new S3Access();
            s3.AWSGetCredentialForIdentity(token.Parameter);
            //s3.ReadObjectDataAsync("sony-bucket", "test_aws.txt", null).Wait();
            return new string[] { "Hello", "World" };
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

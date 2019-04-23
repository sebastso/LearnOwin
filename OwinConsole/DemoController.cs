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
            S3Access s3 = new S3Access();
            s3.AWSGetCredentialForIdentity("eyJraWQiOiIrbmcwYkVnaTlaNTFTQ1pFcUprQzJhRmJ1Z2EyaXNxZXVtU3lcL09VTE5UMD0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiI5NmVhYmMxYy1hMzFlLTQ2NDgtOWIxOS1iNDVhZjAzM2IxZjciLCJhdWQiOiIzaWIwa3BlbDQya2ttNG5oZnZ2cmlxMXZ0ZiIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiZXZlbnRfaWQiOiJmNDBkYjVlMi02NTdjLTExZTktOTYxMC1kZGU3MWRkMDkxNGMiLCJ0b2tlbl91c2UiOiJpZCIsImF1dGhfdGltZSI6MTU1NTk5MjI5NSwiaXNzIjoiaHR0cHM6XC9cL2NvZ25pdG8taWRwLnVzLWVhc3QtMi5hbWF6b25hd3MuY29tXC91cy1lYXN0LTJfMWtYQzBQYTJwIiwiY29nbml0bzp1c2VybmFtZSI6InRlc3R1c2VyMiIsImV4cCI6MTU1NTk5NTg5NSwiaWF0IjoxNTU1OTkyMjk1LCJlbWFpbCI6InNlYkB4eXouY29tIn0.F9drQMRtJCi0kdxr8O42Qv8Pcer3Rr9KBngFfA6fPiguAo_GX3meCYQcxCZrQyrEZOHLjtvX5IxEQPSLJ3dQ2omhVPD-1yrEH4etUae_jKyl5WUoM6RII5i_8uDmqqCBMghGsp_7jq76XaJXYpRxLSJQINE5MPVgBEnz0YE45_OJAZ0weWv4eC9xp10Lpc_iFExpGXPVo8osOmgCwP0-5bEa-bXgmGnadJmlamrfGWSa7XWS-TPuYth_Dk5xZn2yfmnNqvSWAtzjlGwT29av8yz4S1B1KaI6Eyw_U6UGSgRHEkqROakzr99MGvxWADSzITCIMqFGYAH3iLFAFwRYVg");
            s3.ReadObjectDataAsync("sony-bucket", "test_aws.txt", null).Wait();
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

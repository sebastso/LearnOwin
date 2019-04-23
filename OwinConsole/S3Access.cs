using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentity.Model;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinConsole
{

    public class S3Access
    {
        public void AWSGetCredentialForIdentity(string jwtToken)
        {
            var credentials = new CognitoAWSCredentials("us-east-2:264e3c59-f348-43bc-8ee6-870927c4170d", bucketRegion);
            credentials.AddLogin(
                "cognito-idp.us-east-2.amazonaws.com/us-east-2_1kXC0Pa2p",
                jwtToken); // th
            var access = credentials.GetCredentials().AccessKey;
            var secret = credentials.GetCredentials().SecretKey;

            ReadObjectDataAsync("sony-bucket", "start.jpg", credentials).Wait();
        }
        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast2;


        public async Task ReadObjectDataAsync(string bucket, string objectKey, CognitoAWSCredentials cred)
        {
            IAmazonS3 client = null;
            if (cred != null)
                client = new AmazonS3Client(cred, bucketRegion);
            else
                client = new AmazonS3Client(bucketRegion);

            string responseBody = "";
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucket,
                    Key = objectKey
                };
                using (GetObjectResponse response = await client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
                    string contentType = response.Headers["Content-Type"];
                    Console.WriteLine("Object metadata, Title: {0}", title);
                    Console.WriteLine("Content type: {0}", contentType);

                    responseBody = reader.ReadToEnd(); // Now you process the response body.
                    Console.WriteLine("Content : {0}", responseBody);
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
    }
}

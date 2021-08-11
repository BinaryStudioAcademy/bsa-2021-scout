using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Application.Interfaces.AWS;

namespace Infrastructure.AWS
{
    public class S3Uploader : IS3Uploader
    {
        private readonly AmazonS3Client _s3;
        private readonly string _defaultBucket;

        public S3Uploader()
        {
            string keyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            string key = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            string region = Environment.GetEnvironmentVariable("AWS_REGION");

            _defaultBucket = Environment.GetEnvironmentVariable("AWS_DEFAULT_BUCKET");
            _s3 = new AmazonS3Client(keyId, key, RegionEndpoint.GetBySystemName(region));
        }

        public async Task UploadAsync(string path, byte[] fileContent)
        {
            await UploadAsync(_defaultBucket, path, fileContent);
        }

        public async Task UploadAsync(string path, Stream fileContent)
        {
            await UploadAsync(_defaultBucket, path, fileContent);
        }

        public async Task UploadAsync(string bucket, string path, byte[] fileContent)
        {
            await UploadAsync(bucket, path, new MemoryStream(fileContent));
        }

        public async Task UploadAsync(string bucket, string path, Stream fileContent)
        {
            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = bucket;
            request.Key = path;
            request.InputStream = fileContent;

            await _s3.PutObjectAsync(request);
        }
    }
}

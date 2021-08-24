using System.Collections.Generic;

namespace Application.SNS.Dtos
{
    public class S3NotificationObjectDto
    {
        public string Key { get; set; }
    }

    public class S3NotificationInfoDto
    {
        public S3NotificationObjectDto Object { get; set; }
    }

    public class S3NotificationRecordDto
    {
        public S3NotificationInfoDto S3 { get; set; }
    }

    public class S3NotificationDto
    {
        public IEnumerable<S3NotificationRecordDto> Records { get; set; }
    }
}

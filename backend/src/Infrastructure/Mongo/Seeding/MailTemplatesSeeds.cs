using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Infrastructure.Mongo.Seeding
{
    public static class MailTemplatesSeeds
    {
        public static MailTemplate GetDefaultSeed()
        {
            return new MailTemplate
            {
                Id = "6130e8ed3c08bd065627b24e",
                Slug = "default",
                Html = @"<!DOCTYPE html>
<html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <style>
            body {
                width: 100%;
            }
            .container {
                width: 100%;
                padding: 100px 0;
                font-family: Arial;
            }
            .content-wrp {
                margin: 0 auto;
                width: 100%;
                max-width: 700px;
                background-color: #291965D9;
                box-sizing: border-box;
                padding: 60px 80px;
            }
            
            .logo {
                width: 250px;
                max-width: 200px;
                margin-bottom: 30px;
            }
            .content {
                background-color: #F6F7F9;
                width: 100%;
                padding: 15px;
                box-sizing: border-box;
            }
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='content-wrp'>
                <center>
                    <img class='logo' src='https://i.postimg.cc/Kcp5snw2/logo-scout-white.png' />
                </center>
                <div class='content'>
                    {{BODY}}
                </div>
            </div>
        </div>
    </body>
</html>",
            };
        }

        public static IEnumerable<MailTemplate> GetSeeds()
        {
            return new List<MailTemplate>()
            {
                new MailTemplate()
                {
                    Id = "61320543dd59311a13e04492",
                    Slug = "To new applicant",
                    Subject = "Your cv verified",
                    UserCreatedId = "fb055f22-c5d1-4611-8e49-32a46edf58b2",
                    UserCreated = "Lina Baptista",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    VisibilitySetting = 0,
                    Html = "Hi! We are check your cv and your candidacy has been accepted for consideration.",
                    DateCreation = new DateTime(2020, 03, 12)
                },
                new MailTemplate()
                {
                    Id = "613520c8e939ade69627a9cd",
                    Slug = "Testing",
                    Html = "Hello, {vacancy.firstName}. In this letter, you have been given a test task, which must be completed within three days. Good luck.",
                    Subject = "Take our test",
                    UserCreatedId = "057c23ff-58b1-4531-8012-0b5c1f949ee1",
                    UserCreated = "Lana Winters",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    VisibilitySetting = 0,
                    DateCreation = new DateTime(2019, 09, 09)
                },
                new MailTemplate()
                {
                    Id = "613520cde939ade69627a9ce",
                    Slug = "Offer",
                    Subject = "You are hired",
                    Html = "Congratulations! You are hired. We are waiting for you at our office next Monday!",
                    UserCreatedId = "fb055f22-c5d1-4611-8e49-32a46edf58b2",
                    UserCreated = "Lina Baptista",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    VisibilitySetting = 0,
                    DateCreation = new DateTime(2020, 01, 18)
                },
                new MailTemplate()
                {
                    Id = "613520d1e939ade69627a9cf",
                    Slug = "Interview",
                    Subject = "We invite you for an interview",
                    Html = "You are invited for an interview at our office at 12:00.",
                    UserCreatedId = "057c23ff-58b1-4531-8012-0b5c1f949ee1",
                    UserCreated = "Lana Winters",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    VisibilitySetting = 0,
                    DateCreation = new DateTime(2021, 04, 21)
                },
                new MailTemplate()
                {
                    Id = "613520d6e939ade69627a9d0",
                    Slug = "Offer",
                    Subject = "You are hired",
                    Html = "Congratulations! You are hired. We are waiting for you at our office tomorrow!",
                    UserCreatedId = "8be08dba-5dac-408a-ab6e-8d162af74024",
                    UserCreated = "John Constantine",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    VisibilitySetting = 0,
                    DateCreation = new DateTime(2021, 05, 12)
                },
                new MailTemplate()
                {
                    Id = "613520dee939ade69627a9d1",
                    Slug = "Phone screen",
                    Subject = "We invite you for an phone screen",
                    Html = "To confirm your identity, we ask you to call our number at a convenient time for you.",
                    UserCreatedId = "2",
                    UserCreated = "Dominic Torreto",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    VisibilitySetting = 0,
                    DateCreation = new DateTime(2020, 07, 12)
                },
                new MailTemplate()
                {
                    Id = "613520e6e939ade69627a9d2",
                    Slug = "Interview",
                    Html = "The interview will take place in our office. Information about the interview will be sent to you later.",
                    Subject = "We would like to meet you for an interview",
                    UserCreatedId = "057c23ff-58b1-4531-8012-0b5c1f949ee1",
                    UserCreated = "Lana Winters",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    VisibilitySetting = 0,
                    DateCreation = new DateTime(2020, 11, 06)
                },
                new MailTemplate()
                {
                    Id = "613520ebe939ade69627a9d3",
                    Slug = "Tech interview",
                    Html = "You are invited to our office for a technical interview. To clarify the information, call our phone number.",
                    Subject = "We invite you for an tech interview",
                    UserCreatedId = "8be08dba-5dac-408a-ab6e-8d162af74024",
                    UserCreated = "John Constantine",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    VisibilitySetting = 0,
                    DateCreation = new DateTime(2019, 08, 10)
                },
                new MailTemplate()
                {
                    Id = "613520f0e939ade69627a9d4",
                    Slug = "Applied",
                    Subject = "Applied",
                    Html = "Your candidacy has been submitted and accepted for consideration",
                    UserCreatedId = "fb055f22-c5d1-4611-8e49-32a46edf58b2",
                    UserCreated = "Lina Baptista",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    VisibilitySetting = 0,
                    DateCreation = new DateTime(2021, 02, 14)
                },
                new MailTemplate()
                {
                    Id = "613520f6e939ade69627a9d5",
                    Slug = "Phone screen",
                    Html = "To confirm your identity, we ask you to call our number at a convenient time for you.",
                    Subject = "We invite you for an phone screen",
                    UserCreatedId = "057c23ff-58b1-4531-8012-0b5c1f949ee1",
                    UserCreated = "Lana Winters",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    VisibilitySetting = 0,
                    DateCreation = new DateTime(2021, 03, 16)
                },
                new MailTemplate()
                {
                    Id = "613520fae939ade69627a9d6",
                    Slug = "Applied",
                    Html = "Your candidacy has been submitted and accepted for consideration",
                    Subject = "Applied",
                    UserCreatedId = "ac78dabf-929c-4fa5-9197-7d14e18b40ab",
                    UserCreated = "Emery Culhane",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    VisibilitySetting = 0,
                    DateCreation = new DateTime(2020, 09, 04)
                },
                new MailTemplate()
                {
                    Id = "61352224d2348e95b292a402",
                    Slug = "Interview",
                    Html = "The interview will take place in our office. Information about the interview will be sent to you later.",
                    Subject = "We would like to meet you for an interview",
                    UserCreatedId = "2",
                    UserCreated = "Dominic Torreto",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    VisibilitySetting = 0,
                    DateCreation = new DateTime(2021, 05, 04)
                },
                new MailTemplate()
                {
                    Id = "61352228d2348e95b292a403",
                    Slug = "Interview",
                    Html = "The interview will take place in our office. Information about the interview will be sent to you later.",
                    Subject = "We would like to meet you for an interview",
                    UserCreatedId = "ac78dabf-929c-4fa5-9197-7d14e18b40ab",
                    UserCreated = "Emery Culhane",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    VisibilitySetting = 0,
                    DateCreation = new DateTime(2019, 12, 12)
                },
            };
        }
    }
}
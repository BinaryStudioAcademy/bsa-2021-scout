using Domain.Entities;

namespace Infrastructure.Mongo.Seeding
{
    public static class MailTemplatesSeeds
    {
        public static MailTemplate GetSeed()
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
    }
}
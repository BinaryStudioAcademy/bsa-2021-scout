## Applicant Tracking System

todo: add some project description

### Useful links

- Pay attention, that we have certain [quality criteria](https://github.com/BinaryStudioAcademy/quality-criteria/blob/production/source/dotnet.md), which we should follow during application development.

- [Trello board](https://trello.com/invite/b/5po3bvjA/a78b44b8ffafe630205f01629bf005ed/bsa-scout-backlog).

- [Front-end design](https://www.figma.com/file/2z79elGvcUBwPKxjkFDgOO/Project-ProjStagePreparation?node-id=0%3A1)

- Staging.

- Production.

- [Event storming](https://miro.com/app/board/o9J_l6NhsRM=/)

- [Value proposition canvas](https://miro.com/app/board/o9J_l7Mm5LE=/)

### How can I run all this stuff?

#### 1. Using Docker

- Obviously, you have to [install Docker](https://docs.docker.com/get-docker/).

- Run Docker Desktop application. You shouldn't get any errors.

- Create `.env` folder in the root of repository. Copy all files from `.env.example` folder to `.env`

- Open .docker folder in command line and run next command `docker-compose up --build`.

- You should be able to access [backend API specification](http://localhost:5050/swagger) and [frontend app](http://localhost:4200/)

- If something went wrong, use [this](https://gumoreska.in.ua/otche-nash-ukrayinskoyu-tekst-molytvy/) link to fix your problem

`tips:`

- If you want to run application services (like database) and don't want to waste time installing them, run this command in .docker folder
`docker-compose -f docker-compose.services.yml up --build`.

- If you want to test production environment localy, do next steps:

  1 Change `ASPNETCORE_ENVIRONMENT=Development` line to `ASPNETCORE_ENVIRONMENT=Production` in .env\ats_api.env file

  2 Go to `backend\src\WebAPI\Extensions\CorsExtenstion.cs` file, find method `AddProductionCorsPolicies` and follow described instructions.

#### 2. Manually

- Go to `backend/src/WebAPI/Properties`.

- Create file `launchSettings.json`.

- Copy content of `example.launchSettings.json` to `launchSettings.json`.

### How to get AWS credentials

- Create AWS account https://portal.aws.amazon.com/billing/signup#/start
- Sign in to the AWS console as an IAM user with admin rights (for example Root user) https://signin.aws.amazon.com/signin
- Create new user with **Programmatic access** using tutorial - https://docs.aws.amazon.com/IAM/latest/UserGuide/id_users_create.html
    - note: best practice is to follow [The Principle of Least Privileges](https://kirkpatrickprice.com/blog/best-practices-for-privilege-management-in-aws/). I recommend use AmazonS3FullAccess policy for your user.

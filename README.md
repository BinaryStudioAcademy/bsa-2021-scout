## Applicant Tracking System

todo: add some project description

### How can I run all this stuff?

#### 1. Using Docker
##### If you want to run application services (like database) and don't want to waste time installing them, 

- Obviously, you have to [install Docker](https://docs.docker.com/get-docker/).

- Run Docker Desktop application. You shouldn't get any errors.

- Create `.env` folder in the root of repository. Copy all files from `.env.example` folder to `.env`

- Open .docker folder in command line and run next command `docker-compose up --build`.

- You should be able to access [backend API specification](http://localhost:5050/swagger) and [frontend app](http://localhost:4200/)

# ThreadPilotTest

## Prerequisites

Docker desktop
Java
liquibase (https://www.liquibase.com/download-oss) both java and liquibase is included using the link

## Installation

create docker image for postgres database

> docker pull postgres

initiate container for postgres

> docker run --name thread-pilot-db -e POSTGRES_PASSWORD=threadpilotpwd -p 5432:5432 -d postgres

## Architecture

### General architecture

I went for clean architecture with a domain and infractructure projects. Together with a feature architecture for the webapplications

### Database first approach

I decided to go for a Postgres relational database. However for this small assignment a Mongo db instance would have been egnuf
I decided to go for database first approch since it might be better suited for BI soultions that might be build upon the application database.
My approach gives developers full control over the database and it still enables the flecibility that can be found in Code first approach.

### Liquibase

I descided to use Liquibase for versioning och the database schema. There are many similar tools but this one is one of the better ones in mny opinion.

### Reverse Poco

Creates Poco entities form the database tables and gives you teh flexibility of code first

### Error handling / validation

I have previously workt with fluent validation and it has suited me well. However I saw that one of my packages that I use in this solution is marked as depricated and should not be used. But i did not have the time to read into all changes in the new package for this solution.

### Security

I have not implemented any security when it comes to authentication and authorization since that would make it more complex for you to execute my solution.

If I would have implemented security I would have gone for Azure Entra solution with roles or policy depending on the requiments. The users should then need to apply for either a client/secret or a user in my Azure subscription

## Personal reflection

I have been working for VCC with an application handeling car accidents that used alot of integrations bot twith the crached car and also integration to SOS and insurance companies in order to build more safe cars.

The biggest cahllenge for me in this test was accually to find the time needed to complete it.

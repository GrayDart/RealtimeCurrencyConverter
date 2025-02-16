# CurrencyConverter

CurrencyConverter is a Web API (RESTful) application built with C# and .Net 9 that fetches real-time currency exchange rates from the Frankfurter API. The API allows users to pass different currencies and calculate the conversion rates between them. It also includes features like Authentication, Authorization, API versioning, Circuit Breaker, Retry Mechanism and IP Rate Limiting.

## Features

- Real-time currency exchange rates
- Conversion between different currencies (With Symbols Limit)
- Authentication
- Authorization
- API versioning
- Circuit Breaker
- Retry Mechanism
- IP Rate Limiting
- Error handling
- Swagger UI

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- .NET 9
- Visual Studio 2022

### Installing

1. Clone the repository
2. Open the solution in Visual Studio
3. Build the solution to restore Nuget packages
4. Run the application

## Usage

1. Call Login API to Get the Authentiation Token (Default login credential, ccadmin@graydart.com | Qwerty123!)
2. the currency you want to convert from
2. Enter the amount you want to convert
3. Select the currency you want to convert to
4. The application will display the converted amount

## Acknowledgments

- Currency Converter API for providing real-time currency exchange rates

## API Documentation

- find 'api-documentation.pdf' for complete API Swagger documentation (API Version 1.0)

Example:

LOGIN:
curl --location '[BASE-URL]/api/v1/Login' \
--header 'Content-Type: application/json' \
--data-raw '{
  "email": "ccadmin@graydart.com",
  "password": "Qwerty123!"
}'

RES:
{"statusCode":100,"message":"Action Succeeded","data":{"token":"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTczOTcxMzI1MywiYXVkIjoiQ3VycmVuY3lFeGNoYW5nZXIifQ.i0B-TYGsi9HP8yXCcbk_ewV-7qqq8DQb7vo43RbEIGI"}}

CONVERTER:
curl --location '[BASE-URL]/api/v1/Convert/limit' \
--header 'Content-Type: application/json' \
--header 'Authorization: ••••••' \
--data '{
  "amount": 5,
  "base": "EUR",
  "symbols": [
    "BGN",
    "USD"
  ]
}'

RES:
{"statusCode":100,"message":"Action Succeeded","data":{"base":"EUR","amount":5.0,"rates":[{"currency":"BGN","rate":1.9558,"convertedAmount":9.7790},{"currency":"USD","rate":1.0478,"convertedAmount":5.2390}]}}


## Possible Feature Enhancement

1. Can convert this API into Microservice Architecture (into module based service : SSO and Profile Management, Realtime Conversion and Reporting & History)
2. Instead of fetching Exchange rate from runtime API call to third-party, we can integrate background service (which will run on the OS as service) and fetch the latest exchange rate by given time period and store it in Database.
2. Integrate Advance caching with Redis or any other NoSQL providers
3. Integrate Advance IP Rate Limiting and Client Rate Limiting
4. Integrate customer Middleware (Custom middlewares)
5. Advance Logging Techniques
6. Integrate SQL, ORACLE, MySQL (EF) for API Auditing
7. Can Integrate Any Third-Party Exchange Rate provider (just adding implementation same as FrankFutureApi)
8. Docker Deployment with advance features
9. DevOps using Kubetnetes
10. Advance testing (functional testing, unit testing, integration testing)



## SQLite Configuration
DB Migration: dotnet ef migrations add InitialCreate --project ./ --startup-project ../CC_API

DB Update: dotnet ef migrations database update --project ./ --startup-project ../CC_API

insert into Users (email, Password,name,Role,CreatedAt,IsActive) values ('ccadmin@graydart.com','Qwerty123!','Admin','Admin',datetime('now'),1)
insert into Users (email, Password,name,Role,CreatedAt,IsActive) values ('ccstaff@graydart.com','Qwerty123!','Staff','Staff',datetime('now'),1)

----------------------------------------------------
----------- BUILD, RUN, DEPLOY ---------------------
----------------------------------------------------
## Docker Build and Run
BUILD: CC_API> docker build -t currencyconverter:v1 -f Dockerfile .
RUN: CC_API> docker run -f --rm -p 20000:80 currencyconverter .

## Run using dotnet runtime
RESTORE PACKAGE: CC_API> dotnet restore
BUILD: CC_API> dotnet build
RUN: CC_API> dotnet run

## Kubernetes (Kubectl with minikube)
> kubectl apply -f deployment.yml
> kubectl get deploy (kubectl get nodes > to check number of nodes configured on deployment yml)

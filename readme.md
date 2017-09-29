# README


This repository is a test project putting into use the platform Azure Mobile Services allowing its implementation from relatively any platform capable of implementing Mobile Services.

Configurations such as Authorization and Identity Providers are handled directly from the Azure Portal.

You can check out the [webtask.io Simple Demo](https://jcorderodr.github.io/payme-wt.html) of this backend.


## Stack

- Autofac
- xUnit
- EntityFramework
- Web API 2.2
- Azure Mobile Services
- Xamarin Forms

## Roadmap

- Update Tests

- Share debts + Custom messages

- WPF Client

- Xamarin iOS & UWP Clients

- Push Notifications

## Solution

### Services

- PayMe.Services.WebApi : Table and Custom API Controllers [.NET Framework 4.6.2]


### Framework

- PayMe.Framework : This is where the business logic resides. Contains Entities, DTO, Contexts and Services. 


### Data

- PayMe.Database : Contains the Database objects in case you want to publish the schema before running it.


### Tests

- PayMe.Tests.Services : Web API, Data and Framework testig


## Contributions

If you're willing to collaborate, feel free doing so by making a PR with your changes!
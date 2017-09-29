# README

Demo tool for managing user's debts integrating a .NET Backend with a Xamarin Forms frontend.


This repository is a test project putting into use the Azure Mobile Services platform allowing its implementation from relatively any platform capable of implementing Mobile Services.

Configurations such as Authorization and Identity Providers are handled directly from the Azure Portal.


## Stack

- Autofac
- xUnit
- EntityFramework
- Web API 2.2
- Azure Mobile Services
- Xamarin Forms

## Roadmap

- Update Tests

- Share debts +Custom messages

- WPF

- Xamarin iOS & UWP projects

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
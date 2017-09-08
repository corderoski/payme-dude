# README

This repository is a test project putting into use the platform Azure Mobile Services allowing its implementation from relatively any platform capable of implementing Mobile Services.

Configurations such as Authorization and Identity Providers are handled directly from the Azure Portal.


You can check out the [webtask.io Simple Demo](http://jcordero.azurewebsites.net/payme-wt.html) of this backend.


## Stack

- Autofac
- EntityFramework
- Web API 2.2
- Azure Mobile Services

## Solution

### Services

- PayMe.Services.WebApi : Table and Custom API Controllers [.NET Framework 4.6.2]


### Framework

• PayMe.Framework : This is where the business logic resides. Contains Entities, DTO, Contexts and Services. 


### Data

- PayMe.Database : Contains the Database objects


### Tests

- PayMe.Tests.Services : Web API

- PayMe.Tests.Framework: Data and Framework testig


## Roadmap

- Update / Expand Tests

- Custom JWT generation (on demand)

- Push Notifications

- WPF

- Xamarin Android & iOS projects

## Contributions

If you're willing to collaborate, feel free doing it by making a fork of this repository and sent us your changes!

The GeoDataApi 

GeoDataApi serves as an abstraction layer between the GIS environment and applications that consume GIS data. 
This version was built in Fall 2019 and replaces another API that was built to work with an older GIS 
environment. Its purpose is to minimize coding changes in the consuming applications should the GIS environment be upgraded, changed,
or replaced. 

It also is meant to provide failover mechanisms in case the GIS locator services fail for any reason, or if they don't return
data from the parameters provided by the user. The database calls in this API provide an alternative way of retrieving data.

This mixed approach is partly because of initial discussions surrounding the design of the API. Initially the intention was to avoid the
use of the database and exclusively use the locator services, but this approach was problematic. The API therefore uses both. 

One of the goals of this project was to build an API in a simple and flexible manner. It's not currently known all the ways this API will
be used, so developers should feel free to refactor, extend and modify this API as necessary.

Summary of the APIs currently in the solution:

AddressCandidates:
The AddressCandidates API is simply a wrapper around the findAddressCandidates GIS locator service. It is consumed by the following 
applications:
VoterRegistration
ElectionAdministration
SuperCanInventory

MapServer:
The MapServer API accepts the x and y coordinates returned by the AddressCandidates service in the location object. The StAddr property
returned by AddressCandidates is what is passed to the streetAddress parameter of MapServer. MapServer is consumed by the following 
applications:
VoterRegistration


SearchAddress:
SearchAddress is meant to be consumed by applications with autocomplete/type-ahead features. The Empower suite of applications consumes
this API. 
VoterRegistration

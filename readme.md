The GeoDataApi 

GeoDataApi serves as an abstraction layer between the GIS environment and applications that consume GIS data. 
This version was built in Fall 2019 and replaces another API that was built to work with an older GIS 
environment. Its purpose is to minimize coding changes in the consuming applications should the GIS environment be upgraded, changed,
or replaced. 

One of the goals of this project was to build an API in a simple and flexible manner. It's not currently known all the ways this API will
be used, so developers should feel free to refactor, extend and modify this API as necessary.

With the exception of the AddressCandidates API, which is a wrapper around the findAddressCandidates locator service, this API
goes directly to the database for data, rather than using the GIS Server APIs that are provided out of the box. This is because the SQL
views being called represent the most up-to-date address data created by the Planning office, whereas the CommonBoundaries API accesses data
from the Assessor's office, which is not as accurate. For this reason, it was decided to go to the database for most of the information.

Summary of the APIs currently in the solution:

AddressCandidates:
The AddressCandidates API is simply a wrapper around the findAddressCandidates GIS locator service. It is consumed by the following 
applications:
VoterRegistration
ElectionAdministration
SuperCanInventory

MapServer:
The MapServer API accepts the StAddr property returned by AddressCandidates. The query returns data to the following applications:
VoterRegistration


SearchAddress:
SearchAddress is meant to be consumed by applications with autocomplete/type-ahead features. It is used by the following applications:
VoterRegistration
Empower

# ResQ

Emergency Response Coordination backend platform. Report incidents, auto calculate their priority, and option for auto dispatch the closest available team. Built with .NET 10 and Onion Architecture.

## Features
- Full CRUD on every entity: locations, emergency services, response teams, vehicles, operators, incidents, deployments, status updates
- Automatic priority calculation - incident type, casualties and live weather feed into a Low / Medium / Critical score
- Automatic team assignment - ranks available teams by real world distance (haversine) and matches them with available vehicle
- Weather ETL pipeline - a background job extracts live weather from Open-Meteo API, transforms it, and loads it into the database every 30 minutes
- Geocoding on demand - new locations are resolved from a street address via OpenStreetMaps Nominatim API, with existing addresses reused instead of regeocoded
- Async email queue - dispatch notifications are queued in memory and sent by a background worker
- Excel export - incident and team performance reports as downloadable .xlsx files

## Tech stack
- .NET 10 / ASP.NET Core Web API
- EF Core + SQLite
- MailKit for SMTP email
- ClosedXML for Excel export
- Open-Meteo and OpenStreetMap Nominatim - free, key-less external APIs

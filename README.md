# WeatherAppViaJSONApi

A Windows Forms application written in Visual Basic .NET that automatically determines the user’s geographic location via IP based geolocation and retrieves real time weather data using public JSON APIs.

This project demonstrates API integration, JSON deserialization, application state persistence, and UI data presentation within a classic desktop application environment.

---

## Overview

WeatherAppViaJSONApi is designed to provide local weather conditions without requiring manual location input. On first launch, the application determines the user’s approximate latitude and longitude using their public IP address. That location data is persisted in application settings and reused on subsequent launches to reduce unnecessary API calls.

Once location data is available, the application queries the OpenWeatherMap API to retrieve current weather conditions, forecast ranges, wind information, and system level metadata, presenting both a summarized view and raw diagnostic data.

---

## Features

- Automatic geolocation using IP based lookup
- Persistent storage of latitude and longitude using application settings
- Real time weather data retrieval from OpenWeatherMap
- JSON deserialization using `JavaScriptSerializer`
- Detailed weather and system diagnostics display
- Dynamic weather icon loading based on API response
- Simple and clean WinForms user interface

---

## APIs Used

- **IP Geolocation**  
  `http://ip-api.com/json`  
  Used to retrieve approximate geographic location and network metadata.

- **Weather Data**  
  `https://openweathermap.org/api`  
  Used to retrieve current weather conditions and related metrics.

---

## Technologies

- Visual Basic .NET
- Windows Forms
- HTTP Web Requests
- JSON Serialization and Deserialization
- Application Settings Persistence

---

## Configuration

To run this project successfully, you must supply your own OpenWeatherMap API key.

1. Register for a free API key at:  
   https://openweathermap.org/api

2. Replace the placeholder API key in the application code:

```vb
Public api_key As String = "&APPID=YOURAPIKEY"

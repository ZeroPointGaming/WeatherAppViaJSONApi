Imports System.Web.Script.Serialization
Imports System.IO
Imports System.Net

Public Class Form1
    'Location Data
    Public lat As String = My.Settings.Latitude
    Public log As String = My.Settings.Longitude

    'Retrieve location data via ip address
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RefreshLocationData()

        If My.Settings.Latitude = Nothing And My.Settings.Longitude = Nothing Then
            Try
                'Query location data
                Dim Req As HttpWebRequest
                Dim Ret As HttpWebResponse = Nothing
                Dim SR As StreamReader
                Req = DirectCast(WebRequest.Create("http://ip-api.com/json"), HttpWebRequest)
                Ret = DirectCast(Req.GetResponse(), HttpWebResponse)
                SR = New StreamReader(Ret.GetResponseStream())
                Dim Raw As String = Nothing
                Raw = SR.ReadToEnd()
                Dim JavaScriptSerialization As New JavaScriptSerializer()
                Dim ipdata_object As New IPData()

                ipdata_object = JavaScriptSerialization.Deserialize(Raw, ipdata_object.GetType)

                lat = ipdata_object.lat
                log = ipdata_object.lon
                My.Settings.Latitude = lat
                My.Settings.Longitude = log
                My.Settings.Save()
            Catch ex As Exception
                MessageBox.Show(ex.ToString())
            End Try
        Else
            lat = My.Settings.Latitude
            log = My.Settings.Longitude
        End If

        CollectWeatherData(lat, log)
    End Sub

    'generate your own free api key at openweathermap.org to use with your own project.
    Public api_key As String = "&APPID=a756bb2cdd548defd8539e8e36c1d2df"
    Public api_measurement As String = "imperial"

    Public weather_url As String = "http://api.openweathermap.org/data/2.5/weather?lat=" + My.Settings.Latitude + "&lon=" + My.Settings.Longitude + "&APPID=a756bb2cdd548defd8539e8e36c1d2df&units=imperial"

    'Refresh Location Data
    Public Sub RefreshLocationData()
        ListBox2.Items.Clear()

        'Query location data
        Dim Req As HttpWebRequest
        Dim Ret As HttpWebResponse = Nothing
        Dim SR As StreamReader
        Req = DirectCast(WebRequest.Create("http://ip-api.com/json"), HttpWebRequest)
        Ret = DirectCast(Req.GetResponse(), HttpWebResponse)
        SR = New StreamReader(Ret.GetResponseStream())
        Dim Raw As String = Nothing
        Raw = SR.ReadToEnd()
        Dim JavaScriptSerialization As New JavaScriptSerializer()
        Dim ipdata_object As New IPData()

        ipdata_object = JavaScriptSerialization.Deserialize(Raw, ipdata_object.GetType)

        ListBox2.Items.Add("----- Coordinates -----")
        ListBox2.Items.Add("Latitude: " + ipdata_object.lat)
        ListBox2.Items.Add("Longitude: " + ipdata_object.lon)
        ListBox2.Items.Add("")

        ListBox2.Items.Add("----- Location -----")
        ListBox2.Items.Add("Country: " + ipdata_object.country)
        ListBox2.Items.Add("City: " + ipdata_object.city)
        ListBox2.Items.Add("Zip Code: " + ipdata_object.zip)
        ListBox2.Items.Add("")

        ListBox2.Items.Add("----- User Data -----")
        ListBox2.Items.Add("ISP: " + ipdata_object.isp)
        ListBox2.Items.Add("ORG: " + ipdata_object.org)
        ListBox2.Items.Add("Timezone: " + ipdata_object.timezone)
        ListBox2.Items.Add("IP: " + ipdata_object.query)
        ListBox2.Items.Add("")

        ListBox2.Items.Add("----- Region Data -----")
        ListBox2.Items.Add("Country Code: " + ipdata_object.countryCode)
        ListBox2.Items.Add("Region Name: " + ipdata_object.regionName)
        ListBox2.Items.Add("Region: " + ipdata_object.region)
    End Sub

    'collect data 
    Public Sub CollectWeatherData(latitude As String, longitude As String)
        Try
            'Reset application
            ListBox1.Items.Clear()

            'Query weather data
            Dim Req As HttpWebRequest
            Dim Ret As HttpWebResponse = Nothing
            Dim SR As StreamReader
            Req = DirectCast(WebRequest.Create(weather_url), HttpWebRequest)
            Ret = DirectCast(Req.GetResponse(), HttpWebResponse)
            SR = New StreamReader(Ret.GetResponseStream())
            Dim Raw As String = Nothing
            Raw = SR.ReadToEnd()
            Dim JavaScriptSerialization As New JavaScriptSerializer()
            Dim weatherdata As New WeatherResponse()

            weatherdata = JavaScriptSerialization.Deserialize(Raw, weatherdata.GetType)

            'populate the labels with data
            Label1.Text = "Weather: " + weatherdata.weather(0).main
            Label4.Text = "Geographic Coordinates: " + lat + " , " + log
            Label5.Text = "Tempurature: " + weatherdata.main.temp.ToString() + "°"
            Label6.Text = "Tempurature Forecast: " + "High " + weatherdata.main.temp_max.ToString() + "°" + " / " + "Low " + weatherdata.main.temp_min.ToString() + "°"

            'set application icon
            Me.Icon = DirectCast(My.Resources.ResourceManager.GetObject("_" + weatherdata.weather(0).icon.ToString()), Icon)

            'Populate raw weather data
            ListBox1.Items.Add("----- Weather Data -----")
            ListBox1.Items.Add("Forecast: " + weatherdata.weather(0).main)
            ListBox1.Items.Add("Tempurature: " + weatherdata.main.temp + "° F")
            ListBox1.Items.Add("Wind Speed: " + weatherdata.wind.speed + " mph")
            ListBox1.Items.Add("Wind Direction: " + weatherdata.wind.deg + "°")
            ListBox1.Items.Add("")

            ListBox1.Items.Add("----- Systemic Data -----")
            ListBox1.Items.Add("Weather ID: " + weatherdata.weather(0).id)
            ListBox1.Items.Add("Weather Icon: " + weatherdata.weather(0).icon)
            ListBox1.Items.Add("Country Code: " + weatherdata.sys.country)
            ListBox1.Items.Add("Sunrise Data:  " + weatherdata.sys.sunrise)
            ListBox1.Items.Add("Sunset Data: " + weatherdata.sys.sunset)
            ListBox1.Items.Add("Response ID: " + weatherdata.id)

        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub
End Class

'IP Data API Deserialization classes for http://ip-api.com/json API response
<Serializable>
Class IPData
    Public asinfo As String
    Public city As String
    Public country As String
    Public countryCode As String
    Public isp As String
    Public lat As String
    Public lon As String
    Public org As String
    Public query As String
    Public region As String
    Public regionName As String
    Public status As String
    Public timezone As String
    Public zip As String
End Class

'Weather API Deserialization classes for openweathermap.org API response
<Serializable>
Class WeatherResponse
    Public coord As CoordinateInfo
    Public weather As WeatherInfo()
    Public base As String
    Public main As MainInfo
    Public visibility As String
    Public wind As WindInfo
    Public clouds As CloudInfo
    Public dt As String
    Public sys As SysInfo
    Public id As String
    Public name As String 'Location Name
    Public cod As String
End Class

<SerializableAttribute>
Class CoordinateInfo
    Public lon As String
    Public lat As String
End Class

<SerializableAttribute>
Class WeatherInfo
    Public id As String
    Public main As String 'Weather Type
    Public description As String 'Weather Description
    Public icon As String
End Class

<SerializableAttribute>
Class MainInfo
    Public temp As String
    Public pressure As String
    Public humidity As String
    Public temp_min As String
    Public temp_max As String
End Class

<SerializableAttribute>
Class WindInfo
    Public speed As String 'Wind Speed
    Public deg As String 'Wind Heading
End Class

<SerializableAttribute>
Class CloudInfo
    Public all As String
End Class

<SerializableAttribute>
Class SysInfo
    Public type As String
    Public id As String
    Public message As String
    Public country As String
    Public sunrise As String
    Public sunset As String
End Class
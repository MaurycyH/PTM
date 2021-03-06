## General info
This project was created by 4 people:
[DawidDiaco](https://github.com/DawidDiaco)
[LeonPazdur](https://github.com/PazleoJGC)
[MaurycyH](https://github.com/MaurycyH)
Tomasz K.

The purpose of this program is to help you organize your free time and plan your day.
You can log in with Google or Microsoft account. After you successfull log in you can create a new
Task with given time frame, and it will display on your schedule tab.


The application is created in the MVVM pattern, it uses a database created in the code-first approach in the Entity Framework.

My contribution to the creation of the application is:

Creation of basic database entities and their relations

Authentication of users through Microsoft

Download and save user's avatar from Google and Microsoft

Implementing own top bar using ChromeWindow and handling Aero snap events

Writing some simple unit tests

Making WiX installer
	
## Technologies
Project is created with:
* WPF
* NET core 3.1
* Entity Framework Core: 3.1
* MS Graph API
* WiX Installer
	
## Setup
To run this project you need to pass your own database string connection and own OAuth2.0 Tokens to authenticate user.
You can find connectionstring and Microsoft Client Id inside App.config file in PTM.Logic project. Google token you can change inside GoogleAuthentication.cs file.

```
  <connectionStrings>
    <add name="PtmConnection" providerName="System.Data.SqlClient" connectionString="Data Source=*IP*;Initial Catalog=PTM.Dev;Persist Security Info=True;User ID=DevServiceAccount;Password=*PASS*"/>
    <add name="MicrosoftClientId" connectionString="*TOKEN*"/>
  </connectionStrings>
  
```
## Screenshots
Below I am pasting sample screenshots from the application.
<p align="center">
  <img src="https://user-images.githubusercontent.com/72604629/115781838-38145300-a3bb-11eb-94f0-e68d7480abb7.png" width="40%" height="60%"/>
  <img src="https://user-images.githubusercontent.com/72604629/115781919-511d0400-a3bb-11eb-8dda-263ce3389e07.png" width="80%" height="80%"/> 
  <img src="https://user-images.githubusercontent.com/72604629/115781915-511d0400-a3bb-11eb-9d05-ed3e03394047.png" width="80%" height="80%"/>
  <img src="https://user-images.githubusercontent.com/72604629/115781911-50846d80-a3bb-11eb-9ea5-cc67659034ee.png" width="80%" height="80%"/> 
</p>

# Survey Application

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

### Introduction
- Survey app gives the opportunity to create/update/delete surveys if the user is logged.
- Under the voice Survey in side bar, are listed all the surveys and the actions.
- If you press the publish button, the survey cannot be modified and an email will be send to the respondets (which are unique identified by their email).
- The respondests will receive an email which informs them to fill the survey by clicking a link without a login. If they have voted once, they cannot vote again. 
- A reminder will be send on the 3 end days to the respondents which have not taken the survey/ or haven't switched it off. 
- The user logged in can add/remove a respondent or another user  under the voice of side bar respectively Respondents and Users.
- The user logged in can change his password on the right top of the menu by clicking over his email under the voice Change profile.
- A user/respondent can change the language on the right top of the menu.

### Installation

The app has two parts Api and Site.
##### Requirements

 - .NET Core 3.1
 - Node.js and npm
 - Angular V10
 - Docker v19.03.13 or above

#### Running
The app will be served on https://localhost:4202

#### Prerequisites
- To take the certificates and the web root folder for the images, backup database, you must create under the C:\Users\<your User >\.aspnet 3 folders:


	1. dbs (for the database backup):

		-SurveyDb      
		-Survey_log            
	2. https (for the certificates Angular and .net Core https):

		 -aspnetapp.pfx  
		-certificate.crt
		-privateKey.key     
	3. wwwroot (for the images in the app):

		-Images

### Development

### Docker
Survey app is very easy to install and deploy in a Docker container.

To access the application you can execute from the directory of Api and Site this commands:

```sh
docker-compose build 
docker-compose up 
```
This will create the survey image and pull in the necessary dependencies. 

Verify the deployment by navigating to your server address in your preferred browser.

```sh
https://localhost:4202
```



License
----




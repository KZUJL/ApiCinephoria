# 🎬 Cinephoria Back-end (C# .NET 8)

Cinephoria Back-end est l’API REST de la plateforme Cinephoria.  
Elle gère l’authentification, les réservations, la gestion des films, la base de données MySQL, ainsi que les données liées aux utilisateurs et employés.  

---

## 🚀 Prérequis

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)  
- [MySQL](https://dev.mysql.com/downloads/) (version 8+)  
- [MongoDB](https://www.mongodb.com/try/download/community) (pour la gestion des réservations)  

---

## 📂 Installation

### Cloner le dépôt

```bash
git clone https://github.com/ton-org/CinephoriaApi.git
cd CinephoriaApi

Restaurer les dépendances :
dotnet restore

### Initialisation de la base MySQL

#### Exemple de création des tables MySQL

USE CINEPHORIA;

CREATE TABLE roles (
    roleId INT PRIMARY KEY AUTO_INCREMENT,
    roleName VARCHAR(250) NOT NULL
);

CREATE TABLE users (
    userId INT PRIMARY KEY AUTO_INCREMENT,
    firstName VARCHAR(250) NOT NULL,
    lastName VARCHAR(250) NOT NULL,
    password VARCHAR(255) NOT NULL,
    email VARCHAR(250) NOT NULL,
    roleId INT, 
    userName VARCHAR(250) NOT NULL,
    FOREIGN KEY (roleId) REFERENCES roles(roleId)
);

CREATE TABLE cinemas (
    cinemaId INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(250) NOT NULL,
    address VARCHAR(250) NOT NULL,
    country VARCHAR(250) NOT NULL,
    city VARCHAR(100) NOT NULL
);

CREATE TABLE Rooms (
    roomId INT PRIMARY KEY AUTO_INCREMENT,
    cinemaId INT NOT NULL,
    name VARCHAR(250) NOT NULL,
    quality VARCHAR(50) NOT NULL,
    FOREIGN KEY (cinemaId) REFERENCES Cinemas(cinemaId)
);

CREATE TABLE Locations (
    locationId INT PRIMARY KEY AUTO_INCREMENT,
    roomId INT NOT NULL,
    type VARCHAR(50) NOT NULL,
    name VARCHAR(250) NOT NULL, 
    rowLocation INT NOT NULL,
    columnLocation INT NOT NULL,
    FOREIGN KEY (roomId) REFERENCES Rooms(roomId)
);

CREATE TABLE Incident (
    incidentId INT PRIMARY KEY AUTO_INCREMENT,
    roomId INT NOT NULL,
    locationId INT NOT NULL,
    date DATE NOT NULL,
    description VARCHAR(250) NOT NULL,
    FOREIGN KEY (roomId) REFERENCES Rooms(roomId),
    FOREIGN KEY (locationId) REFERENCES Locations(locationId)
);

CREATE TABLE Movies (
    movieId INT PRIMARY KEY AUTO_INCREMENT,
    releaseDate DATE NOT NULL,
    title VARCHAR(250) NOT NULL,
    genre VARCHAR(250) NOT NULL,
    description TEXT NOT NULL,
    duration TIME NOT NULL,
    poster VARCHAR(255) NULL,
    trailer VARCHAR(255) NULL,
    director VARCHAR(250) NOT NULL,
    producer VARCHAR(250) NOT NULL,
    cast TEXT NOT NULL
);

CREATE TABLE MovieTimes (
    movieTimesId INT PRIMARY KEY AUTO_INCREMENT,
    movieId INT NOT NULL,
    cinemaId INT NOT NULL,
    roomId INT NOT NULL,
    day DATE NOT NULL,
    startTime TIME NOT NULL,
    endTime TIME NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (movieId) REFERENCES Movies(movieId),
    FOREIGN KEY (cinemaId) REFERENCES Cinemas(cinemaId),
    FOREIGN KEY (roomId) REFERENCES Locations(locationId)
);

ces requêtes ont été exécutées pour générer les fichiers SQL fournis dans le dossier Data/. Elles permettent de tester l’API en local avec toutes les relations entre tables correctement configurées.


#### Importation des données
Avant de lancer l’API, il faut créer la base de données et importer les tables fournies :
-- Créer la base 
CREATE DATABASE cinephoria CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

Puis, depuis MySQL ou un client comme MySQL Workbench, importer tous les fichiers SQL du dossier Data :
ApiCinephoria/Data/cinephoria_cinema_schedule.sql
ApiCinephoria/Data/cinephoria_cinemas.sql
ApiCinephoria/Data/cinephoria_incident.sql
ApiCinephoria/Data/cinephoria_locations.sql
ApiCinephoria/Data/cinephoria_movies.sql
ApiCinephoria/Data/cinephoria_movietimes.sql
ApiCinephoria/Data/cinephoria_roles.sql
ApiCinephoria/Data/cinephoria_rooms.sql
ApiCinephoria/Data/cinephoria_users.sql

💡 Astuce : en ligne de commande MySQL :
mysql -u root -p cinephoria < ApiCinephoria/Data/cinephoria_users.sql
(refaire pour chaque fichier, ou créer un script pour tout importer d’un coup)

### Exemple de transaction SQL

Pour montrer l’utilisation de transactions SQL, un fichier `transaction_example.sql` est fourni dans le dossier `Data/Transactions/`.  

Ce fichier contient un exemple de **transaction complète pour l’ajout d’un nouvel employé** dans la table `users` avec vérification du rôle associé.  
L’objectif est de garantir que **tout est exécuté ou rien**, afin de maintenir la cohérence de la base de données.

pour exécuter cette transaction depuis MySQL :

```bash
mysql -u root -p cinephoria < ApiCinephoria/Data/Transactions/transaction_example.sql



### Initialisation de la base MONGO DB
L’API utilise MongoDB pour gérer les reviews et réservations. Pour démarrer en local, voici comment configurer la base avec les données existantes.

1️⃣ Créer la base et les collections
use reservation; // Crée la base reservation si elle n'existe pas

db.createCollection("reviews");
db.createCollection("reservations");

2️⃣ Importer les données existantes
Les fichiers JSON sont fournis dans le dossier Data/ et contiennent toutes les données existantes pour démarrer l’API.

Les fichiers JSON sont :

Data/reviews.json

Data/reservations.json

Pour importer :

mongoimport --db reservation --collection reviews --file Data/reviews.json --jsonArray
mongoimport --db reservation --collection reservations --file Data/reservations.json --jsonArray


### Configuration des secrets avec .NET User Secrets

L’API Cinephoria utilise le gestionnaire de secrets intégré à .NET.  
Les secrets doivent être définis **avant de lancer l’API**, sinon l’application ne pourra pas démarrer.

Exécuter les commandes suivantes depuis la racine du projet (adapter le chemin si besoin) :

```bash
# Base de données MySQL
dotnet user-secrets set "ConnectionStrings:MYSQL_CONNECTION" "Server=localhost;Database=cinephoria;User=root;Password=<YOUR_MYSQL_PASSWORD>" --project ApiCinephoria/ApiCinephoria.csproj

# Base de données MongoDB
dotnet user-secrets set "ConnectionStrings:MONGODB_CONNECTION" "mongodb://<USER>:<PASSWORD>@localhost:27017/?authSource=reservation" --project ApiCinephoria/ApiCinephoria.csproj

Remplacez <YOUR_MYSQL_PASSWORD> et <USER>:<PASSWORD> par vos identifiants locaux.

# Mailjet (remplacer par vos propres clés)
dotnet user-secrets set "Mailjet:MAILJET_APIKEY" "<YOUR_MAILJET_APIKEY>" --project ApiCinephoria/ApiCinephoria.csproj
dotnet user-secrets set "Mailjet:MAILJET_APISECRET" "<YOUR_MAILJET_APISECRET>" --project ApiCinephoria/ApiCinephoria.csproj


Pour vérifier que les secrets ont bien été ajoutés :
dotnet user-secrets list --project ApiCinephoria/ApiCinephoria.csproj
⚠️ Pour tester l’envoi d’emails via Mailjet, vous devez créer un compte Mailjet et utiliser vos propres clés API.

## Lancer l’API en local

1. S'assurer que les secrets sont définis (voir section ci-dessus).
2. Lancer l'API depuis le projet ApiCinephoria :

```bash
dotnet run --project ApiCinephoria/ApiCinephoria.csproj

Ouvrir Swagger pour tester les endpoints de l'API :
https://localhost:7121/swagger/index.html

## Test

Exécuter les tests unitaires :
dotnet test

# Autres dépôts du projet Cinephoria

- [Cinephoria Front-end (Vue.js)](https://github.com/KZUJL/CinephoriaWeb)
- [Cinephoria Back-end (C# .NET)](https://github.com/ton-org/CinephoriaApi)
- [Cinephoria Mobile (Flutter)](https://github.com/KZUJL/CinephoriaMobileApp)
- [Cinephoria Desktop (C#)](https://github.com/KZUJL/CinephoriaDesktop)

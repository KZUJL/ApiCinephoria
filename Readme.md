## Configuration des secrets avec .NET User Secrets

L’API Cinephoria utilise le gestionnaire de secrets intégré à .NET.  
Les secrets doivent être définis **avant de lancer l’API**, sinon l’application ne pourra pas démarrer.

Exécuter les commandes suivantes depuis la racine du projet (adapter le chemin si besoin) :

```bash
dotnet user-secrets set "ConnectionStrings:MYSQL_CONNECTION" "Server=localhost;Database=cinephoria;User=root;Password=Lerodech.29;" --project ApiCinephoria/ApiCinephoria.csproj

dotnet user-secrets set "ConnectionStrings:MONGODB_CONNECTION" "mongodb://userAdmin:md123@localhost:27017/?authSource=reservation" --project ApiCinephoria/ApiCinephoria.csproj

dotnet user-secrets set "Mailjet:MAILJET_APIKEY" "f99041c4dd71202038ef822d5a9fd5c9" --project ApiCinephoria/ApiCinephoria.csproj

dotnet user-secrets set "Mailjet:MAILJET_APISECRET" "4f70572095c71aa9a29c705acb7372e0" --project ApiCinephoria/ApiCinephoria.csproj

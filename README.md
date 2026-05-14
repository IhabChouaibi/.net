# LivraisonMicroservices

Plateforme de gestion des livraisons avec microservices ASP.NET Core, API Gateway Ocelot, authentification JWT et frontend ASP.NET Core MVC premium.

## Rôles

- `Admin` : gestion complète
- `User` : client

Dans l'application, `User = Client`.

## Architecture

- Frontend MVC : `LivraisonFrontend`
- Backend :
  - `Backend/AuthService`
  - `Backend/ClientService`
  - `Backend/LivreurService`
  - `Backend/ColisService`
  - `Backend/VehiculeService`
  - `Backend/PaiementService`
  - `Backend/LivraisonService`
  - `Backend/DashboardService`
  - `Backend/GatewayService`

## Inscription client

Le formulaire d'inscription frontend collecte :

- `Nom`
- `Prenom`
- `Email`
- `Telephone`
- `CIN`
- `Adresse`
- `Ville`
- `CodePostal`
- `Login`
- `Password`
- `ConfirmPassword`

Règles :

- le champ `Role` n'est jamais affiché
- le frontend force toujours `Role = "User"`
- l'inscription crée :
  - un `Compte` dans `AuthService`
  - un `Client` lié via `CompteId` dans `ClientService`

## Authentification

Flux login :

1. `POST /auth/login`
2. récupération du JWT
3. stockage Session :
   - `Token`
   - `Role`
   - `Login`
   - `UserId`
   - `ClientId`
4. redirection :
   - `Admin` -> `/Dashboard/Index`
   - `User` -> `/SuiviColis/Index`

Nouveau comportement :

- la session est entièrement réhydratée après chaque login
- `Logout` exécute `HttpContext.Session.Clear()`
- le profil client reste accessible après logout/login
- `SessionManager` et `AuthSessionHelper` exposent :
  - `GetToken()`
  - `GetRole()`
  - `GetLogin()`
  - `GetUserId()`
  - `GetClientId()`
  - `IsAuthenticated()`

## Fonctionnalités Admin

L'administrateur voit :

- Dashboard
- Clients
- Livreurs
- Colis
- Véhicules
- Paiements
- Livraisons
- Comptes

## Fonctionnalités Client

Le client voit uniquement :

- `Suivi Colis`
- `Mes Colis`
- `Mon Profil`

Le client ne peut pas :

- créer un colis
- modifier un colis
- supprimer un colis
- accéder au dashboard admin
- accéder aux modules d'administration

## Routes gateway utilisées par le frontend

- `/auth`
- `/clients`
- `/colis`
- `/livraisons`
- `/dashboard`

Nouveau support ajouté :

- `GET /clients/by-compte/{compteId}`
- `PUT /clients/by-compte/{compteId}`
- `POST /clients/register`
- `GET /colis/client/{clientId}`

## Gestion des exceptions

Frontend :

- gestion centralisée des erreurs API dans `ApiServiceBase`
- prise en charge des erreurs :
  - `401 Unauthorized`
  - `403 Forbidden`
  - `404 Not Found`
  - `500 Internal Server Error`
  - API indisponible
  - timeout API
  - token expiré
  - erreur réseau
  - réponse JSON invalide
- pages premium :
  - `Views/Error/Index.cshtml`
  - `Views/Error/NotFound.cshtml`
  - `Views/Error/AccessDenied.cshtml`
  - `Views/Error/Unauthorized.cshtml`
- feedback utilisateur via `SweetAlert2`

Backend :

- chaque microservice principal expose maintenant un `UseExceptionHandler(...)`
- les erreurs non gérées retournent une réponse `application/problem+json`

## Client sans colis

- `SuiviColis` et `MesColis` ne plantent plus si le client n'a aucun colis
- affichage d'un empty state premium avec icône, message et actions
- message attendu :
  - `Aucun colis disponible pour le moment`

## Améliorations UX/UI

- dashboard admin plus compact
- cards plus petites, padding réduit, hover léger
- formulaires renforcés :
  - focus visuel
  - placeholders cohérents
  - meilleure grille responsive
- tables premium :
  - actions avec icônes Bootstrap
  - tooltips
  - spacing compact
- `Select2` activé sur les dropdowns de création/édition colis

## Création et édition de colis

- le formulaire n'affiche plus uniquement `ClientId` et `LivreurId`
- les listes déroulantes affichent :
  - `Nom + Prenom` pour le client
  - `Nom du livreur` pour le livreur
- l'API continue de recevoir les identifiants numériques

## Design premium

Le frontend applique :

- Bootstrap 5
- Bootstrap Icons
- SweetAlert2
- Chart.js uniquement pour le dashboard admin
- sidebar sombre premium
- navbar claire avec ombre douce
- cards et formulaires modernisés
- page login/register centrée
- badges de statut
- responsive mobile/tablette/desktop

## Tester localement

### 1. SQL Server

```bash
docker compose up -d sqlserver
```

### 2. Lancer les services

```bash
dotnet run --project Backend/AuthService/AuthService.csproj
dotnet run --project Backend/ClientService/ClientService.csproj
dotnet run --project Backend/LivreurService/LivreurService.csproj
dotnet run --project Backend/ColisService/ColisService.csproj
dotnet run --project Backend/VehiculeService/VehiculeService.csproj
dotnet run --project Backend/PaiementService/PaiementService.csproj
dotnet run --project Backend/LivraisonService/LivraisonService.csproj
dotnet run --project Backend/DashboardService/DashboardService.csproj
dotnet run --project Backend/GatewayService/GatewayService.csproj
dotnet run --project LivraisonFrontend/LivraisonFrontend.csproj
```

### 3. URLs utiles

- Gateway : `http://localhost:7000`
- Frontend MVC : `http://localhost:5040`
- Frontend MVC HTTPS : `https://localhost:7295`

## Tester l'inscription

1. ouvrir `http://localhost:5040/Auth/Register`
2. créer un compte client
3. vérifier qu'un `Compte` est créé avec `Role = User`
4. vérifier qu'un `Client` est créé avec le même `CompteId`

## Tester le login

### Admin

- login : `admin`
- mot de passe : `admin123`

Résultat attendu :

- redirection vers `/Dashboard/Index`

### Client

- login : `user`
- mot de passe : `user123`

Résultat attendu :

- redirection vers `/SuiviColis/Index`

## Tester le suivi colis client

1. se connecter avec un compte `User`
2. ouvrir `Suivi Colis`
3. vérifier que seuls les colis du client lié sont visibles
4. utiliser les filtres :
   - id colis
   - libellé
   - statut
   - date livraison
   - ville
5. ouvrir `Mes Colis`
6. ouvrir `Mon Profil`

## Base de données

Une base par service :

- `AuthDb`
- `ClientDb`
- `LivreurDb`
- `ColisDb`
- `VehiculeDb`
- `PaiementDb`
- `LivraisonDb`

## Migrations exécutées

Déjà appliquées :

- `AuthService` : extension profil compte/client
- `ClientService` : lien `CompteId` + données client enrichies

## Remarque backend

Le filtrage sécurisé backend entre `CompteId` et `ClientId` est maintenant posé dans `ClientService`.
Le frontend utilise ensuite `clientId` pour récupérer les colis liés.

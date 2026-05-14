# LivraisonFrontend

Frontend ASP.NET Core MVC premium pour une application de gestion des livraisons connectée à un backend microservices existant via API Gateway.

## Architecture frontend

Structure principale :

- `Controllers/` : logique MVC par module
- `Models/` : modèles simples utilisés par le frontend
- `ViewModels/` : modèles dédiés aux écrans, recherche, dashboard
- `Interfaces/` : contrats des services API
- `Services/` : consommation des APIs backend via `HttpClient`
- `Helpers/` : session, JWT, middleware, ajout automatique du Bearer Token
- `Views/` : vues Razor par module
- `wwwroot/css` : thème SaaS premium
- `wwwroot/js` : interactions UI, confirmations, sidebar
- `wwwroot/images` : logo et avatar UI

## Workflow application

### Admin

1. Login via `/auth/login`
2. Récupération du JWT
3. Stockage en Session ASP.NET Core
4. Création de l'identité locale par cookie auth
5. Redirection vers `Dashboard`
6. Navigation via sidebar
7. CRUD sur clients, livreurs, colis, véhicules, paiements, livraisons, comptes
8. Logout

### User

1. Login via `/auth/login`
2. Stockage JWT en session
3. Redirection vers `Colis/Index`
4. Recherche et consultation de colis
5. Logout

## Design system

- `Bootstrap 5`
- `Bootstrap Icons`
- `Chart.js`
- `SweetAlert2`
- `DataTables` prêt à l'emploi côté UI
- palette SaaS claire, sidebar sombre, cartes de statistiques, tables modernes, formulaires sobres

## Configuration Gateway

Le frontend consomme uniquement la Gateway définie dans :

```json
"ApiSettings": {
  "GatewayBaseUrl": "http://localhost:5000"
}
```

## Sécurité et session

- JWT stocké dans `Session`
- login, rôle et nom stockés en session
- `ApiAuthorizationHandler` ajoute automatiquement `Bearer {token}` à chaque appel API
- middleware `SessionTokenValidationMiddleware` redirige vers `Login` si le token de session est absent
- autorisation par rôle avec politique `AdminOnly`

## Bibliothèques utilisées

- ASP.NET Core MVC
- Bootstrap 5
- jQuery Validation
- Chart.js
- SweetAlert2
- DataTables

## Lancement frontend

Depuis la racine du repo :

```bash
dotnet run --project LivraisonFrontend/LivraisonFrontend.csproj
```

URL locale par défaut :

- `https://localhost:7xxx`
- `http://localhost:5xxx`

Le port exact dépend du profil local et de l'environnement `launchSettings.json`.

## Comptes de démonstration

Les identifiants viennent du backend existant.

- `Admin` : utiliser un compte backend avec rôle `Admin`
- `User` : utiliser un compte backend avec rôle `User`

## Remarques

- pas de base de données dans le frontend
- pas d'Entity Framework
- pas d'AutoMapper
- modèles simples pour rester lisible pour débutants
- le comportement exact des listes paginées dépend du format renvoyé par la Gateway

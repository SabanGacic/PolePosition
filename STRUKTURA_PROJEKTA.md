# PolePosition — Struktura repozitorija (Korak 1)

Predložena struktura foldera na root nivou repozitorija:

```
PolePosition/
├── PolePosition.API/            # Glavni REST API (.NET, C#)
│   ├── Controllers/
│   ├── Services/
│   ├── Models/                  # EF Core entiteti
│   ├── DTOs/
│   ├── Data/                    # DbContext, migracije
│   ├── Middleware/
│   └── Dockerfile
├── PolePosition.Worker/         # Odvojeni mikroservis (RabbitMQ consumer)
│   ├── Consumers/
│   ├── Services/
│   └── Dockerfile
├── poleposition_mobile/         # Flutter mobilna aplikacija (Android)
├── poleposition_desktop/        # Flutter desktop aplikacija (Windows)
├── docker-compose.yml
├── .env                         # NE ide u git (u .gitignore)
├── .env.example                 # Šablona bez pravih tajni, ide u git
├── .gitignore
├── recommender-dokumentacija.md
└── README.md
```

## Sljedeći koraci nakon postavke

1. Kreiraj prazan javni GitHub repozitorij `PolePosition`.
2. Iskopiraj `docker-compose.yml`, `.env.example` i `.gitignore` u root repozitorija.
3. Otvori terminal u rootu i pokreni:
   ```
   dotnet new webapi -n PolePosition.API
   dotnet new worker -n PolePosition.Worker
   ```
4. Kreiraj Flutter projekte:
   ```
   flutter config --enable-windows-desktop
   flutter create poleposition_mobile
   flutter create poleposition_desktop
   ```
5. Napravi lokalni `.env` (kopija `.env.example` sa stvarnim vrijednostima — nikad ga ne commit-uj).
6. Testiraj da li baza i RabbitMQ rade:
   ```
   docker compose up polepositiondb rabbitmq
   ```
   Provjeri da RabbitMQ management UI radi na `http://localhost:15672`.
7. Prvi commit i push na GitHub (sa `.gitignore` da build i `.env` ne uđu u repo).

Kada ovo stoji, prelazimo na **Korak 2: EF Core modeli i prva migracija baze podataka** (tabele: ApplicationUser, Track, Kart, Session, Reservation, Payment, LapResult, MaintenanceRecord, IncidentRecord, Notification, RecommendationLog + referentne tabele).

# ImStudio

> A visual GUI layout studio evolving from a native **C++ / Dear ImGui** editor toward an **ASP.NET Core MVC / .NET 8** web platform, with a planned **Legal Knowledge Graph** workspace.

[![Linux Build Status](https://github.com/boudarbala/ImStudio/actions/workflows/linux.yml/badge.svg)](https://github.com/boudarbala/ImStudio/actions/workflows/linux.yml)
[![Windows Build Status](https://github.com/boudarbala/ImStudio/actions/workflows/windows.yml/badge.svg)](https://github.com/boudarbala/ImStudio/actions/workflows/windows.yml)
[![macOS Build Status](https://github.com/boudarbala/ImStudio/actions/workflows/macos.yml/badge.svg)](https://github.com/boudarbala/ImStudio/actions/workflows/macos.yml)
[![Emscripten Web Build](https://github.com/boudarbala/ImStudio/actions/workflows/emscripten.yml/badge.svg)](https://github.com/boudarbala/ImStudio/actions/workflows/emscripten.yml)
[![.NET MVC Build](https://github.com/boudarbala/ImStudio/actions/workflows/dotnet-mvc.yml/badge.svg)](https://github.com/boudarbala/ImStudio/actions/workflows/dotnet-mvc.yml)

---

## 1. Project overview

ImStudio started as a real-time GUI layout creator/editor for Dear ImGui. The original application is implemented in native C++ and can also be compiled for the web with Emscripten.

The current development direction introduces a second implementation based on **ASP.NET Core MVC / .NET 8**. The two implementations are intentionally kept side by side during migration:

```text
                         ImStudio
                            │
             ┌──────────────┴──────────────┐
             │                             │
      Native implementation          Web implementation
       C++ / Dear ImGui              ASP.NET Core MVC
             │                             │
        main branch                  dotnet-mvc branch
             │                             │
       Desktop / WASM             Browser / Server
                                           │
                                           ▼
                                  Legal Graph workspace
```

The objective is not simply to rewrite the editor. The web architecture is intended to become a general modelling environment where visual UI objects, legal documents, entities and relationships can eventually be represented, persisted, searched and visualized.

---

## 2. Current project directions

The repository currently has three closely related technical directions:

### A. Native GUI editor

The original C++ implementation provides the reference implementation for Dear ImGui-oriented GUI editing.

### B. ASP.NET Core MVC editor

The `dotnet-mvc` branch contains the web migration MVP. It provides a server-side application with MVC controllers, domain models, services and Razor views.

### C. Legal Knowledge Graph

The MVC architecture is being extended toward a legal-information workspace capable of representing:

- laws;
- codes;
- decrees and regulations;
- articles;
- legal concepts;
- courts and jurisdictions;
- cases and decisions;
- offences;
- sanctions;
- legal entities;
- references between legal provisions;
- amendments and repeals;
- temporal/version information;
- provenance and source documents.

The legal graph is intended to support future keyword, vector and graph retrieval and eventually source-grounded Legal RAG / GraphRAG.

---

## 3. Branch strategy

The repository deliberately keeps the native implementation available while the MVC implementation evolves.

| Branch | Purpose |
|---|---|
| `main` | Original/native C++ implementation and Emscripten web build |
| `dotnet-mvc` | ASP.NET Core MVC migration and legal-graph foundation |

This strategy makes it possible to compare the new implementation against the existing application and migrate functionality incrementally.

Do not remove the native implementation merely because the MVC application exists. The migration should reach feature parity before the legacy implementation is considered for retirement.

---

## 4. Existing editor capabilities

The original project includes functionality around:

- drag editing;
- property editing;
- common Dear ImGui widgets;
- child windows;
- real-time generation;
- clipboard export;
- style and color tools;
- demo windows;
- external resources.

The MVC implementation currently represents an MVP rather than complete feature parity.

---

## 5. Repository architecture

The native implementation and web implementation have different runtime models.

```text
ImStudio/
│
├── src/
│   ├── native C++ / Dear ImGui sources
│   │
│   └── ImStudio.Web/
│       ├── Controllers/
│       ├── Models/
│       ├── Services/
│       ├── Views/
│       ├── wwwroot/
│       ├── Properties/
│       ├── Program.cs
│       └── ImStudio.Web.csproj
│
├── docs/
│   └── LEGAL-GRAPH.md
│
├── .github/
│   └── workflows/
│
├── CMakeLists.txt
├── Makefile.emscripten
└── README.md
```

### MVC application

```text
src/ImStudio.Web/
│
├── Controllers/
│   └── EditorController.cs
│
├── Models/
│   ├── Layout.cs
│   ├── UiObject.cs
│   ├── LegalNode.cs
│   └── LegalRelation.cs
│
├── Services/
│   ├── ILayoutService.cs
│   └── LayoutService.cs
│
├── Views/
│   ├── Editor/
│   │   └── Index.cshtml
│   ├── Shared/
│   │   └── _Layout.cshtml
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
│
├── wwwroot/
│   └── css/site.css
│
├── Properties/
│   └── launchSettings.json
│
├── Program.cs
└── ImStudio.Web.csproj
```

---

## 6. MVC architecture explained

The web application follows a conventional layered MVC design.

```text
Browser
   │
   │ HTTP
   ▼
Controller
   │
   ├──────────────► Services
   │                    │
   │                    ▼
   │                 Domain
   │                 Models
   │                    │
   │                    ▼
   │                Persistence
   │                (future)
   │
   ▼
Razor View
```

| Layer | Responsibility |
|---|---|
| Models | Domain representation of layouts, UI objects and legal graph entities |
| Controllers | HTTP endpoints and application orchestration |
| Services | Business logic and future persistence/generation operations |
| Views | Browser-facing Razor UI |
| `wwwroot` | CSS, JavaScript and static assets |
| Persistence | Future SQLite, SQL Server or graph database integration |

The architecture is intentionally simple at this stage so that new capabilities can be added without coupling the UI directly to storage or external services.

---

## 7. Legal Knowledge Graph

The legal graph is an extension of the modelling architecture rather than a separate application.

### Core entities

```text
Law
 ├── Part
 │    ├── Chapter
 │    │    ├── Section
 │    │    │    └── Article
 │    │    └── ...
 │    └── ...
 │
 ├── Amendment
 ├── Repeal
 └── Version

Article ───────► Concept
Article ───────► Article
Article ───────► Law
Article ───────► Case
Case ──────────► Court
Article ───────► Offence
Offence ───────► Sanction
```

### Initial relation vocabulary

The current model supports typed directed relations such as:

- `REFERS_TO`
- `AMENDS`
- `REPEALS`
- `CONCERNS`
- `INTERPRETED_BY`
- `DERIVED_FROM`
- `APPLIES_TO`
- `SANCTIONS`

The vocabulary should remain explicit and extensible rather than embedding relationship semantics inside free-form text.

### Provenance principle

Legal information must remain traceable to its source. A graph node should therefore be associated with information such as:

```text
source document
source identifier
language
article/reference
publication date
validity period
version
source text
```

This is especially important for future retrieval and generated answers: the graph should help locate evidence, not replace the underlying legal source.

More details are available in [`docs/LEGAL-GRAPH.md`](docs/LEGAL-GRAPH.md).

---

## 8. Planned legal ingestion pipeline

The intended pipeline is:

```text
             PDF / HTML / XML / structured data
                              │
                              ▼
                         Ingestion
                              │
                              ▼
                    Text normalization
                              │
                              ▼
                  Document segmentation
                              │
                              ▼
                Legal entity extraction
                              │
                              ▼
                    Relation extraction
                              │
                              ▼
                 Legal Knowledge Graph
                              │
              ┌───────────────┼───────────────┐
              ▼               ▼               ▼
            BM25           Vector           Graph
          retrieval       retrieval       traversal
              └───────────────┼───────────────┘
                              ▼
                       Evidence ranking
                              │
                              ▼
                    Legal RAG / GraphRAG
                              │
                              ▼
                    Source-grounded result
```

The design targets French and Arabic legal content and should preserve the original document and structural context whenever possible.

---

## 9. Legal graph and UI relationship

The long-term idea is to make the GUI editor a visual modelling layer for the legal graph.

```text
Legal document
      │
      ▼
Legal entities
      │
      ▼
Graph model
      │
      ├──────────────► Graph database
      │
      ├──────────────► Search index
      │
      └──────────────► Vector index
      │
      ▼
ImStudio visual workspace
      │
      ├── Node inspector
      ├── Relationship editor
      ├── Document/source panel
      ├── Timeline/version panel
      └── Graph visualization
```

This makes ImStudio potentially useful as both an editor and an information-modelling environment.

---

## 10. Requirements

### Development

- Git
- .NET 8 SDK for the MVC branch
- C++ toolchain for the native application
- CMake for the native build
- Emscripten for the WebAssembly build

### MVC runtime

The MVC application is a server-side ASP.NET Core application. It is **not** a static HTML application.

This distinction is important:

```text
GitHub Pages
     │
     └── Static files / WebAssembly

ASP.NET Core MVC
     │
     └── Running .NET process
             │
             ├── Kestrel
             ├── optional Nginx reverse proxy
             └── optional database/services
```

GitHub Pages therefore remains appropriate for the static Emscripten build, while the MVC application needs a server capable of running .NET.

---

## 11. Run the MVC application locally

Clone the MVC branch:

```bash
git clone -b dotnet-mvc https://github.com/boudarbala/ImStudio.git
cd ImStudio
```

Restore dependencies:

```bash
dotnet restore src/ImStudio.Web/ImStudio.Web.csproj
```

Run:

```bash
dotnet run --project src/ImStudio.Web/ImStudio.Web.csproj
```

The development configuration currently uses:

```text
http://localhost:5080
```

For development, the recommended loop is:

```text
git pull
   ↓
dotnet restore
   ↓
dotnet build
   ↓
dotnet run
   ↓
browser testing
```

---

## 12. Build the MVC application

Debug build:

```bash
dotnet build src/ImStudio.Web/ImStudio.Web.csproj
```

Release build:

```bash
dotnet build src/ImStudio.Web/ImStudio.Web.csproj -c Release
```

Publish the application:

```bash
dotnet publish src/ImStudio.Web/ImStudio.Web.csproj -c Release -o ./publish
```

The resulting `publish/` directory is a deployable application payload. Microsoft documents `dotnet publish` as the standard way to package an ASP.NET Core application for server deployment. citeturn0search0

---

## 13. GitHub Actions / CI-CD

GitHub Actions is used to automate repository workflows such as build, test and deployment. citeturn0search3turn0search8

The MVC CI workflow is conceptually:

```text
Git push
   │
   ▼
GitHub Actions
   │
   ├── checkout repository
   ├── install .NET SDK
   ├── restore dependencies
   ├── build
   ├── test (when tests are available)
   ├── publish
   └── upload artifact
```

Workflow artifacts allow build output to be persisted after a job and shared with later jobs, which makes them suitable for a build → deploy pipeline. citeturn0search1

### Recommended deployment separation

```text
                 ┌───────────────┐
                 │  dotnet-mvc   │
                 │    branch     │
                 └───────┬───────┘
                         │
                         ▼
                    CI workflow
                         │
                 build + publish
                         │
                         ▼
                     Artifact
                         │
                         ▼
                  Deploy workflow
                         │
                         ▼
                    production
```

For production, GitHub environments can provide deployment-specific secrets, variables, branch restrictions and approval rules. citeturn0search4turn0search6

---

## 14. Production deployment model

A recommended Linux deployment architecture is:

```text
Internet
   │
   ▼
 HTTPS :443
   │
   ▼
 Nginx
   │
   │ reverse proxy
   ▼
 Kestrel
   │
   ▼
 ASP.NET Core MVC
   │
   ├── application services
   ├── legal graph services
   ├── search services
   └── database / graph store (future)
```

Microsoft's Linux hosting guidance describes the common pattern of placing ASP.NET Core behind Nginx and using `systemd` to manage the application process. citeturn0search0

### Example server directory

```text
/var/www/imstudio-mvc/
├── ImStudio.Web.dll
├── appsettings.json
├── wwwroot/
└── ...
```

### Example systemd concept

```text
GitHub Actions
      │
      ▼
SSH/SCP deployment
      │
      ▼
/var/www/imstudio-mvc
      │
      ▼
systemctl restart imstudio-mvc
      │
      ▼
Kestrel
      │
      ▼
Nginx
```

The exact server hostname, user and credentials must be supplied through deployment configuration and should never be committed to the repository.

---

## 15. GitHub deployment environments

A production deployment should preferably use a GitHub environment such as:

```text
production
```

Typical deployment configuration can contain:

```text
Environment: production

Secrets:
  DEPLOY_HOST
  DEPLOY_USER
  DEPLOY_SSH_KEY

Variables:
  MVC_URL
```

The workflow can then associate its deployment job with the environment. GitHub supports environment URLs, protection rules and environment-scoped secrets. citeturn0search10turn0search4

**Never place private SSH keys, passwords or server credentials in `README.md`, source code or committed workflow files.**

---

## 16. Static Emscripten deployment

The original Emscripten application is a separate deployment target.

The legacy static build is available at:

```text
https://boudarbala.github.io/ImStudio/
```

The architecture is:

```text
C++ / Dear ImGui
       │
       ▼
   Emscripten
       │
       ▼
 WebAssembly + static assets
       │
       ▼
 GitHub Pages
```

This does not require a running ASP.NET server.

The MVC application has a fundamentally different runtime architecture and therefore should not be treated as a GitHub Pages static application.

---

## 17. Native build

The native implementation remains the reference implementation during migration.

Typical native development follows the project's existing CMake/Makefile configuration.

The purpose of keeping the native implementation is to preserve existing functionality while the MVC implementation progressively adds:

```text
Native feature
      │
      ▼
Domain representation
      │
      ▼
MVC service
      │
      ▼
Razor/JavaScript UI
      │
      ▼
Feature parity
```

This avoids replacing a mature feature set with an incomplete rewrite.

---

## 18. Data and persistence roadmap

The current MVP keeps the persistence layer intentionally lightweight. Future persistence can be introduced behind service interfaces.

Possible storage architecture:

```text
                  Application
                      │
              ┌───────┴───────┐
              ▼               ▼
        Relational DB     Graph DB
              │               │
         SQL Server /      Neo4j /
           SQLite          alternative
              │               │
              └───────┬───────┘
                      ▼
                Search layer
                 ├── BM25
                 ├── Vector
                 └── Graph
```

A relational database can manage application/project metadata while a graph database can represent highly connected legal relationships. The final architecture should be selected after the domain model and query requirements stabilize.

---

## 19. Internationalization and legal data

The legal workspace is designed with multilingual content in mind.

Initial target languages:

- French
- Arabic

Future internationalization should distinguish:

```text
Document language
       │
       ├── French
       ├── Arabic
       └── other

Legal concept
       │
       ├── canonical identifier
       └── multilingual labels
```

A legal concept should not be treated as a completely different entity merely because its label changes between French and Arabic.

---

## 20. Source-grounded retrieval

The future retrieval system should distinguish between:

1. finding relevant legal material;
2. identifying the graph relationships around that material;
3. generating an answer;
4. showing the underlying evidence.

Conceptually:

```text
User question
     │
     ▼
Query analysis
     │
     ├───────────────┐
     ▼               ▼
Keyword search   Vector search
     │               │
     └───────┬───────┘
             ▼
       Graph expansion
             │
             ▼
      Evidence ranking
             │
             ▼
       Answer generation
             │
             ▼
   Source / article references
```

The graph should therefore complement source documents rather than become an opaque replacement for them.

---

## 21. Testing strategy

The testing strategy should progressively cover:

### Unit tests

- model validation;
- relationship validation;
- service behavior;
- serialization;
- legal relation vocabulary.

### Integration tests

- MVC controller/service integration;
- persistence;
- graph database integration;
- document ingestion;
- search/retrieval.

### End-to-end tests

```text
Browser
  ↓
MVC endpoint
  ↓
Service
  ↓
Graph/search layer
  ↓
Evidence
  ↓
Rendered result
```

CI should run these tests before production deployment as the project matures.

---

## 22. Security principles

The production architecture should follow several basic rules:

- never commit credentials;
- use GitHub environment secrets for deployment credentials;
- use HTTPS in production;
- restrict SSH access on deployment servers;
- validate uploaded documents;
- avoid executing untrusted document content;
- validate graph identifiers and relationships;
- preserve source provenance;
- log deployment and application events;
- separate development, staging and production configuration.

For deployment, environment-scoped secrets and protection rules are preferable to hard-coded credentials. citeturn0search4turn0search6

---

## 23. Development workflow

A practical development cycle is:

```text
Issue / feature
      │
      ▼
Create branch
      │
      ▼
Implement
      │
      ▼
Build + tests
      │
      ▼
Pull request
      │
      ▼
CI validation
      │
      ▼
Review
      │
      ▼
Merge
      │
      ▼
Production deployment
```

For the MVC migration, changes should preferably be made incrementally so that each feature can be compared with the original native implementation.

---

## 24. Current MVC MVP

The `dotnet-mvc` branch currently provides the foundation for:

- ASP.NET Core MVC / .NET 8;
- layout and UI-object domain models;
- initial legal graph node and relation models;
- MVC editor controller;
- service layer;
- Razor-based editor view;
- visual canvas;
- basic UI-object interaction;
- JSON-oriented editor functionality;
- local launch configuration;
- automated .NET CI build.

The implementation is intentionally an MVP. It is not yet a complete replacement for the native editor.

---

## 25. Roadmap

### Phase 1 — MVC editor foundation

- [x] ASP.NET Core MVC project
- [x] .NET 8 target
- [x] MVC controller/service structure
- [x] Basic editor view
- [x] Layout/UI object models
- [x] Initial legal graph models
- [x] CI build workflow

### Phase 2 — Visual editor

- [ ] drag-and-drop editor
- [ ] object selection
- [ ] property inspector
- [ ] position/size editing
- [ ] resize handles
- [ ] undo/redo
- [ ] keyboard shortcuts
- [ ] richer widget library

### Phase 3 — Serialization and generation

- [ ] JSON import
- [ ] JSON export
- [ ] HTML/CSS/JavaScript generation
- [ ] Dear ImGui C++ generation
- [ ] reusable project templates

### Phase 4 — Legal graph

- [x] initial node model
- [x] initial relation model
- [x] architecture documentation
- [ ] legal ontology
- [ ] document ingestion
- [ ] PDF/HTML/XML processing
- [ ] article/reference extraction
- [ ] temporal/version model
- [ ] graph visualization

### Phase 5 — Retrieval

- [ ] BM25 retrieval
- [ ] vector retrieval
- [ ] graph traversal
- [ ] hybrid ranking
- [ ] provenance-aware results
- [ ] Legal RAG
- [ ] GraphRAG

### Phase 6 — Production platform

- [ ] database persistence
- [ ] authentication
- [ ] project management
- [ ] Docker image
- [ ] Linux VM deployment
- [ ] Azure deployment option
- [ ] staging environment
- [ ] production environment
- [ ] monitoring and health checks

### Phase 7 — Migration completion

- [ ] feature parity with native editor
- [ ] performance comparison
- [ ] compatibility validation
- [ ] migration documentation
- [ ] decision on retirement of legacy web workflow

---

## 26. Recommended deployment topology

For a production installation, a practical target is:

```text
                         GitHub
                           │
                     GitHub Actions
                           │
                 ┌─────────┴─────────┐
                 │                   │
               Build              Deploy
                 │                   │
                 ▼                   ▼
             Artifact            Linux VM
                                     │
                                  Nginx
                                     │
                                  Kestrel
                                     │
                              ImStudio.Web
                                     │
                   ┌─────────────────┼─────────────────┐
                   ▼                 ▼                 ▼
                SQL DB           Graph DB          Search
                (future)         (future)           (future)
```

This separates source control, build automation and runtime infrastructure.

---

## 27. Important deployment distinction

There are three different deployment products in this repository:

| Target | Runtime | Suitable platform |
|---|---|---|
| Native C++ | Native executable | Windows/Linux/macOS |
| Emscripten | WebAssembly/static files | GitHub Pages/static hosting |
| ASP.NET MVC | .NET server process | Linux VM, Docker, Azure App Service, etc. |

Confusing these targets is a common source of deployment problems. GitHub Pages can serve the static Emscripten output, but it cannot execute the ASP.NET Core MVC server process.

---

## 28. Documentation map

| Document | Purpose |
|---|---|
| `README.md` | Project overview, architecture, development and deployment |
| `docs/LEGAL-GRAPH.md` | Legal graph architecture and domain direction |
| `.github/workflows/` | Automated CI/CD workflows |
| `src/ImStudio.Web/` | ASP.NET Core MVC implementation |
| Native source tree | C++ / Dear ImGui implementation |

---

## 29. Contribution guidelines

When adding a feature:

1. Identify whether it belongs to the native editor, MVC editor or legal graph.
2. Keep domain models independent from presentation where possible.
3. Put reusable business logic in services rather than controllers.
4. Keep source provenance for legal information.
5. Add tests when the feature has stable behavior.
6. Update the documentation when architecture changes.
7. Do not commit secrets or server credentials.
8. Prefer small, reviewable commits.

---

## 30. License and credits

The project retains the original **MIT license**.

The native implementation is based on the existing ImStudio / Dear ImGui ecosystem and original project structure. The MVC and legal-graph work is an incremental evolution of the repository.

---

## 31. Project status

**Current status:** active architectural migration / MVP.

The most important next engineering step is to move from the current MVC foundation toward a real interactive editor while keeping the legal graph domain isolated enough to evolve independently.

The intended final platform is:

```text
                    ImStudio Platform
                           │
       ┌───────────────────┼───────────────────┐
       ▼                   ▼                   ▼
 Visual Editor       Legal Graph          Retrieval/RAG
       │                   │                   │
       └───────────────────┼───────────────────┘
                           ▼
                    Source-grounded
                  information workspace
```

The architecture is deliberately designed so that the GUI editor and legal-information components can evolve together without requiring the original native application to be discarded prematurely.

# ImStudio

[![Linux Build Status](https://github.com/boudarbala/ImStudio/actions/workflows/linux.yml/badge.svg)](https://github.com/boudarbala/ImStudio/actions/workflows/linux.yml)
[![Windows Build Status](https://github.com/boudarbala/ImStudio/actions/workflows/windows.yml/badge.svg)](https://github.com/boudarbala/ImStudio/actions/workflows/windows.yml)
[![macOS Build Status](https://github.com/boudarbala/ImStudio/actions/workflows/macos.yml/badge.svg)](https://github.com/boudarbala/ImStudio/actions/workflows/macos.yml)
[![Emscripten Web Build](https://github.com/boudarbala/ImStudio/actions/workflows/emscripten.yml/badge.svg)](https://github.com/boudarbala/ImStudio/actions/workflows/emscripten.yml)
[![.NET MVC Build](https://github.com/boudarbala/ImStudio/actions/workflows/dotnet-mvc.yml/badge.svg)](https://github.com/boudarbala/ImStudio/actions/workflows/dotnet-mvc.yml)

### 🚀 Legacy Try Online

The existing static Emscripten build is available at https://boudarbala.github.io/ImStudio/.

> The `dotnet-mvc` version is a server-side ASP.NET Core application and cannot run directly on GitHub Pages.

## Project direction

ImStudio is a real-time GUI layout creator/editor for Dear ImGui. The project is being migrated from the native C++ implementation toward a web-based **ASP.NET Core MVC / .NET 8** architecture.

The migration is maintained on the `dotnet-mvc` branch while the original C++ implementation remains on `main` for feature comparison.

The web architecture is also becoming a foundation for a **Legal Graph workspace**: a source-grounded environment for modelling laws, articles, cases, concepts and legal relations as a knowledge graph.

## Existing editor capabilities

- Drag edit
- Property edit
- Common Dear ImGui widgets
- Child windows
- Real-time generation
- Export to clipboard
- Style and color tools
- Demo window and external resources

## .NET MVC architecture

```text
src/ImStudio.Web/
├── Controllers/
│   └── EditorController.cs
├── Models/
│   ├── Layout.cs
│   ├── UiObject.cs
│   ├── LegalNode.cs
│   └── LegalRelation.cs
├── Services/
│   ├── ILayoutService.cs
│   └── LayoutService.cs
├── Views/
│   ├── Editor/Index.cshtml
│   ├── Shared/_Layout.cshtml
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
├── wwwroot/css/site.css
├── Properties/launchSettings.json
├── Program.cs
└── ImStudio.Web.csproj
```

### Responsibilities

| Layer | Responsibility |
|---|---|
| Models | UI layouts and legal graph domain objects |
| Controllers | HTTP/MVC endpoints and orchestration |
| Services | Layout creation and future persistence/generation |
| Views | Editor interface and future legal graph workspace |
| wwwroot | CSS and client-side assets |

## Legal Graph foundation

The first legal-domain models are now included on `dotnet-mvc`:

- `LegalNode` — law, article, case, concept, jurisdiction, offence, sanction, etc.
- `LegalRelation` — directed typed relationships such as `REFERS_TO`, `AMENDS`, `REPEALS`, `CONCERNS` and `INTERPRETED_BY`.
- Source metadata is retained through identifiers, language, source and source text fields.

See [`docs/LEGAL-GRAPH.md`](docs/LEGAL-GRAPH.md) for the architecture and roadmap.

The planned pipeline is:

```text
PDF / HTML / XML
       ↓
   Ingestion
       ↓
 Normalization
       ↓
 Legal Knowledge Graph
       ↓
 ┌─────┼─────┐
 ▼     ▼     ▼
BM25 Vector Graph
 └─────┼─────┘
       ↓
    Legal RAG
       ↓
Source-grounded answer
```

The design targets French and Arabic legal content and keeps provenance/version information so that generated answers can remain linked to their underlying legal sources.

## Requirements

- .NET 8 SDK
- Git

## Run the MVC application

```bash
git clone -b dotnet-mvc https://github.com/boudarbala/ImStudio.git
cd ImStudio
dotnet restore src/ImStudio.Web/ImStudio.Web.csproj
dotnet run --project src/ImStudio.Web/ImStudio.Web.csproj
```

Development URL:

```text
http://localhost:5080
```

## Build and publish

```bash
dotnet build src/ImStudio.Web/ImStudio.Web.csproj -c Release
dotnet publish src/ImStudio.Web/ImStudio.Web.csproj -c Release -o ./publish
```

GitHub Actions automatically restores, builds, publishes and uploads the MVC application as an artifact for the `dotnet-mvc` branch.

## Current MVC MVP

The branch currently provides:

- ASP.NET Core MVC / .NET 8 project
- Layout and UI-object domain models
- Initial legal graph node and relation models
- MVC editor controller
- Service layer
- Visual canvas
- Basic UI objects
- Add-button interaction
- JSON export
- Local launch configuration
- Automated .NET CI build

## Roadmap

1. Drag-and-drop editor
2. Selection and property inspector
3. Position/size editing
4. Undo/redo
5. JSON import/export
6. HTML/CSS/JavaScript generation
7. Dear ImGui C++ code generation
8. Legal document ingestion (PDF/HTML/XML)
9. Legal ontology and typed relation vocabulary
10. Graph persistence with Neo4j or another graph database
11. Keyword + vector + graph retrieval
12. Source-grounded Legal RAG / GraphRAG
13. Interactive legal graph visualization
14. SQLite/SQL Server persistence with EF Core
15. Authentication and project management
16. Docker packaging
17. Azure App Service / Linux VM deployment
18. Feature parity with the native editor
19. Retire the legacy C++ web workflow after parity

## Deployment

GitHub Pages is suitable for the existing static Emscripten output, but ASP.NET Core MVC requires a running .NET server. The MVC application can be deployed to a Linux VM, Docker host, Azure App Service, or another ASP.NET-compatible platform.

## Native implementation

The original C++/Dear ImGui implementation remains on `main` during the migration. This allows the MVC implementation to be developed incrementally without destroying the existing application.

## Credits and license

The project retains the original MIT license.

Thanks to Omar for Dear ImGui and Code-Building for the original inspiration.

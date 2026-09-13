# GitHub Actions, Testing and Deployment Guide

This document is the operational guide for CI, build validation, artifacts and future production deployment of ImStudio.

## Quick links

- [Repository](https://github.com/boudarbala/ImStudio)
- [dotnet-mvc branch](https://github.com/boudarbala/ImStudio/tree/dotnet-mvc)
- [GitHub Actions](https://github.com/boudarbala/ImStudio/actions)
- [.NET MVC workflow](https://github.com/boudarbala/ImStudio/actions/workflows/dotnet-mvc.yml)
- [.NET MVC workflow source](../.github/workflows/dotnet-mvc.yml)
- [Emscripten workflow](https://github.com/boudarbala/ImStudio/actions/workflows/emscripten.yml)
- [Emscripten workflow source](../.github/workflows/emscripten.yml)
- [Linux workflow](https://github.com/boudarbala/ImStudio/actions/workflows/linux.yml)
- [Windows workflow](https://github.com/boudarbala/ImStudio/actions/workflows/windows.yml)
- [macOS workflow](https://github.com/boudarbala/ImStudio/actions/workflows/macos.yml)
- [Legal Graph documentation](./LEGAL-GRAPH.md)
- [Static Web application](https://boudarbala.github.io/ImStudio/)

## Current CI workflows

| Workflow | Branch | Purpose |
|---|---|---|
| `.NET MVC Build` | `dotnet-mvc` | Restore, build, publish and upload MVC artifact |
| `Emscripten Web` | `main` | Build WebAssembly, validate `web/index.html`, upload artifact and publish GitHub Pages |
| `linux` | `main` | Native Linux/CMake build |
| `Windows` | `main` | Native Windows build/package |
| `macOS` | `main` | Native macOS build |

The current MVC workflow supports `push`, pull requests and manual `workflow_dispatch`. GitHub documents `workflow_dispatch` as the mechanism for manually launching a workflow from the Actions interface. See the [GitHub Actions manual-run documentation](https://docs.github.com/en/actions/how-tos/manage-workflow-runs/manually-run-a-workflow).

## MVC CI pipeline

```text
             push / pull request / manual run
                           │
                           ▼
                  .NET MVC Build
                           │
                 ┌─────────┴─────────┐
                 ▼                   ▼
              restore              checkout
                 │
                 ▼
               build
                 │
                 ▼
              publish
                 │
                 ▼
              artifact
                 │
                 ▼
          future deployment job
                 │
                 ▼
             production
```

The workflow currently uses .NET 8 and uploads the published application as `imstudio-dotnet-mvc`. The repository workflow source is `.github/workflows/dotnet-mvc.yml`.

## How to run the MVC test/build action

1. Open the [Actions page](https://github.com/boudarbala/ImStudio/actions).
2. Select **.NET MVC Build**.
3. Select **Run workflow**.
4. Choose the `dotnet-mvc` branch.
5. Start the workflow.
6. Inspect the `Restore`, `Build`, `Publish` and artifact steps.

The workflow is configured with `workflow_dispatch`, so it can be started manually when the workflow is available on the repository's default branch. For branch-specific development, a normal push or pull request is also sufficient to trigger the configured CI.

## Local validation before pushing

Run the same fundamental operations locally:

```bash
dotnet restore src/ImStudio.Web/ImStudio.Web.csproj
dotnet build src/ImStudio.Web/ImStudio.Web.csproj -c Release --no-restore
dotnet publish src/ImStudio.Web/ImStudio.Web.csproj -c Release -o ./publish
```

Then launch the application:

```bash
dotnet run --project src/ImStudio.Web/ImStudio.Web.csproj
```

Open:

```text
http://localhost:5080
```

## Automated tests: current and target state

The MVC workflow currently performs build/publish validation. A dedicated test project should be added as the MVC functionality grows:

```text
tests/
└── ImStudio.Web.Tests/
    ├── Controllers/
    ├── Services/
    ├── Models/
    └── LegalGraph/
```

The target CI sequence is:

```bash
dotnet restore
dotnet build --no-restore
dotnet test --no-build --configuration Release
dotnet publish --configuration Release
```

Recommended test layers:

### Unit tests

- `LayoutService` behavior
- UI object validation
- legal node validation
- legal relation validation
- relation vocabulary rules
- JSON serialization/deserialization

### MVC tests

- editor controller responses
- routing
- model binding
- JSON export
- invalid input handling

### Legal Graph tests

- unique node identifiers
- valid source/provenance metadata
- relation direction
- allowed relation types
- graph consistency
- version/validity constraints

### Integration tests

Future integration tests should validate the application with its persistence layer and search infrastructure.

## Smoke-test checklist

After a successful MVC build, verify:

- [ ] application starts successfully
- [ ] `/` or editor route responds
- [ ] CSS/static files load
- [ ] UI objects can be created
- [ ] JSON export produces valid JSON
- [ ] legal nodes can be represented
- [ ] legal relations preserve source/identifier information
- [ ] application logs contain no startup exception
- [ ] published output contains the application DLL and `wwwroot`

## Artifact validation

The MVC workflow publishes the application into a GitHub Actions artifact named:

```text
imstudio-dotnet-mvc
```

The Emscripten workflow produces:

```text
imstudio-web
```

Artifacts should be used as immutable hand-off packages between build and deployment jobs rather than rebuilding the application during deployment.

## Deployment architecture

```text
GitHub
  │
  ▼
CI build
  │
  ▼
Artifact
  │
  ▼
Deployment job
  │
  ├── GitHub Environment: production
  │
  └── deployment credentials
          │
          ▼
       Linux VM
          │
          ▼
       systemd
          │
          ▼
       Kestrel
          │
          ▼
        Nginx
          │
          ▼
        HTTPS
```

GitHub environments can scope deployment secrets and variables and can add approval/protection rules. See the [GitHub environment deployment documentation](https://docs.github.com/en/actions/how-tos/write-workflows/choose-what-workflows-do/deploy-to-environment).

## Production secrets

Recommended environment-level secrets:

```text
DEPLOY_HOST
DEPLOY_USER
DEPLOY_SSH_KEY
```

Recommended environment-level variables:

```text
MVC_URL
```

Do not commit these values. Do not put private keys in YAML, README files, source files or issue comments.

## Future deployment workflow

The intended production workflow is:

```text
build
  │
  ├── restore
  ├── compile
  ├── test
  └── publish
        │
        ▼
     artifact
        │
        ▼
deploy-production
        │
        ├── download artifact
        ├── transfer package
        ├── update application directory
        ├── restart systemd service
        └── HTTP health check
```

A health-check stage should verify the deployed application before considering the deployment successful.

Example target check:

```bash
curl --fail --silent --show-error https://YOUR-DOMAIN/health
```

A dedicated `/health` endpoint should be added to the MVC application before this check is enabled in production.

## Concurrency and safe deployments

Production deployment should use a concurrency group so two deployments do not overwrite each other simultaneously:

```yaml
concurrency:
  group: imstudio-production
  cancel-in-progress: false
```

GitHub recommends environments and concurrency controls for managing deployments and preventing conflicting releases.

## Self-hosted runner option

A second deployment model is a self-hosted GitHub Actions runner installed directly on the target VM:

```text
GitHub Actions
      │
      ▼
self-hosted runner
      │
      ▼
/var/www/imstudio-mvc
      │
      ▼
systemctl restart imstudio-mvc
```

This removes the need for SSH-based deployment, but the runner becomes part of the production security boundary and must be maintained carefully. GitHub supports targeting self-hosted runners through `runs-on` labels.

## Emscripten / GitHub Pages validation

The Emscripten workflow validates that the generated web package contains:

```text
web/index.html
```

It then uploads the web artifact and, for non-pull-request executions, publishes the generated `web/` directory to the `gh-pages` branch.

The public static application is:

https://boudarbala.github.io/ImStudio/

## CI status interpretation

| Status | Meaning |
|---|---|
| Green | Workflow completed successfully |
| Red | At least one required job/step failed |
| Yellow | Workflow is waiting, queued or awaiting approval |
| Cancelled | Workflow was stopped before completion |
| Skipped | A job/step condition prevented execution |

For failures, open the workflow run and inspect the failed job's logs before changing source code.

## Recommended next CI improvements

1. Add `ImStudio.Web.Tests`.
2. Add `dotnet test` to the MVC workflow.
3. Add ASP.NET integration tests.
4. Add a `/health` endpoint.
5. Add deployment workflow using the `production` environment.
6. Add deployment smoke tests.
7. Add rollback strategy.
8. Add dependency/security scanning.
9. Add code-format/style validation.
10. Add database migration validation.
11. Add Legal Graph consistency tests.
12. Add end-to-end browser tests.

## References

- [GitHub Actions documentation](https://docs.github.com/en/actions)
- [Manual workflow execution](https://docs.github.com/en/actions/how-tos/manage-workflow-runs/manually-run-a-workflow)
- [Deploying to an environment](https://docs.github.com/en/actions/how-tos/write-workflows/choose-what-workflows-do/deploy-to-environment)
- [Choosing a runner](https://docs.github.com/en/actions/how-tos/write-workflows/choose-where-workflows-run/choose-the-runner-for-a-job)
- [Workflow artifacts](https://docs.github.com/en/actions/concepts/workflows-and-actions/artifacts)

# Zametek.Web.ProjectPlan Assessment

## Executive Summary

The current solution is a desktop-first Avalonia application with a reasonably clean separation between domain models, contracts, data versioning, views, and view models. That gives a useful starting point for a web product, but it is not yet structured as a web-ready or SaaS-ready system.

The good news is that a new web app can be added without breaking the current desktop app. The safest approach is to treat the web application as a new composition root and add new projects around the existing reusable core, instead of trying to retrofit the desktop UI stack into the browser.

The main architectural gap is not UI. It is the absence of server-side application boundaries for persistence, IAM, tenant resolution, authorization, concurrency, and operational concerns.

## What Can Be Reused

### Strong candidates for reuse

- `Zametek.Common.ProjectPlan`
  - Core models, enums, scheduling-related concepts.
- `Zametek.Data.ProjectPlan`
  - Versioned serialization and upgrade pipeline.
- Parts of `Zametek.ViewModel.ProjectPlan`
  - Only where logic is genuinely UI-independent.
  - Graph compilation and project processing logic should be extracted from VM orchestration before reuse.

### Poor candidates for direct reuse

- `Zametek.View.ProjectPlan`
  - Avalonia-specific.
- Most of the manager view models in `Zametek.ViewModel.ProjectPlan`
  - Shaped around docked desktop panels, ReactiveUI state, and chart/view concerns.
- `Zametek.ProjectPlan`
  - Desktop composition root only.

## Architectural Changes Required

### 1. Add a server-side application boundary

The current app is effectively a rich client with file-based workflows. A web product needs a backend that owns:

- authentication
- authorization
- tenant resolution
- project persistence
- import/export orchestration
- audit and operational logging
- concurrency and conflict handling
- API contracts for browser clients

Recommended additions:

- `Zametek.Web.ProjectPlan`
  - ASP.NET Core host.
- `Zametek.Application.ProjectPlan`
  - Use cases, commands/queries, DTOs, validation, authorization rules.
- `Zametek.Infrastructure.ProjectPlan`
  - EF Core, storage, IAM integrations, tenant resolution, file/object storage, email, background jobs.
- `Zametek.Persistence.ProjectPlan`
  - Optional split if data access grows large.
- `Zametek.Web.ProjectPlan.Client`
  - Only if using React or a separate SPA.

This keeps the current desktop app intact and avoids contaminating the existing UI layers.

### 2. Introduce persistent storage

A multi-tenant web app cannot stay file-centric as its primary storage model.

You will need:

- a relational database, likely SQL Server or PostgreSQL
- persisted tenants, users, memberships, projects, snapshots, imports, exports
- optimistic concurrency tokens on mutable records
- background job state for long-running imports/exports if needed

The existing `.zpp` format can remain as an import/export format.

### 3. Add IAM and authorization

Authentication and authorization need to be separated.

Recommended approach:

- Use OpenID Connect / OAuth 2.0 with an external identity provider.
- Keep tenant membership and application roles inside the app database.

Viable providers:

- Microsoft Entra ID
- Auth0
- Keycloak
- Authentik

Recommended authorization model:

- `User`
- `Tenant`
- `TenantMembership`
- `Role`
- `Project`
- `ProjectAccess`

Typical roles:

- tenant owner
- tenant admin
- project editor
- project reader

Do not encode all tenancy rules only in the identity provider. The application still needs first-class tenant membership and per-resource authorization.

### 4. Add tenancy as a first-class concern

The current codebase has no tenant boundary. For a web product, every persisted resource needs a clear tenancy story.

Recommended initial model:

- shared database
- shared schema
- `TenantId` on tenant-scoped tables
- global query filters or equivalent tenant guards
- explicit authorization checks on resource access

Why this is the best starting point:

- lowest operational complexity
- fastest path to MVP
- easiest reporting and migrations

Avoid starting with database-per-tenant unless there is a clear regulatory or scale requirement.

### 5. Extract scheduling/application logic out of view-model orchestration

Today, `CoreViewModel` is doing too much from a web perspective. It is both state container and workflow coordinator.

For the web app, move core workflows into application services such as:

- `IProjectPlanCompilerService`
- `IProjectPlanImportService`
- `IProjectPlanExportService`
- `IProjectPlanPersistenceService`
- `ITenantContextAccessor`
- `IAuthorizationPolicyService`

The desktop app can continue to use its existing view models. Over time, the desktop app can also consume those extracted services, but that can be incremental.

## Pattern Changes Recommended

### Current pattern set

The desktop app is built around:

- MVVM
- ReactiveUI
- Splat DI
- docked panel composition
- AutoMapper-based model transformation

That is fine for the desktop app, but it should not be the dominant architectural pattern for the web app.

### Recommended web patterns

Use:

- ASP.NET Core for hosting
- clean application boundary or vertical slices
- request/response handlers for use cases
- EF Core for persistence
- policy-based authorization
- DTOs at the API boundary
- background jobs for long-running exports/imports when needed

Avoid:

- porting desktop MVVM directly into the web app
- sharing desktop view model interfaces with web handlers
- allowing UI state models to become server contracts

### Suggested slice structure

Prefer feature-based organization over technical folders inside the web app, for example:

```text
Zametek.Web.ProjectPlan/
  Features/
    Auth/
    Tenants/
    Projects/
    Scheduling/
    Imports/
    Exports/
    Administration/
  Infrastructure/
  Components/ or Api/
```

This maps better to actual web workflows than the current panel-manager structure.

## Code Organization Changes That Should Not Break The Current App

The safest non-breaking strategy is additive.

### Recommended additive changes

1. Add new projects for web, application, and infrastructure layers.
2. Reuse `Zametek.Common.ProjectPlan` and `Zametek.Data.ProjectPlan` as-is where possible.
3. Extract pure scheduling logic behind new interfaces in a new project instead of changing the desktop UI first.
4. Keep the Avalonia desktop composition root untouched.
5. Keep `.zpp` import/export compatibility unchanged.

### Changes to avoid in the first pass

- Do not replace Splat in the desktop app just to match ASP.NET Core DI.
- Do not rewrite desktop manager VMs to fit web controllers or components.
- Do not try to make one UI abstraction serve both Avalonia and the browser.
- Do not move everything into a large shared project; that usually creates coupling instead of reuse.

### Low-risk extraction candidates

- graph compilation orchestration
- project plan validation
- import/export coordination that does not require dialogs
- mapping between persisted project data and domain models

## Blazor Or React

## Recommendation

If the goal is a serious multi-tenant product with rich editing, graphing, and a browser experience that may eventually approach the desktop app, React is the stronger choice.

If the goal is a faster internal or lower-complexity .NET-only web front end, Blazor Server is viable, but it is still likely to struggle sooner on the richer interaction model.

## React strengths for this product

- better ecosystem for advanced graph and canvas tooling
- better support for highly interactive scheduling UI
- better library options for data grids, charts, drag/drop, and virtualized editing
- clearer separation between ASP.NET Core backend and web client
- easier path if the browser UI becomes materially different from the desktop UI

Likely useful libraries:

- React Flow or Cytoscape for graph editing/viewing
- AG Grid or TanStack Table for activity grids
- ECharts, Nivo, or Visx for charts
- TanStack Query for data access

## Blazor strengths for this product

- one-language full-stack team
- fast integration with ASP.NET Core auth and policies
- good fit for forms, admin screens, and moderate interactivity
- simpler staffing if the team is strongly .NET-centric

## Blazor risks here

- advanced interactive graph tooling is weaker than the JavaScript ecosystem
- browser-side UX may become harder to evolve as complexity grows
- desktop parity will likely require more custom component work

## Bottom line

- For product-grade SaaS: choose React with ASP.NET Core backend.
- For an internal or constrained v1: Blazor Server is acceptable if scope is kept tight.

## Suggested Target Architecture

### Option A: Recommended

```text
Browser React App
        |
        v
ASP.NET Core Web App / API / BFF
        |
        +--> Application Layer
        |
        +--> Infrastructure Layer
        |      |- EF Core
        |      |- IAM integration
        |      |- Tenant resolution
        |      |- File/object storage
        |      |- Background jobs
        |
        +--> Reused domain/data libraries
               |- Zametek.Common.ProjectPlan
               |- Zametek.Data.ProjectPlan
```

### Option B: Simpler initial stack

```text
Blazor Server
        |
        v
Application Layer
        |
        v
Infrastructure Layer
        |
        v
SQL Database + Identity Provider
```

Option B is simpler to start, but less likely to remain comfortable if the product grows toward desktop-like richness.

## Migration Strategy

### Phase 1: Technical spike

Prove the following before committing to a larger build:

- load and persist a project plan via ASP.NET Core
- compile a plan server-side without desktop dependencies
- authenticate a user and resolve current tenant
- authorize access to a tenant-scoped project
- render at least one graph/chart in the browser

### Phase 2: MVP

Build:

- tenant admin
- project CRUD
- project import/export
- scheduling compile endpoint
- basic activity editing
- role-based authorization
- audit logging

### Phase 3: Product hardening

Add:

- background processing
- notifications
- soft delete and retention rules
- usage metering/billing hooks
- support tooling and operational dashboards

## Key Risks

### 1. Over-reusing desktop abstractions

The biggest technical mistake would be forcing the web app to inherit the desktop UI architecture. Reuse domain logic, not desktop composition patterns.

### 2. Tenant rules applied too late

If `TenantId` is not introduced early at the persistence and authorization layers, retrofitting it later will be expensive.

### 3. Treating IAM as only login

IAM here also means invitation flows, tenant membership, role assignment, access revocation, and auditability.

### 4. Underestimating persistence and concurrency

The current app is mostly local-state driven. A web app introduces parallel edits, stale reads, and conflict handling.

## Practical Recommendation

If this were being started now, I would do the following:

1. Create `Zametek.Web.ProjectPlan` as a new ASP.NET Core solution entry point.
2. Add `Zametek.Application.ProjectPlan` and `Zametek.Infrastructure.ProjectPlan`.
3. Reuse `Zametek.Common.ProjectPlan` and `Zametek.Data.ProjectPlan` immediately.
4. Extract compilation and project-processing workflows out of `CoreViewModel` into application services.
5. Use React for the browser UI unless there is a strong organizational reason to stay entirely in .NET UI.
6. Use OIDC with an external identity provider and keep tenant memberships in the application database.
7. Start multi-tenancy with a shared database and explicit `TenantId` boundaries.

## Overall Effort Assessment

This is not a thin UI port. It is effectively a new product surface built on some reusable domain and serialization assets from the existing solution.

Complexity assessment:

- UI port alone: medium
- proper ASP.NET Core backend: high
- IAM done well: medium-high
- multi-tenancy done well: high
- full productization without breaking the desktop app: achievable, but requires disciplined additive architecture

The approach is viable. The right move is to build the web app beside the desktop app, not through it.
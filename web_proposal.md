# Proposal: Safe, Incremental Web Evolution for Zametek.ProjectPlan

## Purpose

This proposal outlines a path toward a web-capable product without breaking the existing desktop application, without forcing a premature rewrite, and without asking the maintainer to accept unnecessary architectural risk upfront.

The core principle is simple:

Keep the current application working exactly as it does today, and only introduce new web-facing capabilities through additive, reversible steps.

This is not a proposal to replace the Avalonia application. It is a proposal to make the core planning engine safely usable from more than one host over time.

## Non-Negotiable Constraints

- The existing desktop application must remain fully functional throughout.
- The current project file format and upgrade path must remain supported.
- Any extraction or refactoring must be introduced behind compatibility-preserving interfaces.
- The desktop application must remain a first-class product, not a temporary stepping stone.
- Each phase must be independently valuable and safe to stop after.

## Summary Recommendation

Do not begin with a full SaaS architecture rollout.

Begin by isolating a small, headless planning kernel that can be exercised by the existing desktop application and by non-UI hosts. Then, once equivalence is proven, add a minimal ASP.NET Core host for a narrow set of workflows such as project upload, compile, save, and download.

Only after that boundary is stable should the project consider tenant-aware persistence, richer browser workflows, or a dedicated browser client.

## Why This Is The Right Starting Point

The current solution already contains reusable domain and document-versioning assets, and it already has evidence that some workflows can run outside the Avalonia shell through the existing command-line entry point.

That means the safest strategy is not to start by designing the final platform. The safest strategy is to strengthen and formalize what is already implicitly reusable.

This approach offers four advantages:

- It minimizes regression risk in the desktop application.
- It lowers the amount of architectural change the maintainer must approve up front.
- It creates a technical proof point before committing to bigger web investment.
- It preserves optionality around browser UI technology and hosting model.

## What This Proposal Is Not

- It is not a rewrite of the desktop application.
- It is not a demand to adopt React immediately.
- It is not a commitment to full multi-tenancy on day one.
- It is not a requirement to remodel all planning data into relational tables before web work can begin.
- It is not a replacement for the current document-based workflow.

## Proposed Strategy

### Phase 1: Stabilize a Headless Core

Goal:
Make the planning engine safely consumable outside the Avalonia desktop shell while keeping all current behavior intact.

Actions:

- Identify the smallest set of workflows that represent the real product core:
  - load project
  - import project
  - compile plan
  - export project
  - save project
- Extract only genuinely UI-independent orchestration into a new headless services layer.
- Keep the desktop application as the first consumer of the extracted services.
- Keep the current command-line host working against the same extracted behavior.

Deliverable:
A reusable planning kernel that both the desktop app and command-line path can use without observable behavior change.

Success criteria:

- Existing desktop workflows behave the same as before.
- Existing project files open, compile, save, and export identically.
- Command-line and desktop results match for representative sample inputs.

### Phase 2: Add Regression Protection

Goal:
Prevent refactoring from breaking what already works.

Actions:

- Add characterization tests around the highest-risk workflows.
- Use representative sample project files as golden inputs.
- Compare outputs across desktop-hosted and headless execution paths.
- Preserve project file compatibility and version-upgrade behavior as a protected contract.

Minimum test coverage should include:

- open existing project files
- import supported external project formats
- compile project plans
- save to current project format
- export to supported output formats

Deliverable:
A regression shield strong enough to support extraction work safely.

Success criteria:

- No material change in output for known sample inputs.
- No loss of compatibility with historical project versions.
- Refactoring can proceed without relying on manual confidence alone.

### Phase 3: Add a Minimal ASP.NET Core Host

Goal:
Prove that the planning kernel can run behind a server boundary without destabilizing the product.

Actions:

- Introduce a minimal ASP.NET Core host as a new composition root.
- Support a narrow initial workflow set:
  - authenticate user
  - upload or select project
  - compile plan
  - save updated project document
  - download project or export artifact
- Keep the server thin and focused on orchestration.
- Avoid building a rich in-browser editor at this stage.

Deliverable:
A small web host that demonstrates value without overcommitting the project.

Success criteria:

- The same core planning logic runs under desktop, command-line, and server hosts.
- The web host exercises real workflows, not a toy demo.
- No existing desktop behavior is altered to make the web host possible.

### Phase 4: Introduce Managed Persistence Carefully

Goal:
Support hosted project ownership and collaboration without immediately redesigning the domain model around a database.

Recommended starting point:

- Store project metadata relationally.
- Store access control relationally.
- Store the project document itself as a versioned artifact.
- Add concurrency control and version checks at the document boundary.

This is intentionally conservative.

It allows the product to gain managed storage, ownership, and authorization without prematurely converting every planning concept into database-first entities.

Deliverable:
A hosted storage model that remains compatible with the existing document pipeline.

Success criteria:

- A hosted project can still round-trip through the existing document format.
- Authorization and ownership are enforceable without changing the planning model everywhere.
- The system retains flexibility for a more normalized persistence model later if needed.

### Phase 5: Decide On Rich Browser UX

Goal:
Choose the browser client approach only after the core and server boundaries are proven.

Decision options:

- keep the web host focused on administrative and document workflows
- add a modest browser editor using a .NET-first UI approach
- build a richer browser editor with a separate SPA if interaction demands justify it

The key point is timing.

The UI stack choice should follow validated product needs, not lead them.

## Architectural Guidance

### Recommended Early Shape

- `Zametek.Common.ProjectPlan` remains the source of core domain concepts.
- `Zametek.Data.ProjectPlan` remains the source of versioned serialization and upgrade behavior.
- A new headless project can own extracted planning workflows and orchestration logic.
- A new ASP.NET Core project can act as a thin host once the headless core is proven.

### What To Avoid Early

- Do not port desktop MVVM structures directly into web handlers or browser components.
- Do not force the desktop app to adopt a new architecture all at once.
- Do not replace the existing composition root solely for architectural symmetry.
- Do not introduce a separate SPA before there is a stable server-side capability worth exposing.
- Do not make relational persistence the prerequisite for every web experiment.

## Maintainer-Focused Rationale

This proposal is intentionally designed to be easier to approve.

It asks for a limited set of changes that are technically useful even if the web effort never progresses beyond an internal spike. That matters, because the maintainer is not being asked to trade a working product for a speculative platform plan.

The proposal is also reversible:

- If Phase 1 succeeds, the codebase becomes cleaner and more reusable.
- If Phase 3 succeeds, the project gains a real alternate host.
- If later web investment proves unjustified, the desktop app still benefits from the refactoring and added tests.

That is the right shape of risk for an established, working application.

## Suggested Initial Scope For Approval

The first approval request should be deliberately narrow.

Recommended ask:

1. Add a small headless planning services project.
2. Add characterization tests for open, import, compile, save, and export workflows.
3. Refactor the desktop and command-line hosts to consume the extracted services without behavior change.
4. Stop there and evaluate before committing to a server host or browser UI.

That is a credible, low-risk starting point.

## Decision Gates

Each phase should have an explicit go or no-go review.

### Gate 1: After Headless Extraction

Proceed only if:

- desktop behavior is unchanged
- command-line behavior is unchanged
- sample project outputs remain equivalent

### Gate 2: After Test Hardening

Proceed only if:

- regression coverage is strong enough to support further change
- document compatibility is protected

### Gate 3: After Minimal Web Host Spike

Proceed only if:

- the host runs real workflows end to end
- the server boundary feels natural rather than forced
- operational complexity remains justified by the value demonstrated

### Gate 4: Before Rich Browser UI

Proceed only if:

- there is clear evidence that a browser editor is worth building
- interaction requirements justify the chosen front-end stack
- the backend boundary is already stable

## Final Position

The long-term direction toward a web-capable product is viable.

However, the correct first move is not a platform expansion. It is a careful extraction and proof-of-equivalence exercise centered on the existing product, the existing file model, and the existing workflows that users already depend on.

If the project must not break what already works, then the web strategy must earn trust incrementally.

That is what this proposal is designed to do.
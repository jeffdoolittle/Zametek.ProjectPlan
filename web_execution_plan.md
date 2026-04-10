# Web Execution Plan

## Purpose

This document is the execution source of truth for the web enablement effort.

It exists to translate the strategy in `web_proposal.md` into an actionable sequence of work that can be tracked through GitHub issues without losing the core constraint:

The existing desktop application must continue to work throughout.

This document should be used as the reference for:

- the master tracking issue
- the first tranche of child issues
- milestone and dependency decisions
- go or no-go reviews between phases

## Program Objective

Make the project planning core safely hostable outside the current Avalonia desktop shell, then prove that capability through a thin web host, while preserving desktop behavior and project-file compatibility.

## Non-Negotiable Constraints

- No regression in existing desktop workflows.
- No break in project file compatibility or version-upgrade behavior.
- Desktop remains a first-class supported host.
- All architectural change must be additive and reversible until proven.
- Each phase must produce value even if the program stops there.

## Source Documents

- `web_critical_assessment.md` explains why a broad rewrite-style web plan is too risky.
- `web_proposal.md` explains the recommended strategy and rationale.
- This document defines execution order, dependencies, issue scope, and gates.

## Execution Model

The program should proceed in controlled tranches.

Only the issues needed for the current phase should be opened initially. Later-phase issues should remain documented here until the previous gate is passed.

This prevents backlog sprawl, keeps attention on the actual critical path, and avoids implying commitment to architecture that has not yet been proven.

## Milestones

### Milestone 1: Protect Existing Behavior

Goal:
Create a reliable regression shield around the workflows that must not change.

Primary output:
A representative project corpus and characterization tests.

Exit condition:
The project can detect material behavior drift in core workflows before extraction work continues.

### Milestone 2: Extract Headless Core

Goal:
Move genuinely UI-independent planning workflows behind headless interfaces and services.

Primary output:
A reusable planning kernel consumed by desktop and command-line hosts.

Exit condition:
Desktop and command-line hosts use extracted services without material behavior change.

### Milestone 3: Prove Minimal Web Host

Goal:
Stand up a new ASP.NET Core composition root that exercises real planning workflows.

Primary output:
A thin authenticated web host that can upload, compile, save, and download project artifacts.

Exit condition:
The same planning behavior runs correctly under desktop, command-line, and server hosts.

### Milestone 4: Add Hosted Project Management

Goal:
Add project ownership, persistence, authorization, and concurrency control without prematurely redesigning the domain model.

Primary output:
A hosted project model using document-backed persistence with managed metadata and access control.

Exit condition:
Hosted projects can be stored, protected, revised, and round-tripped safely.

### Milestone 5: Decide Browser UX Direction

Goal:
Choose whether the project needs a richer browser experience and what stack is justified.

Primary output:
A grounded product and technology decision backed by real hosted workflow experience.

Exit condition:
The project has enough evidence to justify either staying thin or investing in richer browser editing.

## Critical Path

This is the minimum sequence required to reach a credible web proof point without destabilizing the existing application.

1. Establish a representative regression corpus.
2. Define the non-regression contract for core workflows.
3. Add characterization tests for open, import, compile, save, and export behavior.
4. Introduce headless planning service interfaces.
5. Extract compile orchestration into a headless service.
6. Extract file load, import, save, and export orchestration into headless services.
7. Move the desktop application onto the extracted services.
8. Move the command-line host onto the extracted services.
9. Verify output equivalence across desktop and command-line hosts.
10. Introduce a minimal ASP.NET Core host.
11. Add authenticated upload, compile, save, and download workflows.
12. Add hosted project storage with concurrency protection.
13. Add tenant membership and authorization controls.
14. Dogfood the hosted workflow and evaluate whether richer browser UX is warranted.

Nothing outside this sequence should be allowed to dominate the program before these steps are complete.

## Phase Plan

### Phase A: Safety Rails

Intent:
Make regressions visible before refactoring begins.

Planned issues:

- Establish representative project corpus for regression testing.
- Define non-regression acceptance contract for core workflows.
- Add characterization tests for open, import, compile, save, and export.
- Document current workflow boundaries and desktop coupling points.

Required before next phase:

- Sample inputs are collected and stable.
- Core behavior expectations are written down.
- Regression suite can detect material output drift.

### Phase B: Headless Extraction

Intent:
Separate planning workflows from host-specific UI concerns.

Planned issues:

- Introduce headless planning service interfaces.
- Extract compile orchestration into a headless service without behavior change.
- Extract file load, import, save, and export orchestration into headless services.
- Refactor desktop host to consume extracted services.
- Refactor command-line host to consume extracted services.
- Verify output equivalence across hosts.

Required before next phase:

- Desktop host still behaves as before.
- Command-line host still behaves as before.
- Cross-host outputs are materially equivalent for known samples.

### Phase C: Thin Web Host

Intent:
Prove the extracted core can live behind an ASP.NET Core boundary.

Planned issues:

- Create minimal ASP.NET Core web host composition root.
- Add authenticated upload, compile, save, and download endpoints.
- Add internal workflow shell for exercising hosted operations.
- Add operational logging for hosted workflow actions.

Required before next phase:

- Web host runs real planning workflows end to end.
- No desktop-only assumptions leak into the host.
- Operational complexity remains proportionate to demonstrated value.

### Phase D: Hosted Project Management

Intent:
Add persistence and access control conservatively.

Planned issues:

- Add hosted project document storage and metadata model.
- Add optimistic concurrency and version checks.
- Add tenant, membership, and project access model.
- Add auditability for hosted project actions.

Required before next phase:

- Hosted projects can round-trip safely.
- Access control is enforced correctly.
- Revision conflicts are detectable and manageable.

### Phase E: Browser UX Decision

Intent:
Choose whether a richer browser experience is justified.

Planned issues:

- Run structured dogfood evaluation of hosted workflow.
- Decide browser-client direction from validated use cases.
- If justified, run spike for richer graph and schedule editing.

Required before continuation:

- Evidence shows whether a thin hosted workflow is sufficient.
- Front-end technology choices are tied to demonstrated needs, not assumptions.

## Initial Tranche Of Issues

Only these issues should be created at the start.

### WEB-001 Establish representative project corpus for regression testing

Purpose:
Create the input set that all later regression and equivalence checks depend on.

Acceptance criteria:

- Includes representative project files covering normal, edge, and historical version cases where possible.
- Includes files for open, compile, save, import, and export scenarios.
- Corpus is documented and suitable for automated testing.

Dependencies:
None.

### WEB-002 Define non-regression acceptance contract for core workflows

Purpose:
Make “do not break existing behavior” concrete enough to evaluate.

Acceptance criteria:

- Core workflows and expected invariants are written down.
- The contract distinguishes material behavior change from acceptable internal refactoring.
- The contract is referenced by later extraction issues.

Dependencies:
WEB-001.

### WEB-003 Add characterization tests for open, import, compile, save, and export

Purpose:
Protect existing behavior before extraction work begins.

Acceptance criteria:

- Tests exercise real workflow behavior using the representative corpus.
- Failures clearly indicate output drift.
- Existing document upgrade behavior remains protected.

Dependencies:
WEB-001, WEB-002.

### WEB-004 Document current workflow boundaries and desktop coupling points

Purpose:
Clarify what can realistically be extracted and what should remain host-specific.

Acceptance criteria:

- Current core workflows are mapped.
- UI-bound and headless-capable areas are distinguished.
- Known coupling risks are captured for extraction planning.

Dependencies:
None.

### WEB-005 Introduce headless planning service interfaces

Purpose:
Define the first stable headless contracts for planning workflows.

Acceptance criteria:

- Interfaces cover core compile and document workflows.
- Interfaces are shaped for reuse across desktop, command-line, and future web hosts.
- No forced UI assumptions leak into the contracts.

Dependencies:
WEB-002, WEB-004.

### WEB-006 Extract compile orchestration into headless service without behavior change

Purpose:
Move compile-related orchestration behind the new headless boundary.

Acceptance criteria:

- Compile behavior remains materially unchanged for representative inputs.
- Service can run outside the desktop shell.
- Desktop host can consume the service without user-visible regression.

Dependencies:
WEB-003, WEB-005.

### WEB-007 Extract file load, import, save, and export orchestration into headless services

Purpose:
Move document workflow orchestration behind the same headless boundary.

Acceptance criteria:

- Load, import, save, and export behaviors remain materially unchanged.
- Services are callable without Avalonia host assumptions.
- Existing file compatibility is preserved.

Dependencies:
WEB-003, WEB-005.

### WEB-008 Refactor desktop host to consume extracted services

Purpose:
Make the desktop app the first validating consumer of the new headless core.

Acceptance criteria:

- Existing desktop behavior remains intact.
- Existing packaging and startup behavior remain intact.
- Regression suite passes with the desktop host using extracted services.

Dependencies:
WEB-006, WEB-007.

### WEB-009 Refactor command-line host to consume extracted services

Purpose:
Use the current non-UI host as proof that the extraction is genuinely headless.

Acceptance criteria:

- Existing command-line workflows remain intact.
- Outputs remain materially equivalent for representative inputs.
- Host composition no longer depends on desktop-only assumptions for core workflows.

Dependencies:
WEB-006, WEB-007.

### WEB-010 Verify output equivalence across desktop and command-line hosts

Purpose:
Confirm that extraction preserved behavior before any server host is added.

Acceptance criteria:

- Known sample inputs produce materially equivalent outputs across hosts.
- Differences, if any, are documented and justified.
- The result is accepted as the gate to the minimal web host phase.

Dependencies:
WEB-008, WEB-009.

## Later Issues To Keep Deferred Until Gate Passage

These should remain documented here, but not necessarily created as active issues until Milestone 2 is complete.

- Create minimal ASP.NET Core host composition root.
- Add authenticated upload, compile, save, and download workflows.
- Add hosted project document storage and metadata model.
- Add optimistic concurrency and version checks.
- Add tenant, membership, and project access model.
- Add audit logging for hosted actions.
- Run dogfood evaluation of hosted workflow.
- Decide browser-client direction.
- Spike richer browser-based graph and schedule editing.

## Explicit Non-Goals For Early Phases

The following items are intentionally out of scope until the core path is proven:

- replacing the desktop application
- forcing a shared UI abstraction across desktop and web
- choosing React or Blazor up front
- normalizing the entire project-planning model into relational tables
- adding billing, notifications, or deep SaaS operational features
- building a rich browser editor before hosted workflows are validated

## Decision Gates

### Gate 1: Safety Rails Complete

Open the next tranche only if:

- regression corpus exists
- non-regression contract exists
- characterization tests detect meaningful drift

### Gate 2: Headless Core Proven

Open the web-host tranche only if:

- desktop host uses extracted services successfully
- command-line host uses extracted services successfully
- outputs remain materially equivalent across hosts

### Gate 3: Minimal Web Host Proven

Open the hosted project management tranche only if:

- real server-hosted workflows run end to end
- server boundary feels sustainable
- added complexity is justified by demonstrated value

### Gate 4: Hosted Workflow Validated

Open rich browser UX work only if:

- hosted workflow has been exercised in practice
- user needs justify a richer browser interface
- backend and persistence boundaries are already stable enough to support it

## Label Scheme

Suggested labels:

- `critical-path`
- `risk-reduction`
- `desktop-compat`
- `headless-core`
- `web-host`
- `persistence`
- `auth`
- `spike`
- `deferred`

## Milestone Scheme

Suggested milestones:

- `M1 - Protect Existing Behavior`
- `M2 - Extract Headless Core`
- `M3 - Prove Minimal Web Host`
- `M4 - Add Hosted Project Management`
- `M5 - Decide Browser UX`

## How To Use This Document

1. Create a master issue that links to this plan and to the proposal documents.
2. Create only the initial tranche of issues listed here.
3. Use the decision gates to control when additional issues may be opened.
4. Update this document if the program materially changes direction.
5. Do not treat deferred items as approved work until the relevant gate is passed.

## Final Rule

If a proposed issue does not clearly support the current milestone or the critical path, it should stay out of the active backlog.

That rule is how this effort stays disciplined and avoids becoming a speculative rewrite program.
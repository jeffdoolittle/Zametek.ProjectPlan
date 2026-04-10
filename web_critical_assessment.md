# Critical Assessment of the Web Plan

## Verdict

I would not take the current plan to the maintainer as written.

It is directionally sound, but it bundles too many long-term moves into the first conversation, and that makes it read like a parallel product rewrite rather than a conservative extension of an already working application.

The strongest parts of the assessment are the warnings not to port desktop MVVM into the browser and not to contaminate the current desktop composition root. Those are correct.

The weak point is sequencing: for a project with a hard non-breaking constraint, this plan still front-loads too much architectural tax before proving that the existing behavior can be preserved.

## Critical Assessment

- The proposal understates how entangled the current orchestration layer is. `CoreViewModel` is not a small, easily extractable application service; it is a large reactive state and workflow hub.
- The proposal also understates how UI-shaped the current view-model layer is. `Zametek.ViewModel.ProjectPlan` already pulls in Avalonia, Dock, ReactiveUI, and charting packages, so “reusing parts of the view-model project” is much less safe than the assessment makes it sound.
- The biggest political risk is the early React recommendation. It may be the right end-state for a rich browser editor, but putting React in the first architectural pitch makes the proposal harder to sell to a maintainer who is currently shipping a .NET desktop product. Technically reasonable does not mean strategically acceptable.
- The assessment jumps too quickly from “web app” to “full SaaS platform.” Multi-tenancy, IAM, relational persistence, background jobs, audit, and a separate SPA are all valid concerns, but presenting them together creates scope shock. A maintainer will hear “new product, new stack, new ops burden.”
- It misses the most important bridge already present in the repository: there is already a headless command-line composition root. That is a major asset because it proves some core workflows already run outside the Avalonia shell.
- The current file pipeline is stronger than the assessment credits. Opening, upgrading, saving, importing, and exporting already exist. That is the safest place to start web-enablement.
- The repository also already has a versioned document model and upgrade chain, with tests around the converter. That argues for keeping the project document as the canonical persisted artifact longer than the assessment suggests.
- The plan does not define a real regression shield. There is meaningful testing around data-version conversion, but almost none around the orchestration behavior. Extracting logic from the current orchestration layer without characterization tests is exactly how you break what already works.

## What I Would Change

- Reframe this from “build a SaaS architecture” to “make the core product safely hostable outside the desktop shell.”
- Remove the hard React recommendation from the initial pitch. Keep the browser-client decision open until the headless core and server boundary are proven. If React is mentioned at all, position it as a later option for a rich editor, not as an immediate commitment.
- Do not introduce Application, Infrastructure, Persistence, Web, and Client projects all at once. That is clean on paper and expensive in trust. Start with one narrowly scoped headless services project, then add an ASP.NET Core host only after the desktop app is consuming those services successfully.
- Keep the project document model as the canonical source of truth for the first web milestone. A relational database can store ownership, tenant, metadata, permissions, version, and document blobs. That gives multi-user hosting without immediately redesigning every planning concept into tables.
- Make the desktop app the first consumer of every extraction. If a service cannot be adopted by the existing desktop app without behavior change, it is not ready for a web host.
- Add characterization tests before extraction. The minimum bar is golden-master coverage for open, import, compile, export, and save across a representative set of project files. If the same inputs do not produce materially identical outputs before and after refactoring, the change should not ship.

## Safer Roadmap

- Phase 1 should be internal only: extract a small headless planning kernel behind stable interfaces, but keep all current desktop behavior and packaging intact.
- Phase 2 should use the existing command-line path as the benchmark. The same project file should compile and export identically in desktop, command-line, and service-hosted execution.
- Phase 3 should add a thin ASP.NET Core host for authenticated project upload, compile, save, and download. No rich browser editor yet.
- Phase 4 should add tenant membership, authorization, and project storage once the service boundary is proven.
- Phase 5 should decide whether a browser editor justifies React, Blazor, or a mixed approach.

## How To Get The Maintainer On Board

- Sell continuity, not replacement. The pitch should say the desktop app remains first-class and remains shippable throughout.
- Sell risk removal, not architecture purity. The first milestone should be “same engine, another host,” not “new SaaS platform.”
- Sell reversible steps. Every phase should be independently useful and stoppable without leaving the codebase in a half-rewritten state.
- Sell preservation of the current project format and behavior. That matters more to trust than any diagram.
- Avoid proposing a second mandatory skill stack on day one. A maintainer can accept a future React option more easily than an immediate React mandate.

## Conclusion

The assessment is strategically right about the destination, but too aggressive about the opening move.

If the non-breaking requirement is absolute, the first approved plan should be smaller, more document-centric, and explicitly built around proving equivalence with the current desktop and command-line behavior before any serious web-product expansion.
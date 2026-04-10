# Web Issue Drafts

## Purpose

This document contains ready-to-paste GitHub issue drafts for the web enablement effort.

It is intended for use on the fork issue tracker and is aligned with:

- `web_critical_assessment.md`
- `web_proposal.md`
- `web_execution_plan.md`

The structure is deliberately conservative:

- one master tracking issue
- one initial tranche of critical-path child issues
- later phases remain deferred until the relevant gate is passed

## Suggested Labels

- `critical-path`
- `risk-reduction`
- `desktop-compat`
- `headless-core`
- `web-host`
- `persistence`
- `auth`
- `spike`
- `deferred`

## Suggested Milestones

- `M1 - Protect Existing Behavior`
- `M2 - Extract Headless Core`
- `M3 - Prove Minimal Web Host`
- `M4 - Add Hosted Project Management`
- `M5 - Decide Browser UX`

## Master Issue

### Title

Web Enablement Program: Safe Incremental Path Without Desktop Regression

### Suggested labels

- `critical-path`
- `risk-reduction`
- `desktop-compat`

### Suggested milestone

None, or create a dedicated umbrella milestone if preferred.

### Body

## Objective

Establish a safe, incremental path toward web-hosted capabilities for Zametek.ProjectPlan without regressing the current desktop application and without forcing premature architectural commitments.

This program is specifically designed to preserve what already works while making the core planning workflows usable outside the current Avalonia desktop shell.

## Non-Negotiable Constraints

- No regression in existing desktop workflows.
- No break in project file compatibility or version-upgrade behavior.
- Desktop remains a first-class supported host.
- Architectural changes must be additive and reversible until proven.
- Each phase must provide value even if the program stops there.

## Source Material

- `web_critical_assessment.md`
- `web_proposal.md`
- `web_execution_plan.md`

## Program Shape

This effort is intentionally staged.

The initial focus is not a full SaaS rollout and not a rich browser editor.

The first goal is to protect existing behavior, extract a genuinely headless planning core, and prove it through the existing desktop and command-line hosts before adding a minimal web host.

## Critical Path

1. Establish a representative regression corpus.
2. Define the non-regression contract for core workflows.
3. Add characterization tests for open, import, compile, save, and export behavior.
4. Introduce headless planning service interfaces.
5. Extract compile orchestration into a headless service.
6. Extract file load, import, save, and export orchestration into headless services.
7. Move the desktop application onto the extracted services.
8. Move the command-line host onto the extracted services.
9. Verify output equivalence across hosts.
10. Only then open the minimal web host tranche.

## Decision Gates

### Gate 1: Safety Rails Complete

Proceed only if:

- a representative project corpus exists
- a non-regression contract exists
- characterization tests can detect meaningful behavior drift

### Gate 2: Headless Core Proven

Proceed only if:

- desktop host uses extracted services successfully
- command-line host uses extracted services successfully
- outputs remain materially equivalent across hosts

### Gate 3: Minimal Web Host Proven

Proceed only if:

- real hosted workflows run end to end
- the server boundary feels sustainable
- added complexity is justified by demonstrated value

## Initial Tranche

- [ ] WEB-001 Establish representative project corpus for regression testing
- [ ] WEB-002 Define non-regression acceptance contract for core workflows
- [ ] WEB-003 Add characterization tests for open, import, compile, save, and export
- [ ] WEB-004 Document current workflow boundaries and desktop coupling points
- [ ] WEB-005 Introduce headless planning service interfaces
- [ ] WEB-006 Extract compile orchestration into headless service without behavior change
- [ ] WEB-007 Extract file load, import, save, and export orchestration into headless services
- [ ] WEB-008 Refactor desktop host to consume extracted services
- [ ] WEB-009 Refactor command-line host to consume extracted services
- [ ] WEB-010 Verify output equivalence across desktop and command-line hosts

## Deferred Until Gate Passage

The following work remains intentionally deferred until Gate 2 is passed:

- minimal ASP.NET Core host
- authenticated upload, compile, save, and download workflows
- hosted project document storage
- concurrency control
- tenant membership and authorization
- richer browser UX decisions

## Success Condition For This Master Issue

This issue remains open until the project either:

- completes the staged program successfully, or
- explicitly decides to stop after an earlier milestone with documented rationale

## Child Issue Drafts

### WEB-001

#### Title

Establish representative project corpus for regression testing

#### Suggested labels

- `critical-path`
- `risk-reduction`
- `desktop-compat`

#### Suggested milestone

- `M1 - Protect Existing Behavior`

#### Body

## Objective

Create a representative set of project inputs that can be used to detect behavior drift during extraction and host expansion work.

## Why

The web effort cannot proceed safely without a stable regression corpus covering the workflows that already work today.

## Scope

- collect representative project files
- include normal and edge cases where practical
- include historical version cases where practical
- identify scenarios covering open, import, compile, save, and export behavior

## Acceptance Criteria

- corpus includes representative project files for the primary workflows
- corpus is documented and organized for automated test use
- historical version coverage is included where feasible
- corpus is acceptable as the baseline input set for later regression checks

## Out Of Scope

- broad test harness design
- workflow refactoring
- web host work

## Dependencies

None.

### WEB-002

#### Title

Define non-regression acceptance contract for core workflows

#### Suggested labels

- `critical-path`
- `risk-reduction`
- `desktop-compat`

#### Suggested milestone

- `M1 - Protect Existing Behavior`

#### Body

## Objective

Define what “do not break existing behavior” means in concrete, reviewable terms for the core workflows.

## Why

Without a written contract, refactoring decisions will drift and regression debates will become subjective.

## Scope

- identify the core workflows that must remain stable
- define the key observable behaviors and invariants for those workflows
- distinguish material behavior change from acceptable internal refactoring
- create a contract that later extraction work can reference directly

## Acceptance Criteria

- core workflows and expected invariants are documented
- the contract clearly identifies what counts as a regression
- the contract is suitable for linking from later child issues

## Out Of Scope

- implementing tests
- extracting services
- designing future web workflows

## Dependencies

- WEB-001

### WEB-003

#### Title

Add characterization tests for open, import, compile, save, and export

#### Suggested labels

- `critical-path`
- `risk-reduction`
- `desktop-compat`

#### Suggested milestone

- `M1 - Protect Existing Behavior`

#### Body

## Objective

Add characterization tests that lock in the current behavior of the key project workflows before refactoring begins.

## Why

Extraction work without characterization tests is the most likely route to breaking the current application.

## Scope

- exercise open behavior against the representative corpus
- exercise import behavior for supported inputs where practical
- exercise compile behavior on known project cases
- exercise save behavior against current project format expectations
- exercise export behavior for supported output formats where practical

## Acceptance Criteria

- tests run against the representative corpus
- failures clearly indicate behavior drift
- document upgrade behavior remains protected
- tests are usable as a gate for later extraction issues

## Out Of Scope

- changing workflow behavior intentionally
- adding web host features

## Dependencies

- WEB-001
- WEB-002

### WEB-004

#### Title

Document current workflow boundaries and desktop coupling points

#### Suggested labels

- `risk-reduction`
- `desktop-compat`

#### Suggested milestone

- `M1 - Protect Existing Behavior`

#### Body

## Objective

Map the existing workflow boundaries and identify where the current implementation is tightly coupled to desktop-specific concerns.

## Why

The extraction plan needs a realistic view of what is genuinely headless-capable and what remains host-specific.

## Scope

- identify current orchestration hotspots
- distinguish UI-bound behavior from host-agnostic workflow behavior
- capture known extraction risks and constraints

## Acceptance Criteria

- current core workflows are mapped clearly
- desktop-only assumptions are identified
- candidate headless extraction boundaries are documented

## Out Of Scope

- actual extraction work
- web host design

## Dependencies

None.

### WEB-005

#### Title

Introduce headless planning service interfaces

#### Suggested labels

- `critical-path`
- `headless-core`
- `desktop-compat`

#### Suggested milestone

- `M2 - Extract Headless Core`

#### Body

## Objective

Define the first stable headless service contracts for planning workflows that need to be reused across hosts.

## Why

The project needs explicit headless contracts before core orchestration can be safely moved out of host-shaped structures.

## Scope

- define service interfaces for compile and document workflows
- shape interfaces for reuse by desktop, command-line, and future web hosts
- ensure contracts do not encode UI assumptions

## Acceptance Criteria

- interfaces cover the initial core workflow surface
- interfaces are host-agnostic
- later extraction issues can target these interfaces directly

## Out Of Scope

- full implementation of extracted services
- web host composition

## Dependencies

- WEB-002
- WEB-004

### WEB-006

#### Title

Extract compile orchestration into headless service without behavior change

#### Suggested labels

- `critical-path`
- `headless-core`
- `desktop-compat`

#### Suggested milestone

- `M2 - Extract Headless Core`

#### Body

## Objective

Move compile-related orchestration behind a headless service boundary while preserving current behavior.

## Why

Compile orchestration is a core prerequisite for any future non-desktop host.

## Scope

- move compile orchestration into a headless service implementation
- keep current desktop behavior materially unchanged
- ensure the extracted behavior is callable outside the desktop shell

## Acceptance Criteria

- compile behavior remains materially unchanged for representative inputs
- desktop host can consume the extracted service without user-visible regression
- extracted service can run independently of desktop UI assumptions

## Out Of Scope

- broad workflow redesign
- file import or export extraction
- web host work

## Dependencies

- WEB-003
- WEB-005

### WEB-007

#### Title

Extract file load, import, save, and export orchestration into headless services

#### Suggested labels

- `critical-path`
- `headless-core`
- `desktop-compat`

#### Suggested milestone

- `M2 - Extract Headless Core`

#### Body

## Objective

Move the project document workflows behind headless service boundaries while preserving compatibility and behavior.

## Why

The future web host depends on project document workflows that can run outside the current desktop shell.

## Scope

- extract load behavior
- extract import behavior
- extract save behavior
- extract export behavior
- preserve current project format compatibility

## Acceptance Criteria

- load, import, save, and export behavior remain materially unchanged
- services can run without Avalonia-specific assumptions
- existing document compatibility is preserved

## Out Of Scope

- compile extraction
- hosted persistence
- web host implementation

## Dependencies

- WEB-003
- WEB-005

### WEB-008

#### Title

Refactor desktop host to consume extracted services

#### Suggested labels

- `critical-path`
- `desktop-compat`
- `headless-core`

#### Suggested milestone

- `M2 - Extract Headless Core`

#### Body

## Objective

Make the desktop application the first validating consumer of the extracted headless services.

## Why

The extraction is not successful unless the current desktop host can adopt it without regression.

## Scope

- replace direct host-owned orchestration with extracted services where applicable
- preserve current application startup and packaging behavior
- keep user-visible behavior intact

## Acceptance Criteria

- existing desktop behavior remains intact
- existing startup and packaging behavior remain intact
- regression suite passes with the desktop host using extracted services

## Out Of Scope

- browser UI work
- hosted persistence
- intentional desktop behavior change

## Dependencies

- WEB-006
- WEB-007

### WEB-009

#### Title

Refactor command-line host to consume extracted services

#### Suggested labels

- `critical-path`
- `headless-core`

#### Suggested milestone

- `M2 - Extract Headless Core`

#### Body

## Objective

Use the current command-line host to prove that the extracted workflow layer is genuinely reusable outside the desktop shell.

## Why

The command-line host is the most immediate non-UI proof point for successful headless extraction.

## Scope

- update command-line composition to use extracted services
- preserve current command-line workflow behavior
- remove unnecessary dependence on desktop-shaped assumptions for core workflows

## Acceptance Criteria

- existing command-line workflows remain intact
- outputs remain materially equivalent for representative inputs
- command-line host uses extracted services successfully

## Out Of Scope

- web host implementation
- new command-line features

## Dependencies

- WEB-006
- WEB-007

### WEB-010

#### Title

Verify output equivalence across desktop and command-line hosts

#### Suggested labels

- `critical-path`
- `risk-reduction`
- `desktop-compat`

#### Suggested milestone

- `M2 - Extract Headless Core`

#### Body

## Objective

Confirm that the extracted core preserves materially equivalent behavior across the current desktop host and the command-line host.

## Why

No web host work should begin until the extraction has been proven across existing hosts.

## Scope

- compare known sample outputs across desktop and command-line execution paths
- document any differences that remain
- determine whether the result satisfies the gate to the next milestone

## Acceptance Criteria

- representative sample inputs produce materially equivalent outputs across hosts
- any remaining differences are documented and justified
- result is accepted as the gate for opening minimal web host work

## Out Of Scope

- building the web host
- adding new workflow behavior

## Dependencies

- WEB-008
- WEB-009

## Filing Order Recommendation

Create the issues in this order:

1. Master issue
2. WEB-001
3. WEB-002
4. WEB-003
5. WEB-004
6. WEB-005
7. WEB-006
8. WEB-007
9. WEB-008
10. WEB-009
11. WEB-010

## Final Note

Do not file later-phase issues yet.

The discipline of this effort depends on keeping the active backlog limited to the current gate and the immediate critical path.
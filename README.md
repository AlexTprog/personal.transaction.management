# personal.transaction.management

## Database Migrations

**Generate a migration** (run from the solution root):

```bash
dotnet ef migrations add <MigrationName>   --project personal.transaction.management.infrastructure   --startup-project personal.transaction.management.api   --output-dir Persistence/Migrations
```

**Apply migrations to the database:**

```bash
dotnet ef database update   --project personal.transaction.management.infrastructure   --startup-project personal.transaction.management.api
```

**Revert the last migration:**

```bash
dotnet ef migrations remove   --project personal.transaction.management.infrastructure   --startup-project personal.transaction.management.api
```

> `dotnet-ef` must be installed globally: `dotnet tool install --global dotnet-ef`

## Running with Docker Compose

```bash
# 1. Copy and fill in the required secrets
cp .env.example .env

# 2. Build and start all services
docker compose up --build

# API → http://localhost:8080
# Scalar UI → http://localhost:8080/scalar/v1
```

## Roadmap / Future work

Deliberately out of scope for this version. Each item is a candidate, not a commitment.

| Idea | Why it is interesting | What it would need |
|---|---|---|
| **Cash flow projection** | Linear projection of an account balance N days ahead from the last 3 months of income/expense rate. Small feature, good discussion on assumptions and limits. | A read-side query in Application, no domain change. |
| **Audit log endpoint** | `GET /api/audit-log` backed by the `CreatedBy/ModifiedBy` fields that already exist on every entity. | A query and a paginated response model. |
| **Multi-currency reports** | `ExchangeRate` is already stored on transfers. Normalizing all amounts to a base currency enables cross-account reports. | A rate source and a rule for which rate applies to which date. |
| **Webhook notifications** | Outbound events for budget thresholds. The outbox is the natural place to guarantee delivery with retry. | An outbound HTTP client, retry policy, and a first real consumer for the outbox (see ADR-003). |
| **OpenTelemetry tracing** | Spans around the MediatR pipeline behaviors. | `ActivitySource` and an exporter. |

Not planned: CSV/PDF export, rate limiting, recurring-transaction detection. Useful, but they do not add new architectural decisions to the project.


# Architecture Decision Records

Short records of the decisions that shape this API. Each one lists the alternatives that were considered and the trade-offs accepted.

---

## ADR-001: Account balance is stored and updated through domain events

**Status:** Accepted

**Context**
Every account needs its current balance on read paths (dashboards, reports). Computing it as `SUM(transactions)` on every request gets more expensive as history grows.

**Decision**
`Account.Balance` is persisted. Transaction commands raise domain events, and a handler applies the balance change. The Application layer never mutates a balance directly.

**Alternatives considered**
- *Compute balance from transactions on demand:* always consistent, but read cost grows with history.
- *Update balance directly in the command handler:* simpler, but couples every use case to account rules and makes the rule easy to bypass.

**Consequences**
- (+) Balance reads are O(1).
- (+) The balance rule lives in one place.
- (−) Balance is derived state: it needs a reconciliation query (`SUM(transactions)` vs `Balance`) to detect drift.
- (−) Consistency depends on the event handler running in the same unit of work as the transaction write.

---

## ADR-002: Transfers are two linked transactions (`TransferId`)

**Status:** Accepted

**Context**
Moving money between accounts, possibly in different currencies, affects two balances. The model has to support cross-currency transfers with an exchange rate.

**Decision**
A transfer is two `Transaction` rows (`TransferOut` and `TransferIn`) sharing the same `TransferId`. `ExchangeRate` is required when the two accounts have different currencies. Both legs are created, modified and deleted together.

**Alternatives considered**
- *A single `Transfer` entity with source and destination accounts:* fewer rows, but every report must special-case transfers.

**Consequences**
- (+) Each leg behaves like a normal transaction in reports and balance logic.
- (+) Per-account history is naturally complete.
- (−) The invariant "both legs exist and share a `TransferId`" must be enforced explicitly.
- (−) Deletes and edits must be atomic across both legs.

---

## ADR-003: Domain events without a consumer are not raised; the outbox is only for events that need reliable delivery

**Status:** Accepted

**Context**
An audit of the codebase found events being written to the outbox table even though no handler consumed them. This made the table noisy: pending rows could not be told apart from rows that would never be processed, which defeats the purpose of an outbox (retry of failed deliveries).

**Decision**
- An event is only raised if at least one handler consumes it today.
- The outbox stores only events whose handling has side effects outside the current transaction and needs retry.
- New events are added when a consumer exists, not in advance.

**Alternatives considered**
- *Keep raising all events and filter at persistence time:* keeps the domain open for future consumers, but leaves unused events in the model.
- *Separate domain events (in-process) from integration events (outbox):* cleaner in theory, more code than this project justifies.

**Consequences**
- (+) The outbox table means "work pending delivery", nothing else.
- (+) The event model reflects real behavior, not speculation.
- (−) Adding a consumer later means touching the aggregate to raise the event.

---

## ADR-004: Idempotency keys on write endpoints

**Status:** Accepted

**Context**
Clients retry on timeouts. Without protection, a retried `POST /transactions` creates a duplicate transaction and corrupts the balance.

**Decision**
`POST`/`PUT` endpoints accept an `Idempotency-Key` header. The first result for a key is stored and returned for any retry with the same key instead of executing the command again.

**Alternatives considered**
- *Natural-key deduplication (amount + date + description):* produces false positives for legitimate repeated purchases.

**Consequences**
- (+) Safe retries, standard practice in payment APIs.
- (−) Needs storage for keys and a retention policy.
- (−) Clients must generate and reuse keys correctly.
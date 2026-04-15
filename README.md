# Distributed Computing Backend

A .NET 10 backend for communicating with an SMP (Secure Message Protocol) server. Exposes a REST API for session management and messaging, and includes a console client for direct interaction.

## Architecture

Clean layered architecture:

```
Backend.API            → HTTP controllers, filters, DI composition
Backend.Application    → Command/query handlers (use cases)
Backend.Domain         → Aggregates, value objects, domain logic
Backend.Infrastructure → SMP TCP/TLS client, in-memory repository
Backend.Console        → Interactive CLI client
```

## Prerequisites

- .NET 10 SDK
- Docker & Docker Compose (for containerised local dev)

## Running Locally

### With Docker Compose

Place the SMP server certificate at `./certs/smpserver.cer` (skip if you want cert validation bypassed in dev):

```bash
docker compose up -d --build
```

Services:
| Service | Host | Port |
|---|---|---|
| REST API | `localhost` | `1234` |
| SMP Server (echo) | `smp-server` | `8443` |

The API is reachable at `http://localhost:1234`. When connecting via the `/api/session/connect` endpoint from inside Docker, use `host: smp-server`.

### Without Docker

```bash
dotnet run --project Backend.API
# API listens on http://localhost:5000
```

### Console Client

```bash
dotnet run --project Backend.Console
```

Available commands: `connect`, `login`, `upload`, `download`, `messages`, `status`, `quit`.

## Configuration

| Key | Default | Description |
|---|---|---|
| `Smp:CertPath` | `certs/smpserver.cer` | Path to pinned SMP server certificate |
| `Cors:AllowedOrigins` | `http://localhost:3000` | Allowed frontend origins |

Environment variables use `__` as delimiter (e.g. `Smp__CertPath`).

## API Reference

### Session

#### `POST /api/session/connect`
Connect to an SMP server.

```json
{ "host": "smp-server", "port": 8443 }
```

#### `POST /api/session/authenticate`
Authenticate with the connected server.

```json
{ "username": "alice", "password": "secret" }
```

Returns `401` on invalid credentials.

#### `GET /api/session/status`
Get current session state.

```json
{ "isConnected": true, "isAuthenticated": true, "isClosed": false }
```

#### `DELETE /api/session`
Disconnect (`QUIT`) from the server.

---

### Messages

> All message endpoints require an authenticated session (`RequireSessionFilter`).

#### `POST /api/messages`
Upload a message to the server.

```json
{ "content": "hello world" }
```

Constraints: max 500 characters, no newlines. Returns `422` on validation failure.

#### `POST /api/messages/download`
Download messages from the server and cache them locally.

```json
[{ "id": "...", "content": "hello world", "receivedAt": "2026-04-15T10:00:00Z" }]
```

#### `GET /api/messages`
Return locally cached messages (previously downloaded).

```json
[{ "id": "...", "content": "hello world", "receivedAt": "2026-04-15T10:00:00Z" }]
```

---

### Error Responses

All failures return:

```json
{ "code": "auth.credentials.invalid", "message": "Username or password is incorrect." }
```

| Code | Meaning |
|---|---|
| `auth.credentials.invalid` | Wrong username or password |
| `auth.server_address.invalid` | Bad host or port |
| `auth.not_connected` | No active connection |
| `auth.not_authenticated` | Session not authenticated |
| `messages.content.invalid` | Message failed validation |

## SMP Protocol

Text-based protocol over TLS. Command flow:

```
→ HELO <username>
← 201 <challenge>

→ AUTH <username> <password>
← 200 OK

→ UPLD <content>
← 200 OK

→ DNLD
← 202 <message>
← 202 <message>
← 203 EndMessages

→ QUIT
← 200 OK
```

## Certificate Pinning

When `Smp:CertPath` points to an existing `.cer` file, the TLS connection validates the server certificate by comparing thumbprints. If the file is absent, validation is bypassed (useful for local dev without a cert).

## CI/CD

GitHub Actions workflow (`.github/workflows/docker-build.yml`) builds and pushes the image to GHCR on every push to `main`:

```
ghcr.io/<owner>/<repo>:latest
ghcr.io/<owner>/<repo>:sha-<commit>
```

No secrets required — uses the built-in `GITHUB_TOKEN`.

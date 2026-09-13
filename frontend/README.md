# UniMate AI — STEP 1: Foundation

Demo hackathon project. **Not** an official university system — all data is sample data.

## A. Architecture (big picture)

```
Browser (HTML + CSS + Vanilla JS)
        │  fetch() → JSON over HTTP
        ▼
ASP.NET Core Web API  (UniMateAI.Api)
        │
        ├── Controllers  → receive HTTP requests
        ├── Services     → business logic (GPA math, AI calls, etc.)
        ├── Data (EF Core DbContext) → talks to the database
        │
        ▼
SQLite (dev)  →  same EF Core code  →  SQL Server (production)
        │
        └── AIService → calls an external LLM API (OpenAI / Gemini / Groq)
             (API key lives ONLY in backend config, never in JS)
```

Two independent projects sit side by side in the solution:

- **UniMateAI.Web** — plain static files (HTML/CSS/JS). This is what
  a browser downloads and runs.
- **UniMateAI.Api** — the ASP.NET Core project. This is what actually
  talks to the database and the AI provider.

They talk to each other only through HTTP JSON requests (`fetch`).
The browser never touches the database or the AI key directly — that
is the whole point of having a backend.

## B. Technology stack

| Layer | Technology |
|---|---|
| Frontend | HTML5, CSS3, Vanilla JavaScript, Fetch API |
| Backend | C#, ASP.NET Core Web API, .NET 8 LTS |
| ORM | Entity Framework Core |
| Database (dev) | SQLite |
| Database (prod) | SQL Server |
| AI | OpenAI / Gemini / Groq via backend service (added from Step 10 onward) |

## C. Required software

Install these once, on your own machine:

1. **.NET SDK 8** — https://dotnet.microsoft.com/download
   Check it installed correctly:
   ```bash
   dotnet --version
   ```
   You should see something like `8.0.4xx`.
2. **A code editor** — Visual Studio Code (free) or Visual Studio
   Community. VS Code is fine for this whole project.
3. **A modern browser** — Chrome, Edge, or Firefox, for testing.
4. *(Optional, later steps)* **DB Browser for SQLite** — a free GUI
   to look inside your `.db` file once we create it.

You do **not** need Node.js, npm, or any JavaScript build tool. The
frontend is plain files opened by the browser — no compile step.

## D. Exact setup commands (create the ASP.NET Core project)

Run these in a terminal, **inside the folder where you keep your
projects** (e.g. `Documents/Projects`):

```bash
# 1. Create the solution folder and enter it
mkdir UniMateAI
cd UniMateAI

# 2. Create the solution file (this just groups projects together)
dotnet new sln -n UniMateAI

# 3. Create the Web API project (this is your C# backend)
dotnet new webapi -n UniMateAI.Api -o UniMateAI.Api

# 4. Add the API project to the solution
dotnet sln add UniMateAI.Api/UniMateAI.Api.csproj

# 5. Create the static frontend folder (plain files, no dotnet command needed)
mkdir UniMateAI.Web
```

At this point your folder looks like this:

```
UniMateAI/
├── UniMateAI.sln
├── UniMateAI.Api/        ← C# backend (created by dotnet new webapi)
└── UniMateAI.Web/        ← HTML/CSS/JS frontend (files I gave you)
```

Copy the files I generated (`index.html`, `css/`, `js/`, `images/`)
into `UniMateAI.Web/`, replacing the empty folder.

We will only start editing `UniMateAI.Api` from **Step 5** (database)
onward. For Step 1, the API project just needs to exist and run — you
don't need to understand its internals yet.

## E. Folder structure — what each folder is for

```
UniMateAI/
│
├── UniMateAI.sln                 ← ties both projects together
│
├── UniMateAI.Api/                ← C# BACKEND (built from Step 5+)
│   ├── Controllers/              ← handles HTTP requests, e.g. GET /api/notices
│   ├── Services/                 ← business logic (GPA math, AI calls)
│   ├── Models/                   ← C# classes matching DB tables (Student, Course...)
│   ├── DTOs/                     ← "shape" of data sent to/from the frontend
│   ├── Data/                     ← EF Core DbContext — the DB connection
│   ├── Migrations/                ← auto-generated DB change history
│   ├── Program.cs                ← the app's startup file
│   ├── appsettings.json          ← non-secret settings
│   └── appsettings.Development.json
│
└── UniMateAI.Web/                ← FRONTEND (built today, Step 1)
    ├── index.html                ← public landing page (done today)
    ├── register.html             ← Step 2
    ├── login.html                ← Step 3
    ├── dashboard.html            ← Step 4
    ├── css/
    │   ├── style.css             ← colors, type, header, hero, sections
    │   ├── components.css        ← buttons, badges (reusable pieces)
    │   └── responsive.css        ← mobile/tablet/desktop breakpoints
    ├── js/
    │   └── main.js               ← mobile menu today; auth.js/dashboard.js etc. later
    └── images/
        └── university-logo.png   ← you add this file
```

## F. How the frontend and backend will talk (from Step 2 onward)

The browser calls the API with `fetch()` and gets JSON back:

```javascript
const response = await fetch("https://localhost:5001/api/notices");
const notices = await response.json();
```

Today (Step 1) `index.html` does not call the API at all — it is a
static page. Real `fetch()` calls start in Step 2 (registration).

## G. Database plan

We start with **SQLite** because it needs zero installation — it's
just a file (`unimate.db`) sitting in your project folder. Entity
Framework Core is the layer that lets your C# code talk to *any*
relational database using the same C# code. So later, moving to
**SQL Server** is a one-line change in `Program.cs` (swap
`UseSqlite(...)` for `UseSqlServer(...)`) — your Controllers, Services
and Models don't need to change. We'll do this properly in Step 5.

## H. AI integration plan

From Step 10 onward:

1. `AIController` receives a chat/roadmap request from the browser.
2. `AIService` (C#) builds a prompt using the student's real data
   (courses, attendance, grades — pulled from the database).
3. `AIService` calls the LLM provider's API (OpenAI/Gemini/Groq).
4. The API key is read from **User Secrets** (local dev) or an
   **environment variable** (deployment) — never from a file that
   gets committed to Git, and never from JavaScript.

## I. Where the university logo goes

Copy your real logo file to:

```
UniMateAI.Web/images/university-logo.png
```

`index.html` already references it in the header and footer, sized
responsively (40×40px display size). If the file is missing, the
`onerror` attribute on the `<img>` tag just hides the broken-image
icon so your layout doesn't break — nothing crashes.

## J–L. What was built today

- `index.html` — public landing page: header + nav, hero with the
  UniMate AI tagline and three CTA buttons (Explore University /
  Student Login / Register), a small stats strip, a "three portals"
  section (University / Student / AI), a sample Academics grid,
  sample Notices and Events (clearly labelled "Demo data"), an
  admissions call-to-action band, and a footer with contact info and
  a disclaimer that this is not an official university system.
- `css/style.css` — all brand colors as CSS variables (`--navy`,
  `--blue`, `--red`, etc.), typography (Fraunces for headings, Inter
  for body/UI), and the layout for every section.
- `css/components.css` — buttons and badges, reused across the whole
  site so future pages (register/login/dashboard) stay visually
  consistent.
- `css/responsive.css` — breakpoints at 1024px (tablet, nav collapses
  into a hamburger menu), 768px (stacks grids to one/two columns),
  480px (phone spacing, full-width buttons), and 1600px (roomier
  desktop layout).
- `js/main.js` — opens/closes the mobile menu.

## M. How to run it

**Frontend only (what you need for Step 1):**

You don't need the API running yet — `index.html` is a static file.

*Option 1 — VS Code Live Server (recommended):*
1. Open the `UniMateAI.Web` folder in VS Code.
2. Install the "Live Server" extension (one-time).
3. Right-click `index.html` → **Open with Live Server**.
4. Your browser opens automatically at something like
   `http://127.0.0.1:5500/index.html`.

*Option 2 — just double-click:*
Double-click `index.html` in your file explorer. It will open in your
default browser directly from disk. This works fine for Step 1 since
there's no `fetch()` call yet.

**Backend (just to confirm it was created correctly):**

```bash
cd UniMateAI.Api
dotnet restore
dotnet build
dotnet run
```

Expected output ends with something like:
```
Now listening on: https://localhost:5001
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

Open `https://localhost:5001/swagger` in your browser — you should
see the default Swagger API test page ASP.NET Core generates for you.
That confirms your backend project is wired up correctly. We will not
build real endpoints in it until Step 2/Step 5.

## N. Testing checklist for Step 1

- [ ] `dotnet --version` prints a version starting with `8.`
- [ ] `dotnet run` inside `UniMateAI.Api` starts without errors and
      Swagger loads at `/swagger`
- [ ] `index.html` opens in the browser and the hero section is
      visible with the UniMate AI tagline
- [ ] Resize the browser (or use DevTools device toolbar) to check:
  - [ ] 320px / 375px — nav collapses to a hamburger, buttons stack
        full-width, no horizontal scrollbar
  - [ ] 768px — portal cards and department cards go to fewer columns
  - [ ] 1024px — nav still hamburger (tablet)
  - [ ] 1366px / 1920px — full desktop nav bar visible, layout has
        breathing room, doesn't stretch edge-to-edge oddly
- [ ] Click the hamburger menu on mobile width — it opens and closes,
      and tapping a link closes it again
- [ ] Tab through the page with keyboard only — you can see a visible
      focus outline on links and buttons
- [ ] All three CTA buttons in the hero are visible and clickable
      (they'll 404 to `login.html`/`register.html` until Step 2/3 —
      that's expected today)

## O. Concepts you learned in Step 1

- **Solution vs. project**: a `.sln` file is just a folder of related
  `.csproj` projects — it doesn't run anything itself.
- **Static frontend vs. Web API backend**: two separate things that
  only communicate over HTTP, never by directly sharing code.
- **Why SQLite first**: zero setup, single file, same EF Core code
  ports to SQL Server later.
- **CSS custom properties (`--navy`, `--blue`...)**: define a color
  once, reuse everywhere — change the brand palette in one place.
- **Mobile-first responsive breakpoints**: build the small-screen
  layout as the default, then add rules for larger screens.
- **Why API keys never go in JavaScript**: anything in browser code
  is visible to anyone who opens DevTools — secrets must live only on
  the server.

---
Next: say **"STEP 2"** to build student registration (form, validation,
and the first real `fetch()` call to the backend).

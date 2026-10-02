# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

Project overview, commands, structure and toolchain are in the README; the rules every contributor follows (branches, commits, PRs, CI, formatting, Definition of Done, when to write an ADR) are in CONTRIBUTING. Both are imported here so they are always in context; do not restate them in this file.

@README.md
@CONTRIBUTING.md

The rest of this file is only what agents need on top of those two.

## Current state

The app is still close to the `dotnet new mvc` skeleton. The only feature is the About page (`HomeController.About()`, `Views/Home/About.cshtml`, a "Giới thiệu" menu item), with unit tests in `tests/SimpleBlog.Tests/Unit/` and acceptance tests in `tests/SimpleBlog.Tests/Acceptance/About/`. There is no DbContext, entity, migration or service layer yet, so there are no established patterns beyond stock ASP.NET Core MVC.

Known workaround: `Program` is internal, so the acceptance tests build `WebApplicationFactory` through reflection. Do not copy that pattern; the fix is `public partial class Program {}` in the Web project.

There are no project-level decisions yet (vision, non-functional requirements, UI guidelines, architecture, backlog). When a task needs one of those decisions, ask instead of assuming.

## Agent permissions

`.claude/settings.json` allows agents to push only `feature/*` and `chore/*` branches with `git push -u origin <branch>`, and to use `gh pr create`, `gh pr view` and `gh pr checks`. It denies force push, deleting remote branches, pushing `main`, and `gh pr merge`: a human merges. Branch protection on GitHub enforces the same for everyone (ADR-0002, ADR-0004).

Changes to `.claude/` (agents, skills, permissions) go through a PR and are approved by a human edit by edit. Never work around a permission denial.

## Skills

- `/feature <name> <description>` runs BA → Architect → Developer → Reviewer → Tester, stops for the user to approve `01-requirements.md`, then pushes `feature/<name>` and opens a PR.
- `/sync [branch]` runs after the user squash-merges a PR: checks the PR is `MERGED`, switches to `main`, `git pull --ff-only`, deletes the local branch.

## Database

- Only run `ef` or SQL commands against `(localdb)\MSSQLLocalDB`.
- Acceptance tests that need a database use their own database `SimpleBlog_Test`, never `SimpleBlog`.

## Vietnamese text in Razor

Razor HTML-encodes non-ASCII characters printed through `@` (for example `ệ` becomes `&#x1EC7;`). Write fixed Vietnamese strings literally in the `.cshtml` file, not through `@ViewData`, `@Model` or a C# variable, so tests that match raw HTML see the real text. `.cshtml` files are UTF-8 with BOM (ADR-0005).

## Lessons learned

Before finishing any task that ends in a PR, check whether it produced a lesson (an unexpected failure, a workaround, a recommendation the user had to correct, a missing process step). If so, append an entry to `docs/lessons-learned.md` in the same PR, in the format the file uses. Do not wait to be asked.

## Environment

- If `gh` is not found, GitHub CLI is installed at `C:\Program Files\GitHub CLI\gh.exe` but the editor has not been restarted since; tell the user rather than hard-coding the path into scripts.
- If `dotnet build` fails because `SimpleBlog.Web.exe` is locked, the user is running the app. Use `-c Release`; never kill the user's process.

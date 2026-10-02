#!/usr/bin/env bash
# Foundation gate (ADR-0007, docs/design/project-foundation.md section 6.2).
#
# A pull request that adds or changes anything under docs/features/ (other than
# docs/features/_template/) passes only if the five foundation documents in
# docs/project/ exist and each has "status: approved" in its front matter.
#
# Usage: BASE_REF=origin/main scripts/check-foundation.sh
# With BASE_REF empty (push to main, manual run without a base) the gate passes.
set -euo pipefail

base="${BASE_REF:-}"
if [ -z "$base" ]; then
  echo "foundation-gate: no base ref (not a pull request), skipping."
  exit 0
fi

changed="$(git diff --name-only "$base"...HEAD -- docs/features | grep -v '^docs/features/_template/' || true)"
if [ -z "$changed" ]; then
  echo "foundation-gate: no changes under docs/features/, passing."
  exit 0
fi

echo "foundation-gate: this PR changes feature documents:"
echo "$changed" | sed 's/^/  /'

problems=()
for name in vision nfr ui-guidelines architecture backlog; do
  file="docs/project/$name.md"
  if [ ! -f "$file" ]; then
    problems+=("$file: missing")
    continue
  fi
  # Front matter = lines between the first line "---" and the next "---".
  status="$(tr -d '\r' < "$file" | awk 'NR==1 { if ($0 != "---") exit; next } $0 == "---" { exit } { print }' \
    | sed -n 's/^status:[[:space:]]*\([a-z]*\)[[:space:]]*$/\1/p' | head -n 1)"
  if [ "$status" != "approved" ]; then
    problems+=("$file: status is '${status:-<none>}', expected 'approved'")
  fi
done

if [ "${#problems[@]}" -gt 0 ]; then
  echo
  echo "foundation-gate: FAILED. Feature work needs the approved project foundation first:"
  printf '  %s\n' "${problems[@]}"
  echo
  echo "Run /init-project in Claude Code (or /init-project --from <step> to continue), get each file approved, and merge its PR."
  exit 1
fi

echo "foundation-gate: all five foundation documents are approved, passing."

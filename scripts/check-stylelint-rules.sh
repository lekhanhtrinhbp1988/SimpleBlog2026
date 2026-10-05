#!/usr/bin/env bash
# ADR-0015: proves the Stylelint rules still catch violations (AC-29).
# tests/stylelint/violations.css breaks the colour and font-size rules on purpose.
set -euo pipefail

sample="tests/stylelint/violations.css"

set +e
output="$(npx stylelint "$sample" --formatter json 2>&1)"
code=$?
set -e

if [ "$code" -eq 0 ]; then
  echo "FAIL: Stylelint reported no problem for $sample" >&2
  exit 1
fi

for rule in color-no-hex declaration-property-unit-disallowed-list; do
  if ! printf '%s' "$output" | grep -q "\"rule\":\"$rule\""; then
    echo "FAIL: Stylelint did not report rule $rule for $sample" >&2
    exit 1
  fi
done

echo "OK: Stylelint rejects hex colours and px font sizes."

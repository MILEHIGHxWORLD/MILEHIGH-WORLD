import json
import logging
import argparse
from datetime import datetime, timezone
from pathlib import Path

class EvidenceCollector:
    """Automated gathering of SOC 2 artifacts for specific audit windows."""

    def __init__(self, output_dir: str, audit_start: str, audit_end: str):
        self.output_dir = Path(output_dir)
        self.output_dir.mkdir(parents=True, exist_ok=True)
        try:
            self.audit_start = datetime.fromisoformat(audit_start).replace(tzinfo=timezone.utc)
            self.audit_end = datetime.fromisoformat(audit_end).replace(tzinfo=timezone.utc)
        except ValueError as e:
            logging.error(f"Invalid date format: {e}. Use ISO 8601 (YYYY-MM-DD).")
            raise
        self.manifest = []

    def collect_access_review(self, iam_export_path: str):
        """Maps to CC6.1-02: Regular review of user access rights."""
        source = Path(iam_export_path)
        if source.exists():
            dest = self.output_dir / "CC6.1-02_access_review.json"
            dest.write_text(source.read_text())
            self.manifest.append({
                "control_id": "CC6.1-02",
                "collected_at": datetime.now(timezone.utc).isoformat(),
                "status": "collected",
                "artifact": str(dest)
            })
            logging.info(f"Collected access review: {dest}")
        else:
            logging.warning(f"IAM export not found at {iam_export_path}")
        return self.manifest[-1] if self.manifest else None

    def collect_change_logs(self, changelog_dir: str):
        """Maps to CC8.1-01: Change management evidence within period."""
        changes = []
        changelog_path = Path(changelog_dir)
        if not changelog_path.exists():
            logging.warning(f"Changelog directory not found: {changelog_dir}")
            return 0

        for f in changelog_path.glob("*.json"):
            try:
                data = json.loads(f.read_text())
                # Handle both list of entries or single entry
                entries = data if isinstance(data, list) else [data]

                # Filter changes that occurred within the audit period
                period_changes = [
                    entry for entry in entries
                    if self.audit_start <= datetime.fromisoformat(entry["date"]).replace(tzinfo=timezone.utc) <= self.audit_end
                ]
                changes.extend(period_changes)
            except (json.JSONDecodeError, KeyError, ValueError) as e:
                logging.error(f"Error processing {f}: {e}")

        out = self.output_dir / "CC8.1-01_changes.json"
        out.write_text(json.dumps(changes, indent=2))
        self.manifest.append({
            "control_id": "CC8.1-01",
            "collected_at": datetime.now(timezone.utc).isoformat(),
            "count": len(changes),
            "artifact": str(out)
        })
        logging.info(f"Collected {len(changes)} change logs to {out}")
        return len(changes)

    def generate_manifest(self):
        """Generates the master list for auditor review."""
        manifest_path = self.output_dir / "evidence_manifest.json"
        manifest_path.write_text(json.dumps(self.manifest, indent=2))
        logging.info(f"Generated manifest at {manifest_path}")

def main():
    parser = argparse.ArgumentParser(description="SOC 2 Evidence Collector")
    parser.add_argument("--output", default="evidence_output", help="Output directory for evidence")
    parser.add_argument("--start", required=True, help="Audit window start (YYYY-MM-DD)")
    parser.add_argument("--end", required=True, help="Audit window end (YYYY-MM-DD)")
    parser.add_argument("--iam-export", help="Path to IAM export JSON")
    parser.add_argument("--changelog-dir", help="Directory containing changelog JSONs")
    parser.add_argument("--mock", action="store_true", help="Generate mock data for testing")

    args = parser.parse_args()
    logging.basicConfig(level=logging.INFO, format="%(levelname)s: %(message)s")

    if args.mock:
        mock_dir = Path("mock_data")
        mock_dir.mkdir(exist_ok=True)

        iam_mock = mock_dir / "iam_export.json"
        iam_mock.write_text(json.dumps([{"user": "admin", "roles": ["Owner"]}], indent=2))

        changes_dir = mock_dir / "changes"
        changes_dir.mkdir(exist_ok=True)
        (changes_dir / "change1.json").write_text(json.dumps([
            {"id": "PR-101", "date": args.start, "author": "dev1", "description": "Update encryption"},
            {"id": "PR-102", "date": args.end, "author": "dev2", "description": "Fix login bug"}
        ], indent=2))

        args.iam_export = str(iam_mock)
        args.changelog_dir = str(changes_dir)

    collector = EvidenceCollector(args.output, args.start, args.end)

    if args.iam_export:
        collector.collect_access_review(args.iam_export)

    if args.changelog_dir:
        collector.collect_change_logs(args.changelog_dir)

    collector.generate_manifest()

if __name__ == "__main__":
    main()

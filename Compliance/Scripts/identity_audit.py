import requests
import argparse
import logging
import json
from datetime import datetime, timezone, timedelta

def audit_okta_users(org_url, api_token, dry_run=True):
    """
    Performs programmatic access audits, identifying inactive users or those without MFA enrollment.
    Note: In a real environment, this would hit the Okta API.
    For this implementation, we handle both mock data and potential real API calls.
    """
    headers = {"Authorization": f"SSWS {api_token}"}
    findings = []

    try:
        # Mocking the API response if the URL is "mock://okta"
        if org_url == "mock://okta":
            users = [
                {
                    "profile": {"login": "active_user@example.com"},
                    "status": "ACTIVE",
                    "factors": ["MFA_ENROLLED"],
                    "lastLogin": datetime.now(timezone.utc).isoformat()
                },
                {
                    "profile": {"login": "no_mfa@example.com"},
                    "status": "ACTIVE",
                    "factors": [],
                    "lastLogin": datetime.now(timezone.utc).isoformat()
                },
                {
                    "profile": {"login": "inactive@example.com"},
                    "status": "ACTIVE",
                    "factors": ["MFA_ENROLLED"],
                    "lastLogin": (datetime.now(timezone.utc) - timedelta(days=91)).isoformat()
                },
                {
                    "profile": {"login": "terminated@example.com"},
                    "status": "DEPROVISIONED",
                    "factors": [],
                    "lastLogin": None
                }
            ]
        else:
            response = requests.get(f"{org_url}/api/v1/users", headers=headers, timeout=10)
            response.raise_for_status()
            users = response.json()

        now = datetime.now(timezone.utc)

        for user in users:
            login = user['profile']['login']
            status = user['status']

            # 🛡️ Sentinel: Including "Leaver" (DEPROVISIONED) users in the audit to verify
            # termination process integrity.
            if status == 'DEPROVISIONED':
                # For deprovisioned users, ensure no active factors remain (CC6.1-03)
                if user.get('factors'):
                    findings.append({"user": login, "issue": "DEPROVISIONED BUT RETAINS FACTORS", "severity": "CRITICAL"})
                continue

            if status != 'ACTIVE':
                continue

            # Check for MFA enrollment status (CC6.1-05)
            factors = user.get('factors', [])
            if 'MFA_ENROLLED' not in factors:
                findings.append({"user": login, "issue": "MFA NOT ENROLLED", "severity": "HIGH"})

            # Check for inactivity > 90 days (CC6.1-03)
            last_login_str = user.get('lastLogin')
            if last_login_str:
                last_login = datetime.fromisoformat(last_login_str.replace('Z', '+00:00'))
                if (now - last_login).days > 90:
                    findings.append({"user": login, "issue": "INACTIVE > 90 DAYS", "severity": "MEDIUM", "last_login": last_login_str})
            else:
                findings.append({"user": login, "issue": "NEVER LOGGED IN", "severity": "MEDIUM"})

    except Exception as e:
        logging.error(f"Failed to audit Okta users: {e}")
        return [{"issue": "AUDIT_FAILURE", "error": str(e)}]

    return findings

def main():
    parser = argparse.ArgumentParser(description="Okta Identity Governance Audit")
    parser.add_argument("--url", default="mock://okta", help="Okta Org URL")
    parser.add_argument("--token", default="MOCK_TOKEN", help="Okta API Token")
    parser.add_argument("--output", help="Save findings to JSON file")

    args = parser.parse_args()
    logging.basicConfig(level=logging.INFO, format="%(levelname)s: %(message)s")

    logging.info(f"Starting audit for {args.url}...")
    findings = audit_okta_users(args.url, args.token)

    if findings:
        logging.warning(f"Found {len(findings)} security issues:")
        for f in findings:
            print(f"  - [{f['severity']}] {f['user']}: {f['issue']}")
    else:
        logging.info("No security issues found.")

    if args.output:
        with open(args.output, 'w') as f:
            json.dump(findings, f, indent=2)
        logging.info(f"Findings saved to {args.output}")

if __name__ == "__main__":
    main()

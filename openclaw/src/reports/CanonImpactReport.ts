import { ValidationResult } from '../agent/CanonValidator.js';

export class CanonImpactReport {
  public static generate(results: ValidationResult[]): string {
    let report = "### 🔮 Canon Impact Assessment\n\n";

    if (results.length === 0) {
      report += "✅ No lore violations detected.\n";
      return report;
    }

    report += "| Rule ID | Severity | Message |\n| --- | --- | --- |\n";
    for (const res of results) {
      report += `| ${res.ruleId} | ${res.severity} | ${res.message} |\n`;
    }

    return report;
  }
}

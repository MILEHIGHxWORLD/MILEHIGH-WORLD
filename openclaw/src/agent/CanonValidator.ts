import * as fs from 'fs/promises';
import * as path from 'path';

export type RuntimeField =
  | 'alive'
  | 'corrupted'
  | 'omenLevel'
  | 'stability'
  | 'action';

export interface RuntimeEntityState {
  entityId: string;
  alive: boolean;
  corrupted: boolean;
  omenLevel: number;
  stability?: number;
  action?: string;
  lastUpdated: string;
}

export interface LoreAssertion {
  field: RuntimeField;
  operator: 'EQUALS' | 'NOT_EQUALS' | 'GREATER_THAN' | 'LESS_THAN';
  value: any;
}

export interface LoreValidationRule {
  ruleId: string;
  targetEntityId: string;
  severity: 'INFO' | 'WARNING' | 'ERROR' | 'CRITICAL';
  assertion: LoreAssertion;
}

export interface ValidationResult {
  ruleId: string;
  severity: 'INFO' | 'WARNING' | 'ERROR' | 'CRITICAL';
  message: string;
}

export class CanonValidator {
  public static evaluateRule(rule: LoreValidationRule, state: RuntimeEntityState): boolean {
    const actualValue = (state as any)[rule.assertion.field];

    switch (rule.assertion.operator) {
      case 'EQUALS':
        return actualValue === rule.assertion.value;
      case 'NOT_EQUALS':
        return actualValue !== rule.assertion.value;
      case 'GREATER_THAN':
        return actualValue > rule.assertion.value;
      case 'LESS_THAN':
        return actualValue < rule.assertion.value;
      default:
        return true;
    }
  }

  public static async runReport(affectedEntities: RuntimeEntityState[]): Promise<ValidationResult[]> {
    const rulesPath = path.resolve(process.cwd(), 'src/knowledge/codex/ValidationRules.json');
    const rules: LoreValidationRule[] = JSON.parse(await fs.readFile(rulesPath, 'utf-8'));

    const results: ValidationResult[] = [];

    for (const entity of affectedEntities) {
      const entityRules = rules.filter(r => r.targetEntityId === entity.entityId);
      for (const rule of entityRules) {
        const passed = this.evaluateRule(rule, entity);
        if (!passed) {
          results.push({
            ruleId: rule.ruleId,
            severity: rule.severity,
            message: `Entity ${entity.entityId} failed ${rule.assertion.field} ${rule.assertion.operator} ${rule.assertion.value}. Actual: ${ (entity as any)[rule.assertion.field] }`
          });
        }
      }
    }

    return results;
  }
}

import * as fs from 'fs/promises';
import * as path from 'path';
import { RuntimeEntityState } from '../../agent/CanonValidator.js';

export class RuntimeStateStore {
  private statePath: string;

  constructor() {
    this.statePath = path.resolve(process.cwd(), 'src/knowledge/runtime/RuntimeState.json');
  }

  public async getEntityState(entityId: string): Promise<RuntimeEntityState | undefined> {
    const data = JSON.parse(await fs.readFile(this.statePath, 'utf-8'));
    return data.states.find((s: RuntimeEntityState) => s.entityId === entityId);
  }

  public async updateEntityState(state: RuntimeEntityState): Promise<void> {
    const data = JSON.parse(await fs.readFile(this.statePath, 'utf-8'));
    const index = data.states.findIndex((s: RuntimeEntityState) => s.entityId === state.entityId);

    if (index !== -1) {
      data.states[index] = state;
    } else {
      data.states.push(state);
    }

    await fs.writeFile(this.statePath, JSON.stringify(data, null, 2));
  }
}

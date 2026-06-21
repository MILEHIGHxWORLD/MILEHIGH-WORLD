import * as fs from 'fs/promises';
import * as path from 'path';
import { RuntimeEntityState } from './CanonValidator.js';

export class MemoryManager {
  private knowledgePath: string;

  constructor() {
    this.knowledgePath = path.resolve(process.cwd(), 'src/knowledge');
  }

  private async getAllJsonFiles(dir: string): Promise<string[]> {
    const entries = await fs.readdir(dir, { withFileTypes: true });
    const files = await Promise.all(entries.map((res) => {
      const resolve = path.resolve(dir, res.name);
      return res.isDirectory() ? this.getAllJsonFiles(resolve) : resolve;
    }));
    return Array.prototype.concat(...files).filter(f => f.endsWith('.json'));
  }

  public async retrieveRelevantContext(taskPrompt: string): Promise<string> {
    try {
      const files = await this.getAllJsonFiles(this.knowledgePath);
      let context = "";

      for (const file of files) {
        const content = await fs.readFile(file, 'utf-8');
        context += `\n--- Context from ${path.basename(file)} ---\n${content}\n`;
      }

      return context;
    } catch (error) {
      console.error("Error retrieving context:", error);
      return "Context unavailable.";
    }
  }

  public async mapFilesToEntities(files: any[]): Promise<RuntimeEntityState[]> {
    const mappingsPath = path.resolve(this.knowledgePath, 'codex/CodeMappings.json');
    const mappings = JSON.parse(await fs.readFile(mappingsPath, 'utf-8'));

    const affectedEntityIds = new Set<string>();

    for (const file of files) {
      const filename = file.filename;
      for (const mapping of mappings) {
        if (mapping.github.paths.some((p: string) => filename.startsWith(p))) {
          affectedEntityIds.add(mapping.entityId);
        }
      }
    }

    const runtimeStorePath = path.resolve(this.knowledgePath, 'runtime/RuntimeState.json');
    const runtimeData = JSON.parse(await fs.readFile(runtimeStorePath, 'utf-8'));

    return runtimeData.states.filter((s: RuntimeEntityState) => affectedEntityIds.has(s.entityId));
  }
}

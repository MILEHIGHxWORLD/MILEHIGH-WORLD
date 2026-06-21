import { Octokit } from "@octokit/rest";
import { CanonValidator } from "../agent/CanonValidator.js";
import { MemoryManager } from "../agent/MemoryManager.js";
import { CanonImpactReport } from "../reports/CanonImpactReport.js";

export class GitHubTool {
    private octokit: Octokit;
    private memory: MemoryManager;

    constructor(token: string) {
        this.octokit = new Octokit({ auth: token });
        this.memory = new MemoryManager();
    }

    /**
     * Executes the lore-validation loop on a Pull Request.
     */
    public async analyzePullRequest(owner: string, repo: string, prNumber: number): Promise<void> {
        console.log(`Analyzing PR #${prNumber} for lore consistency...`);

        // 1. Fetch changed files
        const { data: files } = await this.octokit.pulls.listFiles({ owner, repo, pull_number: prNumber });

        // 2. Map files to Enneaverse Entities
        const affectedEntities = await this.memory.mapFilesToEntities(files);

        // 3. Run Lore Validation
        const validationResults = await CanonValidator.runReport(affectedEntities);
        const validationReport = CanonImpactReport.generate(validationResults);

        // 4. Post feedback to PR
        await this.octokit.issues.createComment({
            owner,
            repo,
            issue_number: prNumber,
            body: validationReport
        });

        // 5. Apply Labels
        if (validationReport.includes("CRITICAL") || validationReport.includes("ERROR")) {
            await this.octokit.issues.addLabels({
                owner, repo, issue_number: prNumber, labels: ['needs-architectural-review', 'canon-breaking-change']
            });
        }
    }
}

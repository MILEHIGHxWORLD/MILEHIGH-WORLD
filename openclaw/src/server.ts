import * as dotenv from 'dotenv';
import { OpenClawAgent } from './agent/OpenClawAgent.js';

dotenv.config();

async function main() {
  const token = process.env.GITHUB_TOKEN;
  if (!token) {
    console.error("GITHUB_TOKEN not found in environment variables.");
    process.exit(1);
  }

  const agent = new OpenClawAgent(token);
  const task = process.argv[2] || "Initialize repository audit";

  await agent.orchestrate(task);
}

main().catch(err => {
  console.error("Agent execution failed:", err);
  process.exit(1);
});

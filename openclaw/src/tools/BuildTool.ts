import { exec } from 'child_process';
import { promisify } from 'util';

const execAsync = promisify(exec);

export class BuildTool {
  public async validateProject(): Promise<void> {
    console.log("Running project validation...");
    try {
      const { stdout } = await execAsync('python3 ../validate_implementation.py');
      console.log(stdout);
    } catch (error) {
      console.error("Validation failed:", error);
      throw error;
    }
  }

  public async buildCSharp(): Promise<void> {
    console.log("Building C# scripts...");
    try {
      const { stdout } = await execAsync('dotnet build ../Assets/Scripts/VerifyAll.csproj');
      console.log(stdout);
    } catch (error) {
      console.error("Build failed:", error);
      throw error;
    }
  }
}

using LabsLibrary;
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.InteropServices;

[Command(Name = "labs", Description = "Labs runner"),
Subcommand(typeof(Run), typeof(Version), typeof(SetPath))]
class Lab4
{
	private const string LAB_PATH = "LAB_PATH";
	public static void Main(string[] args) => CommandLineApplication.Execute<Lab4>(args);
	private int OnExecute(CommandLineApplication commandLineApp, IConsole activeConsole)
	{
		activeConsole.WriteLine("You must specify at a command.");

		activeConsole.WriteLine($"Platform = " + Environment.OSVersion.Platform);
		commandLineApp.ShowHelp();
		return 1;
	}

	[Command("version", Description = "Project Info")]
	private class Version
	{
		private int OnExecute(IConsole console)
		{
			Console.WriteLine("Created by Illia Kosiak IPZ-43");
			var actualVersion = Assembly.GetExecutingAssembly().GetName().Version;
			console.WriteLine($"Version: {actualVersion}");

			var labPathEnv = SetPath.GetLabPathEnv();

			if (string.IsNullOrWhiteSpace(labPathEnv))
			{
				console.WriteLine("Variable LAB_PATH: not set");
			}
			else
			{
				console.WriteLine("Variable LAB_PATH: not set: " + labPathEnv);
			}

			console.WriteLine($"Variable LAB_PATH: {(string.IsNullOrWhiteSpace(labPathEnv) ? "not set" : labPathEnv)}");
			return 1;
		}
	}

	[Command("run", Description = "Run app"), Subcommand(typeof(Lab1Presenter)), Subcommand(typeof(Lab2Presenter)), Subcommand(typeof(Lab3Presenter))]
	private class Run
	{
		private int OnExecute(IConsole console)
		{
			console.Error.WriteLine("You must choose a lab");
			return 1;
		}

		private abstract class LabsLibrary
		{
			[Option("--input -i", Description = "Select input")]
			public string Input { get; } = null!;

			[Option("--output -o", Description = "Select output")]
			public string Output { get; } = null!;

			protected string GetInputPath()
			{
				if (File.Exists(Input))
				{
					return Input;
				}

				var labPathEnv = SetPath.GetLabPathEnv();

				if (string.IsNullOrWhiteSpace(labPathEnv))
				{
					labPathEnv = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				}
				labPathEnv = Path.Combine(labPathEnv, "INPUT.TXT");

				if (File.Exists(labPathEnv))
				{
					return labPathEnv;
				}

				return string.Empty;
			}
			protected string GetOutputPath()
			{
				if (!string.IsNullOrWhiteSpace(Output))
				{
					return Output;
				}

				var labPathEnv = SetPath.GetLabPathEnv();

				if (string.IsNullOrWhiteSpace(labPathEnv))
				{
					labPathEnv = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				}

				if (Directory.Exists(labPathEnv))
				{
					return Path.Combine(labPathEnv, "OUTPUT.TXT");
				}

				return "OUTPUT.TXT";
			}

			protected string ReadInputFile()
			{
				string inputPath = GetInputPath();

				if (string.IsNullOrWhiteSpace(inputPath))
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine($"Error: INPUT.TXT not found.");
					Console.ForegroundColor = ConsoleColor.White;
					return string.Empty;
				}

				Console.WriteLine($"Read input file: {inputPath}");

				return File.ReadAllText(inputPath);
			}
			protected void WriteOuputFile(string outputData)
			{
				string outputPath = GetOutputPath();

				Console.WriteLine($"Write output file: {outputPath}");

				File.WriteAllText(outputPath, outputData);
			}

			protected virtual int OnExecute(IConsole console)
			{
				console.WriteLine("Input: " + Input + "Output: " + Output);
				return 1;
			}
		}

		[Command("lab1", Description = "Execute lab1")]
		private class Lab1Presenter : LabsLibrary
		{
			protected override int OnExecute(IConsole console)
			{
				string inputData = ReadInputFile();
				if (string.IsNullOrWhiteSpace(inputData))
				{
					return -1;
				}
				console.WriteLine($"Executing lab1");

				var result = Lab1.RunApp(Input);


				console.WriteLine($"Result: " + result);

				WriteOuputFile(result.ToString());
				return 1;
			}
		}

		[Command("lab2", Description = "Execute lab2")]
		private class Lab2Presenter : LabsLibrary
		{
			protected override int OnExecute(IConsole console)
			{
				string inputData = ReadInputFile();
				if (string.IsNullOrWhiteSpace(inputData))
				{
					return -1;
				}

				console.WriteLine($"Executing lab2");

				var result = Lab2.RunApp(Input);
				console.WriteLine($"Result: " + result);

				WriteOuputFile(result.ToString());
				return 1;
			}
		}

		[Command("lab3", Description = "Execute lab3")]
		private class Lab3Presenter : LabsLibrary
		{
			protected override int OnExecute(IConsole console)
			{
				string inputData = ReadInputFile();
				if (string.IsNullOrWhiteSpace(inputData))
				{
					return -1;
				}

				console.WriteLine($"Executing lab3");
				var result = Lab3.RunApp(Input);
				console.WriteLine($"Result: " + result);

				WriteOuputFile(result.ToString());
				return 1;
			}
		}
	}

	[Command("set-path", Description = "Set LAB_PATH")]
	private class SetPath
	{
		[Option("--path -p", Description = "path to input/output")]
		[Required(ErrorMessage = "You must specify the path")]
		public string Path { get; } = null!;
		private int OnExecute(IConsole console)
		{
			Environment.SetEnvironmentVariable(LAB_PATH, Path);

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				File.WriteAllText(".env", $"{LAB_PATH}={Path}");
			}

			return 1;
		}
		public static string GetLabPathEnv()
		{
			var path = Environment.GetEnvironmentVariable(LAB_PATH);
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				var text = File.ReadAllText(".env");
				var keyValue = text.Split('=');
				if (keyValue.Length == 2)
					path = keyValue[1];
			}
			return path ?? "";
		}
	}
}
namespace Lab1_Kosiak
{
	public class Program
	{
		public static string InputFilePath = @"..\..\input.txt";
		public static string OutputFilePath = @"..\..\output.txt";

		static void Main(string[] args)
		{
			var zooManager = new ZooManager();
			var amountOfAnimals = zooManager.CountAnimalsForExhibition();
			FileInfo outputFileInfo = new FileInfo(OutputFilePath);

			using (StreamWriter streamWriter = outputFileInfo.CreateText())
			{
				streamWriter.WriteLine(amountOfAnimals);
			}
		}

		public class ZooManager
		{
			public long CountAnimalsForExhibition()
			{
				var amountOfAnimalKinds = int.Parse(File.ReadLines(InputFilePath).First());
				var amountOfEveryKind = File.ReadLines(InputFilePath).Skip(1).First().Split(' ').Select(am => Convert.ToInt32(am)).ToList();

				long result = 0;
				for (int i = 0; i < amountOfEveryKind.Count - 2; i++)
				{
					for (int j = i + 1 ; j < amountOfEveryKind.Count - 1; j++)
					{
						for (int k = j + 1 ; k < amountOfEveryKind.Count; k++)
						{
							result += amountOfEveryKind[i] * amountOfEveryKind[j] * amountOfEveryKind[k];

                        }
					}
				}

				return result;
			}
		}
	}
}
namespace LabsLibrary
{
	public static class Lab1
	{
		public static long RunApp(string inputTextFile = "INPUT.TXT")
		{
			var zooManager = new ZooManager();
			var amountOfAnimals = zooManager.CountAnimalsForExhibition(inputTextFile);
			return amountOfAnimals;
		}

		private class ZooManager
		{
			public long CountAnimalsForExhibition(string inputTextFile = "INPUT.TXT")
			{
				var amountOfAnimalKinds = int.Parse(File.ReadLines(inputTextFile).First());
				var amountOfEveryKind = File.ReadLines(inputTextFile).Skip(1).First().Split(' ').Select(am => Convert.ToInt32(am)).ToList();

				long result = 0;
				for (int i = 0; i < amountOfEveryKind.Count - 2; i++)
				{
					for (int j = i + 1; j < amountOfEveryKind.Count - 1; j++)
					{
						for (int k = j + 1; k < amountOfEveryKind.Count; k++)
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
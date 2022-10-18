namespace Lab2_Kosiak
{
	public class Program
	{
		public static string InputFilePath = @"..\..\input.txt";
		public static string OutputFilePath = @"..\..\output.txt";

		static void Main(string[] args)
		{
			FileInfo outputFileInfo = new FileInfo(OutputFilePath);
			var inputData = File.ReadLines(InputFilePath).ToList();
			using (StreamWriter streamWriter = outputFileInfo.CreateText())
			{
				if (!inputData.Any() || Convert.ToInt32(inputData[0].Trim()) > 1000 || Convert.ToInt32(inputData[0].Trim()) < 1)
				{
					streamWriter.WriteLine("Number is out of range");
				}
				else
				{
					var numberList = inputData[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n)).ToList();

					if (numberList.Any(n => n > 10000 || n < -10000))
					{
						streamWriter.WriteLine("Number out of range");
					}
					else
					{
						streamWriter.WriteLine(GetMaxLengthOfIncreasingSubsezuence(numberList));
					}
				}
			}
		}

		/// <summary>
		/// Отримати довжину найбільшої зростаючої підмножини у послідовності
		/// </summary>
		/// <param name="numberList">Послідовність чисел</param>
		/// <returns></returns>
		private static int GetMaxLengthOfIncreasingSubsezuence(List<int> numberList)
		{
			int maxLength = 1;
			int thisLength = 1;
			for (int i = 1; i < numberList.Count; i++)
			{
				if (numberList[i] >= numberList[i - 1])
				{
					thisLength++;
				}
				else
				{
					maxLength = maxLength < thisLength ? thisLength : maxLength;
					thisLength = 1;
				}
			}
			return maxLength;
		}
	}
}

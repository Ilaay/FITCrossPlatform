namespace LabsLibrary
{
	public static class Lab2
	{
		public static int RunApp(string inputTextFile = "INPUT.TXT")
		{
			var inputData = File.ReadLines(inputTextFile).ToList();


			if (!inputData.Any() || Convert.ToInt32(inputData[0].Trim()) > 1000 || Convert.ToInt32(inputData[0].Trim()) < 1)
			{
				//streamWriter.WriteLine("Number is out of range");
				return -1;
			}
			else
			{
				var numberList = inputData[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n)).ToList();
				return GetMaxLengthOfIncreasingSubsequence(numberList);
			}
		}

		/// <summary>
		/// Отримати довжину найбільшої зростаючої підмножини у послідовності
		/// </summary>
		/// <param name="numberList">Послідовність чисел</param>
		/// <returns></returns>
		private static int GetMaxLengthOfIncreasingSubsequence(List<int> numberList)
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
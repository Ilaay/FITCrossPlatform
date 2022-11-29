namespace lab3_10
{
	public class Program
	{
		private const string InputFilePath = @"..\..\input.txt";
		private const string OutputFilePath = @"..\..\output.txt";

		static void Main(string[] args)
		{
			var listOfUnsuitableTableNeighbors = new Dictionary<int, List<int>>();
			var firstLineOfInputFile = File.ReadLines(InputFilePath).First();
			var indexOfSpace = firstLineOfInputFile.IndexOf(' ');
			var amountOfVIPs = Convert.ToInt32(firstLineOfInputFile.Substring(0, indexOfSpace));

			using (var sr = new StreamReader(InputFilePath))
			{
				for (int i = 1; i <= 100; i++)
				{
					var line = File.ReadLines(InputFilePath).Skip(i).FirstOrDefault();
					if (line is null) break;

					var spaceIndex = line.IndexOf(' ');
					var guestNumber = Convert.ToInt32(line.Substring(0, spaceIndex));
					var enemyNumber = Convert.ToInt32(line.Substring(spaceIndex));

					if (listOfUnsuitableTableNeighbors.ContainsKey(guestNumber))
					{
						var guestEnemiesList = listOfUnsuitableTableNeighbors[guestNumber];
						guestEnemiesList.Add(enemyNumber);
					}
					else
					{
						listOfUnsuitableTableNeighbors.Add(guestNumber, new List<int>() { enemyNumber });
					}
				}
			}

			//добавление поиском по ключах диктионари. Если нет такого ключа - создаём его и запихиваем в value врагов. Если есть - записываем в value врагов

			var organizer = new VIPsOrganizer(amountOfVIPs, listOfUnsuitableTableNeighbors);
			var itIsPossibleToOrganizeEvent = organizer.CheckIfSituationIsOrganizible();
			var outputFileInfo = new FileInfo(OutputFilePath);

			using (StreamWriter streamWriter = outputFileInfo.CreateText())
			{
				if (itIsPossibleToOrganizeEvent) streamWriter.WriteLine("TRUE");
				else streamWriter.WriteLine("FALSE");
			}
		}
	}

	public class VIPsOrganizer
	{

		private List<int> table1 = new List<int>(), table2 = new List<int>();
		private Dictionary<int, List<int>> listOfUnsuitableTableNeighbors;
		private int amountOfVIPs;

		public VIPsOrganizer(int AmountOfVIPs, Dictionary<int, List<int>> ListOfUnsuitableTableNeighbors)
		{
			amountOfVIPs = AmountOfVIPs;
			listOfUnsuitableTableNeighbors = ListOfUnsuitableTableNeighbors;
		}

		public bool CheckIfSituationIsOrganizible()
		{
			foreach (var guest in listOfUnsuitableTableNeighbors.Keys)
			{
				var guestIsAlreadyPlaced = CheckIfGuestAlreadyPlaced(guest);
				var guestEnemies = listOfUnsuitableTableNeighbors.GetValueOrDefault(guest);

				if (guestIsAlreadyPlaced) //значит он был в списке врагов одного из предыдущих гостей и сидит за 2ым столом. Проверяем могут ли его неприятели сидеть за 1ым столом
				{
					List<int> tableOfThisGuest = new List<int>();
					List<int> oppositeTable = new List<int>();

					if (table1.Contains(guest))
					{
						tableOfThisGuest = table1;
						oppositeTable = table2;
					}

					if (table2.Contains(guest))
					{
						tableOfThisGuest = table2;
						oppositeTable = table1;
					}

					var enemiesCanSeatTogether = CheckIfEnemiesCanSeatTogether(guest);
					if (!enemiesCanSeatTogether) return false;

					foreach (var enemy in guestEnemies)
					{
						var enemiesOfTheEnemyAlreadySeats = CheckIfEnemiesAlreadySeats(enemy, oppositeTable); //может ли враг сидеть с теми кто уже сидит за столом?
						if (enemiesOfTheEnemyAlreadySeats) return false;
					}

					if (!oppositeTable.Any(x => guestEnemies.Any(y => x == y))) oppositeTable.AddRange(guestEnemies);
				}
				else
				{
					var enemiesCanSeatTogether = CheckIfEnemiesCanSeatTogether(guest); //могут ли враги гостя сидеть вместе?
					if (!enemiesCanSeatTogether) return false;

					foreach (var enemy in guestEnemies)
					{
						var enemiesOfTheEnemyAlreadySeats = CheckIfEnemiesAlreadySeats(enemy, table2); //может ли враг сидеть с теми кто уже сидит за столом?
						if (enemiesOfTheEnemyAlreadySeats) return false;
					}

					if (!table1.Contains(guest)) table1.Add(guest);
					if (!table2.Any(x => guestEnemies.Any(y => x == y))) table2.AddRange(guestEnemies);
				}

				if (table1.Count + table2.Count == amountOfVIPs) return true;
			}

			return true;
		}

		private bool CheckIfGuestAlreadyPlaced(int numberOfGuest)
		{
			if (table1.Contains(numberOfGuest) || table2.Contains(numberOfGuest)) return true;
			return false;
		}

		private bool CheckIfEnemiesCanSeatTogether(int numberOfGuest)
		{
			var guestEnemies = listOfUnsuitableTableNeighbors.GetValueOrDefault(numberOfGuest);

			foreach (var enemy in guestEnemies) //проверяем каждого врага
			{
				if (!table1.Contains(enemy) || !table2.Contains(enemy))
				{
					var anotherGuests = new List<int>();
					anotherGuests.AddRange(guestEnemies);
					anotherGuests.Remove(enemy);

					var enemyCantSeatWithAnotherGuests = CheckIfGuestCantSeatWithThose(enemy, anotherGuests); //может ли враг сидеть с другими врагами гостя?
					if (enemyCantSeatWithAnotherGuests) return false;
				}
			}

			return true; //если все могут -> садим
		}

		private bool CheckIfGuestCantSeatWithThose(int numberOfGuest, List<int> thoseGuests)
		{
			var unsuitableGuests = listOfUnsuitableTableNeighbors.GetValueOrDefault(numberOfGuest);
			if (unsuitableGuests is null) return false;

			var cantSeat = unsuitableGuests.Any(x => thoseGuests.Any(y => y == x));

			return cantSeat;
		}

		private bool CheckIfEnemiesAlreadySeats(int guestNumber, List<int> table)
		{
			var enemies = listOfUnsuitableTableNeighbors.GetValueOrDefault(guestNumber);
			if (enemies is null) return false;

			if (table.Any(x => enemies.Any(y => x == y))) return true;

			return false;
		}
	}
}
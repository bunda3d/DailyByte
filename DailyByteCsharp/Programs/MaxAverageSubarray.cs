//https://thedailybyte.dev/
//Gven an array of integers, nums, and a value k, return the maximum average value from any contiguous subarray of size k in nums.

//Ex: Given the following nums and k…
//nums = [4, -1, 4, 2], k = 2, return 3.0 ((4 + 2) / 2 = 3.0).

//Ex: Given the following nums and k…
//nums = [5, 1, -3, 5, 2], k = 3, return 1.33

//Ex: Given the following nums and k…
//nums = [1, 2, 3, -1, 4, 2, 6, 0, 9, 1, 7, 1, 3, -2, 4, 6, 2, 0, -4, 1, 7, 9], k = 3, return 5.67
//to return the subarray(s) for the result:
//return ONLY the first subarray, even if ties exist:
//GetHighestMaxAverage() = [9, 1, 7]
//return the highest subarray (or subarrays if ties)
//GetHighestMaxAverages() = [ [9, 1, 7], [1, 7, 9] ]
//return the highest subarray (or subarrays if ties), with the last array members returning to the array's start to fill out its subarray size
//GetHighestMaxAveragesOuroborosArray() = [ [9, 1, 7], [1, 7, 9], [7, 9, 1] ]

public class MaxAverageSubarray
{
	public static void Test()
	{
		int[] array = { 1, 2, 3, -1, 4, 2, 6, 0, 9, 1, 7, 1, 3, -2, 4, 6, 2, 0, -4, 1, 7, 9 };
		int subarraySize = 3;

		#region printout0_Explainer

		Console.WriteLine($"Given this array: nums = [{string.Join(", ", array)}] ");
		Console.WriteLine($"and a subarray size of k = {subarraySize}, ");
		Console.WriteLine($"we need to find the highest average subarray(s) \n\n");

		#endregion printout0_Explainer

		//
		//if you want only 1 highest average array result, even if there are ties
		//

		#region method1_GetHighestMaxAverage()

		static (int[] subArray, double maxAvg) GetHighestMaxAverage(int[] nums, int k)
		{
			double maxAverage = double.MinValue;
			int startIndex = 0;

			for (int i = 0; i <= nums.Length - k; i++)
			{
				double sum = 0;
				for (int j = i; j < i + k; j++)
				{
					sum += nums[j];
				}

				double average = sum / k;
				if (average > maxAverage)
				{
					maxAverage = average;
					startIndex = i;
				}
			}

			// Construct the subarray with the highest average
			int[] resultSubarray = nums.Skip(startIndex).Take(k).ToArray();

			return (resultSubarray, maxAverage);
		}

		#region printout1

		var result = GetHighestMaxAverage(array, subarraySize);
		Console.WriteLine();
		Console.WriteLine($"In case we want ONLY ONE result, regardless of ties: " +
			$"\nwe GetHighestMaxAverage() ...(no plural)...");
		Console.WriteLine($"\nThe highest average of subarrays = {result.maxAvg:F3}");
		Console.WriteLine($"The subarray with the highest average: ");
		Console.WriteLine($"    [{string.Join(", ", result.subArray)}] \n\n");

		#endregion printout1

		#endregion method1_GetHighestMaxAverage()

		//
		//what if multiple subarrays tie for highest average?
		//

		#region method2_GetHighestMaxAverages()

		static (List<int[]> subArrays, double maxAvg) GetHighestMaxAverages(int[] nums, int k)
		{
			int n = nums.Length;
			double maxAverage = double.MinValue;
			List<int[]> maxSubarrays = new();

			for (int i = 0; i <= n - k; i++)
			{
				double sum = 0;
				for (int j = i; j < i + k; j++)
				{
					sum += nums[j];
				}

				double average = sum / k;
				if (average > maxAverage)
				{
					maxAverage = average;
					maxSubarrays.Clear(); // Clear previous max subarrays
					maxSubarrays.Add(nums.Skip(i).Take(k).ToArray());
				}
				else if (Math.Abs(average - maxAverage) < 1e-3)
				{
					// If tied for max average (within .001), add this subarray to the list
					maxSubarrays.Add(nums.Skip(i).Take(k).ToArray());
				}
			}

			return (maxSubarrays, maxAverage);
		}

		#region printout2

		var results = GetHighestMaxAverages(array, subarraySize);
		Console.WriteLine();
		Console.WriteLine($"In case of subarrays TIED for highest average, " +
			$"\nwe GetHighestMaxAverages() ...(plural)...");
		Console.WriteLine($"\nThe highest average of subarray(s) = {results.maxAvg:F3}");
		Console.WriteLine($"The subarray(s) with the highest average: ");
		Console.WriteLine("[ ");
		foreach (var sub in results.subArrays)
		{
			Console.WriteLine($"    [{string.Join(", ", sub)}]");
		}
		Console.WriteLine("] \n\n");

		#endregion printout2

		#endregion method2_GetHighestMaxAverages()

		//
		//what if multiple subarrays tie for highest average?
		//AND
		//what if the last array elements fill out their subarray size by circling around to the array start?
		//e.g., nums=[1,2,3] with k=2 would produce 3 subarrays, the last being [3,1]
		//

		#region method3_GetHighestMaxOuroboros()

		static (List<int[]> subArrays, double maxAvg) GetHighestMaxOuroboros(int[] nums, int k)
		{
			int n = nums.Length;
			double maxAverage = double.MinValue;
			List<int[]> maxSubarrays = new();

			for (int i = 0; i < n; i++)
			{
				double sum = 0;
				int[] currentSubarray = Array.Empty<int>();
				for (int j = 0; j < k; j++)
				{
					int index = (i + j) % n; // Wrap around to the beginning if needed
					int currentVal = nums[index];
					sum += currentVal;
					currentSubarray = currentSubarray.Append(currentVal).ToArray();
					//Console.WriteLine($"i = {i}, currentVal = {currentVal}, currentSubarray = [{string.Join(", ", currentSubarray)}]");
				}

				double average = sum / k;
				if (average > maxAverage)
				{
					maxAverage = average;
					maxSubarrays.Clear(); // Clear previous max subarrays
					maxSubarrays.Add(currentSubarray);
					//Console.WriteLine($"i = {i}, currentSubarray = [{string.Join(", ", currentSubarray)}]");
				}
				else if (Math.Abs(average - maxAverage) < 1e-3)
				{
					// If tied for max average (within .001), add this subarray to the list
					maxSubarrays.Add(currentSubarray);
					//Console.WriteLine($"i = {i}, currentSubarray = [{string.Join(", ", currentSubarray)}]");
				}
				//Console.WriteLine($"i = {i}, sum = {sum}, average = {average}, maxAverage = {maxAverage}");
			}

			return (maxSubarrays, maxAverage);
		}

		#region printout3

		var resultx = GetHighestMaxOuroboros(array, subarraySize);
		Console.WriteLine();
		Console.WriteLine($"In case of subarrays TIED for highest average, with CIRCULAR array " +
			$"\n(so the last array elements fill out their subarray size by circling around to the array's start), " +
			$"\nwe GetHighestMaxAveragesOuroboros() ...(plural,circular)...");
		Console.WriteLine($"\nThe highest average of subarray(s) = {resultx.maxAvg:F3}");
		Console.WriteLine($"The subarray(s) with the highest average: ");
		Console.WriteLine("[ ");
		foreach (var sub in resultx.subArrays)
		{
			Console.WriteLine($"    [{string.Join(", ", sub)}]");
		}
		Console.WriteLine("] \n\n");

		#endregion printout3

		#endregion method3_GetHighestMaxOuroboros()
	}
}
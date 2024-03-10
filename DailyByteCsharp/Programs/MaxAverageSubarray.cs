using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MaxAverageSubarray
{
	public static void Test()
	{
		int[] array = { 1, 2, 3, -1, 4, 2, 6, 0, 9, 1, 7, 1, 3, -2, 4, 6, 2, 0, -4, 1, 7, 9, 1 };
		int subarraySize = 3;

		//
		//if you want only 1 highest average array result, even if ties
		//

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

		var result = GetHighestMaxAverage(array, subarraySize);
		Console.WriteLine();
		Console.WriteLine($"In case of THE subarray, or 1ST subarray with the highest average, we GetHighestMaxAverage() ...(no plural)...");
		Console.WriteLine($"The highest average of subarrays: {result.maxAvg}");
		Console.WriteLine($"The subarray with the highest average: ");
		Console.WriteLine($"    [{string.Join(", ", result.subArray)}]");

		//
		//what if multiple subarrays tie for highest average?
		//

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
					// If tied for max average as close as .001, add this subarray to the list
					maxSubarrays.Add(nums.Skip(i).Take(k).ToArray());
				}
			}

			return (maxSubarrays, maxAverage);
		}

		var results = GetHighestMaxAverages(array, subarraySize);
		Console.WriteLine();
		Console.WriteLine($"In case of subarrays tied for highest average, we GetHighestMaxAverages() ...(plural)...");
		Console.WriteLine($"The highest average of subarray(s): {results.maxAvg}");
		Console.WriteLine($"The subarray(s) with the highest average: ");
		Console.WriteLine("[ ");
		foreach (var sub in results.subArrays)
		{
			Console.WriteLine($"    [{string.Join(", ", sub)}]");
		}
		Console.WriteLine("] ");
	}
}
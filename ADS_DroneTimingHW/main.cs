using System;
using System.IO;

class HelloWorld
{
    static void Main()
    {
        int numRepeat = 1000; // Number of repetitions for each function to average timing
        int max = 1000;     // Increase to 1000000 for larger runs
        int min = 100;       // Minimum number of drones
        int stepsize = 100;  // Step size for number of drones
        int numsteps = (max - min) / stepsize;

        // Arrays to store average timings for each function
        float[] timeAverage = new float[numsteps];
        float[] timeBubbleSort = new float[numsteps];
        float[] timeMin = new float[numsteps];
        float[] timeMax = new float[numsteps];

        for (int i = 0; i < numsteps; i++)
        {
            int numdrones = i * stepsize + min;
            Console.WriteLine("Current num drones = " + numdrones);

            Flock flock = new Flock(numdrones);
            flock.Init(numdrones); // Initialize flock with numdrones

            var watch = new System.Diagnostics.Stopwatch();

            // Timing for average()
            watch.Start();
            for (int rep = 0; rep < numRepeat; rep++)
            {
                flock.average();
            }
            watch.Stop();
            timeAverage[i] = watch.ElapsedMilliseconds / (float)numRepeat;

            // Timing for bubble sort
            watch.Restart();
            for (int rep = 0; rep < numRepeat; rep++)
            {
                flock.bubblesort();
            }
            watch.Stop();
            timeBubbleSort[i] = watch.ElapsedMilliseconds / (float)numRepeat;

            // Timing for min()
            watch.Restart();
            for (int rep = 0; rep < numRepeat; rep++)
            {
                flock.min();
            }
            watch.Stop();
            timeMin[i] = watch.ElapsedMilliseconds / (float)numRepeat;

            // Timing for max()
            watch.Restart();
            for (int rep = 0; rep < numRepeat; rep++)
            {
                flock.max();
            }
            watch.Stop();
            timeMax[i] = watch.ElapsedMilliseconds / (float)numRepeat;
        }

        // Write results to CSV
        using (StreamWriter file = new StreamWriter("results.csv"))
        {
            file.WriteLine("NumDrones,AverageTime,BubbleSortTime,MinTime,MaxTime");
            for (int i = 0; i < numsteps; i++)
            {
                int numdrones = i * stepsize + min;
                file.WriteLine($"{numdrones},{timeAverage[i]},{timeBubbleSort[i]},{timeMin[i]},{timeMax[i]}");
            }
        }

        Console.WriteLine("Results written to results.csv");
    }
}
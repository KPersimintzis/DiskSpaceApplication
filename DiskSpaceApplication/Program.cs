using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskSpaceApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            //int[] used = { 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49 };
            //int[] total = { 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50 };

            //int[] used = { 750, 800, 850, 900, 950 };
            //int[] total = { 800, 850, 900, 950, 1000 };

            //int[] used = { 300, 525, 110 };
            //int[] total = { 350, 600, 115 };

            //int[] used = { 1, 200, 200, 199, 200, 200 };
            //int[] total = { 1000, 200, 200, 200, 200, 200 };

            //int[] used = { 331, 242, 384, 366, 428, 114, 145, 89, 381, 170, 329, 190, 482, 246, 2, 38, 220, 290, 402, 385 };
            //int[] total = { 992, 509, 997, 946, 976, 873, 771, 565, 693, 714, 755, 878, 897, 789, 969, 727, 765, 521, 961, 906 };

            //int[] used = { 8, 500, 200, 15, 60 };
            //int[] total = { 1000, 500, 800, 200, 150 };

            int[] used;
            int[] total;
            Input.InputDisks(out used, out total);
            Console.WriteLine(new DiskSpace().minDrives(used, total) + " hard drives in use"); 

        }
    }

    class DiskSpace 
    {        
        public int minDrives(int[] used, int[] total)
        {
            Array.Sort(total, used);
            int cleanDisks = 0; // every time the amount of used space gets 0 , cleanDisks will increase by 1.
            if (total.Length == 0) { return 0; }
            int maxIndex = total.Length - 1;
            int minIndex = 0;
            while (minIndex != maxIndex && CheckIfSplitable(used, total))
            {
                cleanDisks = Transfer(ref minIndex, maxIndex, cleanDisks, used, total);
                maxIndex--;
            }
            return used.Length - cleanDisks;
        }



        //check the NON empty hard drives if they have enough space to fill from other drive.
        //for example : Free Space: 200  but the disk with less occupied space has 300 so no changes can be done.
        public bool CheckIfSplitable(int[] used, int[] total) => (total.Sum() - used.Sum() > used.Min()) ? true : false;



        //Transfer data from one disk to other. 
        public int Transfer(ref int minIndex, int max, int result, int[] used, int[] total)
        {
            if (total[max] - used[max] >= used[minIndex]) // check if the disk[min] occupies less disk than the free space of disk[max] if so we completely tranfer it to disk[max]
            {                                       // if not(command else and below), we move the exact space which the disk[max] will be full
                used[max] += used[minIndex];
                used[minIndex] = 0;
                total[minIndex] = 0;
                minIndex++;
                return Transfer(ref minIndex,max, result + 1, used, total); //we call the function again until the disk[max] be full.
            }
            else
            {
                int diff = used[minIndex] - (total[max] - used[max]);
                used[minIndex] = diff;
                used[max] = total[max];
            }
            return result;
        }

    }

    class Input 
    {
        public static void InputDisks(out int[] used, out int[] total)
        {
            Console.Write("Disks in use: \t[max:50]\n>");
            int lenght = CheckInput(0, 50);
            used = new int[lenght];
            total = new int[lenght];
            int i = 0;
            while (i < lenght)
            {
                Console.Write("\nTotal disk capacity of drive " + i + ": \t[max:1000]\t\n>");
                int totalValue = CheckInput(0, 1000);
                Console.Write("Amount of disk space used in drive " + i + ": \t[max:1000]\t\n>");
                int usedValue = CheckInput(0, 1000);
                if (!ValidationForProperDiskInputs(usedValue, totalValue))
                {
                    used[i] = usedValue;
                    total[i] = totalValue;
                    i++;
                }
            }
        }

        public static int CheckInput(int min, int max)  // check if the input is integer and if it is inside the boundaries
        {
            int tempint;
            string input = Console.ReadLine();
            while (!int.TryParse(input, out tempint) || (tempint < min) || (tempint > max))
            {
                Console.WriteLine("Invalid Number");
                Console.Write($"Please type a number between {min} and {max}: ");
                input = Console.ReadLine();
            }
            return tempint;
        }

        public static bool ValidationForProperDiskInputs(int use, int total) // check if the used space is smaller or equal than total space
        {
            if (use > total)
            {
                Console.WriteLine("Amount of disk space used in drives must always be less than or equal to total capacity!\n");
                return true;
            }
            return false;
        }
    }


}

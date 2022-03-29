using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caculate_Wave
{
    class Program
    {
        static void Main(string[] args)
        {
        }



        public static int findMaxIndex(int[] arr, int begin, int end)
        {
            int center = (begin + end) / 2;

            //如果中间元素处于上坡的位置
            if (arr[center] > arr[center - 1] && arr[center] < arr[center + 1])
            {
                begin = center + 1;
                return findMaxIndex(arr, begin, end);

            }//如果处于下坡的位置
            else if (arr[center] < arr[center - 1] && arr[center] > arr[center + 1])
            {
                end = center - 1;
                return findMaxIndex(arr, begin, end);
            }
            else
            {//此种情况是当arr[center-1]<arr[center]<arr[center+1]因为此数组是单峰数组，

                return center;
            }
        }

        public static void main(String[] args)
        {
            int[] arr = new int[]{ 1, 2, 3, 5, 6, 7, 8, 9, 10, 4, 3, 2, 1 };
           Console.WriteLine(findMaxIndex(arr, 0, arr.Length));
        }
    }
}

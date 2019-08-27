using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineBase.Helper.RandomHelper
{
    public class RandomQuestion<T> where T : class
    {
        public static List<T> Randomize(List<T> list)
        {
            List<T> randomizedList = new List<T>();
            Random rnd = new Random();
            while (list.Count > 0)
            {
                int index = rnd.Next(0, list.Count); //pick a random item from the master list
                randomizedList.Add(list[index]); //place it at the end of the randomized list
                list.RemoveAt(index);
            }
            return randomizedList;
        }
    }
}

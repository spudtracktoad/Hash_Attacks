using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hash_Attacks
{
    class Program
    {
        //Assumption: these are the only valid charaters
        public static char[] validCharters = new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                                                         'A', 'B','C', 'D', 'E', 'F','G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};
        static void Main(string[] args)
        {
            int[] testSizes = new int[] { 8, 12, 16, 20 };
            Dictionary<int, List<int>> CollisionDict = new Dictionary<int, List<int>>();
            Dictionary<int, List<int>> PreImageDict = new Dictionary<int, List<int>>();
            Dictionary<string, int> hashDict = new Dictionary<string, int>();
            SimpleSHA1 testSha = new SimpleSHA1();
            string baseCase = "testTEST";
            Random random = new Random();

            foreach (var item in testSizes)
            {
                hashDict.Clear();
                var BaseResult = testSha.ComputeHash(item, baseCase);
                List<int> collisionList = new List<int>();
                List<int> preimageList = new List<int>();
                for (int sample = 0; sample < 50; sample++)
                {
                    //preimage
                    BitArray testResult;
                    int count = 0;
                    bool collision;
                    do
                    {
                        collision = true;
                        count++;
                        char[] test = new char[item];
                        for (int i = 0; i < item; i++)
                        {
                            test[i] = validCharters[random.Next(0, validCharters.Length)];
                        }

                        testResult = testSha.ComputeHash(item, new string(test));
                        for (int d = 0; d < item; d++)
                        {
                            if (BaseResult.Get(d) != testResult.Get(d))
                            {
                                collision = false;
                                break;
                            }
                        }
                    } while (!collision);
                    preimageList.Add(random.Next(0, count));
                    count = 0;
                    //Collision
                    do
                    {
                        collision = false;
                        count++;
                        char[] test = new char[item];
                        for (int i = 0; i < item; i++)
                        {
                            test[i] = validCharters[random.Next(0, validCharters.Length)];
                        }
                        var testStr = new string(test);
                        testResult = testSha.ComputeHash(item, testStr);
                        byte[] strArr = new byte[testResult.Length / 8];
                        for (int i = 0; i < testResult.Length / 8; i++)
                        {
                            for (int index = i * 8, m = 1; index < i * 8 + 8; index++, m *= 2)
                            {
                                strArr[i] += testResult.Get(index) ? (byte)m : (byte)0;
                            }
                        }
                        var hashStr = Encoding.ASCII.GetString(strArr);
                        if (hashDict.ContainsKey(hashStr))
                        {
                            int tmpCount;
                            hashDict.TryGetValue(hashStr, out tmpCount);
                            count += tmpCount;
                            hashDict[hashStr] = count;

                            collision = true;
                        }
                        else
                        {
                            hashDict.Add(hashStr, count);
                        }

                    } while (!collision);
                    collisionList.Add(count);
                }
                PreImageDict.Add(item, preimageList);
                CollisionDict.Add(item, collisionList);
            }

            using (var writer = new StreamWriter(@"C:\Users\spudt\Documents\Fall 2020\Applied Cryptology\Labs\Hash_Attack\PreImageDict.csv"))
            {
                foreach (var pair in PreImageDict)
                {
                    writer.WriteLine("{0},{1};", pair.Key, String.Join(",", pair.Value.Select(x => x.ToString()).ToArray()));
                }
            }

            using (var writer = new StreamWriter(@"C:\Users\spudt\Documents\Fall 2020\Applied Cryptology\Labs\Hash_Attack\CollisionDict.csv"))
            {
                foreach (var pair in CollisionDict)
                {
                    writer.WriteLine("{0},{1};", pair.Key, String.Join(",", pair.Value.Select(x => x.ToString()).ToArray()));
                }
            }

            Console.WriteLine("start time: {0}", DateTime.Now);
            Console.ReadLine();
        }
    }
}

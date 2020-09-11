using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Collections;

namespace Hash_Attacks
{
    public class SimpleSHA1
    {
        private int Length;
        private SHA1Managed sHA1Managed;

        byte[] testHashInput = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70, 0x80, 0x90, 0xa0, 0xb0, 0xc0, 0xd0, 0xe0, 0xf0 };
        //byte[] HashInput = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70, 0x80, 0x90, 0xa0, 0xb0, 0xc0, 0xd0, 0xe0, 0xf1 };

        public SimpleSHA1()
        {
            sHA1Managed = new SHA1Managed();
        }

        public BitArray ComputeHash(int length, string input)
        {
            Length = length;
            sHA1Managed.ComputeHash(Encoding.ASCII.GetBytes(input));
            return buildHashOutput(sHA1Managed.Hash);
        }

        private BitArray buildHashOutput(byte[] input)
        {
            BitArray result = new BitArray(Length);
            var test = new BitArray(input);
                        
            for (int i = 0; i < Length; i++)
            {
                result.Set(i, test.Get(i));
            }

            return result;
        }

        private String BitArrayToStr(BitArray ba)
        {
            byte[] strArr = new byte[ba.Length / 8];

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            for (int i = 0; i < ba.Length / 8; i++)
            {
                for (int index = i * 8, m = 1; index < i * 8 + 8; index++, m *= 2)
                {
                    strArr[i] += ba.Get(index) ? (byte)m : (byte)0;
                }
            }

            return encoding.GetString(strArr);
        }

        public int CountToNextCollision()
        { return 0; }
    }
}

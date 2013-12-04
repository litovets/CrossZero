using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] data = { 1, 260, 300, 0 };
            byte[] buffer = new byte[data.Length * sizeof(int)];
            Buffer.BlockCopy(data, 0, buffer, 0, buffer.Length);
            foreach (byte b in buffer)
            {
                Console.Write(b + ", ");
            }
            Console.ReadLine();

            int[] backData = new int[buffer.Length/sizeof (int)];
            Buffer.BlockCopy(buffer, 0, backData, 0, buffer.Length);
            foreach (int i in backData)
            {
                Console.Write(i + ", ");
            }
            Console.ReadLine();
        }
    }
}

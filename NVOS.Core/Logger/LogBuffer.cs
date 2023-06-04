using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Logger
{
    public class LogBuffer<T>
    {
        private T[] buffer;
        private int writeIndex;

        public LogBuffer(int size)
        {
            buffer = new T[size];
            writeIndex = 0;
        }

        public void Write(T item)
        {
            buffer[writeIndex] = item;
            writeIndex = (writeIndex + 1) % buffer.Length;
        }

        public T[] ReadItems()
        {
            T[] result = new T[buffer.Length];
            Array.Copy(buffer, result, buffer.Length);
            return result;
        }
    }
}

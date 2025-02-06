using System.Collections.Generic;

namespace FractalFactory.Common
{
    public class GenericStack<T>
    {
        private Stack<T> stack = new Stack<T>();

        public GenericStack() { }

        public void Clear() { stack.Clear(); }

        public T Push(T item)
        {
            stack.Push(item);
            return item;
        }

        public T Pop() { return stack.Pop(); }

        public T Peek() { return stack.Peek(); }

        public int Count { get { return stack.Count; } }
    }
}

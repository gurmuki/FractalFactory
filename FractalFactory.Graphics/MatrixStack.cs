using OpenTK.Mathematics;
using System.Collections.Generic;

namespace FractalFactory.Graphics
{
    public class Transform
    {
        public Matrix4 translation;
        public Matrix4 scale;
    }

    public class MatrixStack
    {
        private Stack<Transform> transforms = new Stack<Transform>();

        public MatrixStack() { }

        public void Clear()
        {
            transforms.Clear();
        }

        public void Push(Matrix4 t, Matrix4 s)
        {
            if (Count > 0)
            {
                Transform top = Peek();
                if (t.Equals(top.translation) && s.Equals(top.scale))
                    return;
            }

            // Debug.WriteLine("Push()");
            Transform transform = new Transform();
            transform.translation = MatrixCopy(t);
            transform.scale = MatrixCopy(s);
            transforms.Push(transform);
        }

        public Transform Pop()
        {
            // Debug.WriteLine("Pop()");
            return transforms.Pop();
        }

        public Transform Peek() { return transforms.Peek(); }

        public int Count { get { return transforms.Count; } }

        private Matrix4 MatrixCopy(Matrix4 m)
        {
            return new Matrix4(m.Row0, m.Row1, m.Row2, m.Row3);
        }
    }
}

using System.Collections.Generic;

namespace FractalFactory.Graphics
{
    public class CameraStack
    {
        private Stack<Camera> stack = new Stack<Camera>();

        public CameraStack() { }

        public void Clear() { stack.Clear(); }

        public Camera Push(Camera camera)
        {
            stack.Push(camera);
            return camera;
        }

        public Camera Pop() { return stack.Pop(); }

        public Camera Peek() { return stack.Peek(); }

        public int Count { get { return stack.Count; } }
    }
}

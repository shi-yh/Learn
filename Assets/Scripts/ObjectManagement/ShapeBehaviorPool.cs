using System.Collections.Generic;

namespace ObjectManagement
{
    public static class ShapeBehaviorPool<T> where T : ShapeBehavior, new()
    {
        private static Stack<T> _stack = new Stack<T>();

        public static T Get()
        {
            if (_stack.Count>0)
            {
                return _stack.Pop();
            }

            return new T();
        }

        public static void Reclaim(T behavior)
        {
            _stack.Push(behavior);
        }
    
    
    }
}
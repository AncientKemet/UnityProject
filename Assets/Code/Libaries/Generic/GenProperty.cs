using System;

namespace OldBlood.Code.Libaries.Generic
{
    public class GenProperty<T> 
    {
        private T _t;

        public T Get
        {
            get
            {
                if(_t == null)
                {
                    _t = (T)Activator.CreateInstance(typeof(T), null);
                }
                    return _t;
            }
        }
    }
}


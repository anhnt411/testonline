using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineShared.Model
{
    public class Ref<T> where T : class
    {
        public Ref() { }
        public Ref(T value) { Value = value; }
        public T Value { get; set; }
        public static implicit operator T(Ref<T> r) { return r.Value; }
        public static implicit operator Ref<T>(T value) { return new Ref<T>(value); }
    }
}
 
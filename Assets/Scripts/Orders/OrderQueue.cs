using System;
using System.Collections.Generic;

public struct OrderQueue
{
        public readonly Queue<OrderPacket> Queue
        {
                get
                {
                        Queue<OrderPacket> result = new Queue<OrderPacket>();
                        for (int i = 0; i < _index; i++)
                        {
                                result.Enqueue(_array[i]);
                        }

                        return result;
                }
        }
        private OrderPacket[] _array;
        private int _index;
        private int _capacity;

        public void Initialize()
        {
                _array = new OrderPacket[0];
        }

        public void Add(OrderPacket packet)
        {
                _index++;
                if (_index > _capacity)
                {
                        _capacity += 10;
                        Array.Resize(ref _array, _capacity);
                }

                _array[_index - 1] = packet;
        }

        public void Reset()
        {
                _array = new OrderPacket[0];
                _capacity = 0;
                _index = 0;
        }

        public override string ToString()
        {
                string result = "";
                for (int i = 0; i < _index; i++)
                {
                        result += _array[i];
                }

                return result;
        }
}
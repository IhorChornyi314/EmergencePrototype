using System;
using Unity.Netcode;
using Unity.VisualScripting;

public class OrderPacket
{
        public bool Reset;
        public int[] EntityIds;
        public Order Order;

        public OrderPacket(bool r, int[] e, Order o)
        {
                Reset = r;
                EntityIds = e;
                Order = o;
        }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Entity;
using MCForge.World;

namespace MCForge.API.PlayerEvent
{
    public enum ActionType : byte
    {
        Delete,
        Place
    }
    public class OnPlayerBlockChange : Event, PlayerEvent, Cancelable
    {
        public delegate void OnCall(OnPlayerBlockChange eventargs);
        ushort x;
        byte holding;
        ushort y;
        ushort z;
        ActionType action;
        bool _canceled;
        Player p;
        public OnPlayerBlockChange(ushort x, ushort y, ushort z, ActionType action, Player p, byte b) { this.x = x; this.y = y; this.z = z; this.action = action; this.p = p; this.holding = b; }
        internal OnPlayerBlockChange() { }
        public ushort GetX()
        {
            return x;
        }
        public ushort GetY()
        {
            return y;
        }
        public ushort GetZ()
        {
            return z;
        }
        public byte GetPlayerHolding()
        {
            return holding;
        }
        public ActionType GetAction()
        {
            return action;
        }
        public Player GetPlayer()
        {
            return p;
        }
        public bool IsCanceled { get { return _canceled; } }
        public void Cancel(bool value)
        {
            _canceled = value;
        }
        public override void Call()
        {
            EventHelper.cache.ForEach(e =>
            {
                if (e.GetType() == GetType())
                    ((OnCall)e.Delegate)(this);
            });
            if (IsCanceled)
                p.SendBlockChange(x, y, z, holding);
        }

        /// <summary>
        /// Register this event
        /// </summary>
        /// <param name="method">The method to call when this event gets excuted</param>
        /// <param name="priority">The importance of the call</param>
        public static void Register(OnCall method, Priority priority)
        {
            EventHelper temp = new EventHelper(method, priority, new OnPlayerBlockChange());
            EventHelper.Push(temp);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utubz.Async
{
    public sealed class InputContext : Object
    {
        private int size;
        private sbyte[] state;
        private sbyte[] updated;
        private sbyte[] frame;
        private double[] mdata;
        private Vector2 mvec;

        public bool KeyHeld(Key key)
        {
            return state[(int)key] > 0;
        }

        public bool KeyDown(Key key)
        {
            return state[(int)key] == 2;
        }

        public bool KeyUp(Key key)
        {
            return state[(int)key] == -1;
        }

        public bool KeyHeld(int key)
        {
            return state[key] > 0;
        }

        public bool KeyDown(int key)
        {
            return state[key] == 1;
        }

        public bool KeyUp(int key)
        {
            return state[key] == -1;
        }

        public int Get(Key key)
        {
            return state[(int)key];
        }

        public int Get(int key)
        {
            return state[key];
        }

        public Vector2 MousePos()
        {
            mvec.x = (float)mdata[0];
            mvec.y = (float)mdata[1];
            return mvec;
        }

        public Vector2 MouseDelta()
        {
            mvec.x = (float)mdata[2] - (float)mdata[0];
            mvec.y = (float)mdata[3] - (float)mdata[1];
            return mvec;
        }

        public Vector2 MouseScroll()
        {
            mvec.x = (float)mdata[4];
            mvec.y = (float)mdata[5];
            return mvec;
        }

        public void Update()
        {
            Array.Clear(state, 0, size);
            Array.Copy(updated, 0, state, 0, size);
            Array.Copy(frame, 0, updated, 0, size);
            mdata[0] = mdata[2];
            mdata[1] = mdata[3];
            mdata[4] = mdata[6];
            mdata[5] = mdata[7];
        }

        public void Modify(Key key, bool pressed)
        {
            Change((int)key, pressed ? 1 : 0);
        }

        public void Modify(int key, bool pressed)
        {
            Change(key, pressed ? 1 : 0);
        }

        internal void Change(int key, int state)
        {
            switch (state)
            {
                case 0:
                    updated[key] = -1;
                    frame[key] = 0;
                    break;

                case 1:
                    updated[key] = 2;
                    frame[key] = 1;
                    break;
            }
        }

        internal void Change(double x, double y)
        {
            mdata[2] = x;
            mdata[3] = x;
        }

        internal void Scroll(double x, double y)
        {
            mdata[6] = x;
            mdata[7] = y;
        }

        public Window Window { get; }

        internal InputContext(int size)
        {
            this.size = size;
            state = new sbyte[size];
            updated = new sbyte[size];
            frame = new sbyte[size];
            mdata = new double[8];
            mvec = Vector2.Zero;
        }
    }
}

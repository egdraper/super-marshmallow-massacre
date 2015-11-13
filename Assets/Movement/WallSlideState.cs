using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.PlayerMovement
{
    class WallSlideState : IMovement
    {
        private Scripts.Movement _movement;

        public WallSlideState(Scripts.Movement movement)
        {
            this._movement = movement;
        }

        public void Die()
        {
            throw new NotImplementedException();
        }

        public void Fall()
        {
            throw new NotImplementedException();
        }

        public void Jump()
        {
            throw new NotImplementedException();
        }

        public void RecieveHit()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public void SlideDownWall()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void ThrowObject()
        {
            throw new NotImplementedException();
        }

        public void Walk()
        {
            throw new NotImplementedException();
        }
    }
}

using UnityEngine;
using System.Collections;

namespace Assets.PlayerMovement
{
    public interface IMovement
    {
        void Die();
        void RecieveHit();
        void Jump();
        void Fall();
        void Run();
        void Walk();
        void Stop();
        void ThrowObject();
        void SlideDownWall();


    }
}

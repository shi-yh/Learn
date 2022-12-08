using UnityEngine;

namespace Tower_Defense
{
   public abstract class GameBehavior : MonoBehaviour
   {
      public virtual bool GameUpdate() => true;
      public abstract void Recycle();
   }
}

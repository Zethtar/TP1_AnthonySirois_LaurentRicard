using System.Collections;
using UnityEngine;

namespace Playmode.Entity.Destruction
{
    public class RootDestroyer : Destroyer
    {
        public override void Destroy()
        {
            StartCoroutine(DestroyAtEndOfFrame());
        }

        private IEnumerator DestroyAtEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            Destroy(transform.root.gameObject);
        }
    }
}
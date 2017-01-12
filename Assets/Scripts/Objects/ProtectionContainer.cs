using UnityEngine;
using Game.Interface;
//By @JavierBullrich

namespace Game.Obj {
	public class ProtectionContainer : MonoBehaviour, IReset {
        public Protection[] protections;

        public void Respawn()
        {
            foreach (Protection prot in protections)
            {
                prot.Respawn();
            }
        }
    }
}
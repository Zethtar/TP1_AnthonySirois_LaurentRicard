using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playmode.Entity.Senses
{
	public delegate void PickupSensorEventHandler();
	
	public class PickupSensor : MonoBehaviour {

		public event PickupSensorEventHandler OnPickup;

		public void Hit(int hitPoints)
		{
			NotifyPickup();
		}

		private void NotifyPickup()
		{
			if (OnPickup != null) OnPickup();
		}
	}
}



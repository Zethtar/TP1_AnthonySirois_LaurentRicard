using System.Collections;
using System.Collections.Generic;
using Playmode.Pickable;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public delegate void PickableSightSensorEventHandler(PickableController pickable);
    
	public class PickableSightSensor : MonoBehaviour
	{
	    public event PickableSightSensorEventHandler OnPickableSeen;
	    public event PickableSightSensorEventHandler OnPickableSightLost;
		
		public void See(PickableController pickable)
		{
			NotifyPickableSeen(pickable);
		}

		public void LooseSightOf(PickableController pickable)
		{
			NotifyPickableSightLost(pickable);
		}

		private void NotifyPickableSeen(PickableController pickable)
		{
			OnPickableSeen?.Invoke(pickable);
		}

		private void NotifyPickableSightLost(PickableController pickable)
		{
			OnPickableSightLost?.Invoke(pickable);
		}
	}
}



using System.Collections;
using System.Collections.Generic;
using Playmode.Pickable;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public delegate void PickableSightSensorEventHandler(PickableController pickable);
    
	public class PickableSightSensor : MonoBehaviour
	{
	    private ICollection<PickableController> pickablesInSight;

	    public event PickableSightSensorEventHandler OnPickableSeen;
	    public event PickableSightSensorEventHandler OnPickableSightLost;

	    public IEnumerable<PickableController> PickablesInSight => pickablesInSight;

		private void Awake()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			pickablesInSight = new HashSet<PickableController>();
		}
		
		public void See(PickableController pickable)
		{
			pickablesInSight.Add(pickable);

			NotifyPickableSeen(pickable);
		}

		public void LooseSightOf(PickableController pickable)
		{
			pickablesInSight.Remove(pickable);

			NotifyPickableSightLost(pickable);
		}

		private void NotifyPickableSeen(PickableController pickable)
		{
			if (OnPickableSeen != null) OnPickableSeen(pickable);
		}

		private void NotifyPickableSightLost(PickableController pickable)
		{
			if (OnPickableSightLost != null) OnPickableSightLost(pickable);
		}

	}
}



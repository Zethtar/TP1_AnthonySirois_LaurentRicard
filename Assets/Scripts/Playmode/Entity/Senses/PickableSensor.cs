using System.Collections;
using System.Collections.Generic;
using Playmode.Pickable;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public delegate void PickableSightSensorEventHandler(PickableController pickable);
    
	public class PickableSensor : MonoBehaviour
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
		
		private void See(PickableController pickable)
		{
			pickablesInSight.Add(pickable);

			NotifyEnnemySeen(pickable);
		}

		private void LooseSightOf(PickableController pickable)
		{
			pickablesInSight.Remove(pickable);

			NotifyEnnemySightLost(pickable);
		}

		private void NotifyEnnemySeen(PickableController pickable)
		{
			if (OnPickableSeen != null) OnPickableSeen(pickable);
		}

		private void NotifyEnnemySightLost(PickableController pickable)
		{
			if (OnPickableSightLost != null) OnPickableSightLost(pickable);
		}

	}
}



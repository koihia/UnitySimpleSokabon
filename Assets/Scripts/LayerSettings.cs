using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sokabon
{
	[CreateAssetMenu(fileName = "LayerSettings", menuName = "Sokabon/LayerSettings", order = 0)]
	public class LayerSettings : ScriptableObject
	{
		//ScriptableObjects are great and we love them.
		
		public LayerMask solidLayerMask;
		public LayerMask blockLayerMask;
		public LayerMask triggerLayerMask;
	}
}
using System;
using System.Collections.Generic;
using XRL.Rules;
using XRL.UI;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_GooglyEyes : IPart
	{
		public acegiak_GooglyEyes()
		{
			base.Name = "Googly Eyes";
		}

		public override bool SameAs(IPart p)
		{
			return false;
		}

		public override void Register(GameObject Object)
		{
			Object.RegisterPartEvent(this, "GetInventoryActions");
			Object.RegisterPartEvent(this, "InvCommandGooglify");
			base.Register(Object);
		}


		public void Googlify(GameObject GO,GameObject Googler){
            GO.AddPart(new acegiak_ModGoogly());
            Popup.Show(GO.The+GO.DisplayNameOnly+GO.GetVerb("look")+" very intimidating.");
            if(GO.pBrain != null){
                GO.pBrain.AdjustFeeling(Googler,-1);
            }
            ParentObject.ForceUnequipAndRemove(true);
        }

		public override bool FireEvent(Event E)
		{
			if (E.ID == "GetInventoryActions")
			{
				EventParameterGetInventoryActions eventParameterGetInventoryActions = E.GetParameter("Actions") as EventParameterGetInventoryActions;
				eventParameterGetInventoryActions.AddAction("Googlify", 'G',  false, "&WG&yooglify", "InvCommandGooglify");
				return true;
			}
			if (E.ID == "InvCommandGooglify")
			{
					string text2 = XRL.UI.PickDirection.ShowPicker();
					if (text2 == null)
					{
						return true;
					}
					Cell cell = null;
					if (ParentObject.pPhysics.Equipped != null)
					{
						cell = ParentObject.pPhysics.Equipped.pPhysics.CurrentCell;
					}
					if (ParentObject.pPhysics.InInventory != null)
					{
						cell = ParentObject.pPhysics.InInventory.pPhysics.CurrentCell;
					}
					if (ParentObject.pPhysics.CurrentCell != null)
					{
						cell = ParentObject.pPhysics.CurrentCell;
					}
					cell = cell.GetCellFromDirection(text2);
					if (cell != null)
					{
							if (cell.Objects.Count > 0 && cell.GetHighestRenderLayerObject()!= null)
							{
								Googlify(cell.GetHighestRenderLayerObject(),E.GetGameObjectParameter("Owner"));
							}else{
                                Popup.Show("The googly eyes don't stick.");
                                return false;
                            };
					}
			}
			return base.FireEvent(E);
		}
	}
}

using System;
using System.Text;
using XRL.Rules;
using XRL.World.Parts.Skill;
using XRL.World.Parts.Effects;
using XRL.UI;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_ModGoogly : IModification
	{
		public int Chance = 2;

		public acegiak_ModGoogly()
		{
		}

		public acegiak_ModGoogly(int Tier)
			: base(Tier)
		{
		}

		public override void Configure()
		{
			WorksOnSelf = true;
		}

		public override bool ModificationApplicable(GameObject Object)
		{
			return true;
		}

		public override void ApplyModification(GameObject Object)
		{
			IncreaseDifficultyAndComplexityIfComplex(1, 1);
		}

		public override bool AllowStaticRegistration()
		{
			return true;
		}

		public override bool SameAs(IPart p)
		{
			return base.SameAs(p);
		}

		public override void Register(GameObject Object)
		{
			Object.RegisterPartEvent(this, "GetDisplayName");
			Object.RegisterPartEvent(this, "GetShortDisplayName");
			Object.RegisterPartEvent(this, "GetShortDescription");
			Object.RegisterPartEvent(this, "WeaponHit");
			Object.RegisterPartEvent(this, "AfterLookedAt");
			base.Register(Object);
		}

		public override bool FireEvent(Event E)
		{
            if (E.ID == "AfterLookedAt")
			{
				GameObject gameObjectParameter = E.GetGameObjectParameter("Looker");
				if(gameObjectParameter == null){
					gameObjectParameter = XRLCore.Core.Game.Player.Body;
				}
				if (gameObjectParameter != null && !gameObjectParameter.HasEffect("Shaken"))
				{
                    if(gameObjectParameter.IsPlayer()){
                        Popup.Show(gameObjectParameter.It+gameObjectParameter.Is+" disturbed by "+ParentObject.the+ParentObject.DisplayNameOnly+"'s intimidating eyes.");
                    }else{
                        IPart.AddPlayerMessage(gameObjectParameter.The+gameObjectParameter.DisplayNameOnly+gameObjectParameter.Is+" disturbed by "+ParentObject.the+ParentObject.DisplayNameOnly+"'s intimidating eyes.");
                    }
					gameObjectParameter.ApplyEffect(new Shaken(Stat.Random(300, 360), 1));
				}
			}
			if (E.ID == "WeaponHit")
			{
				if ((Chance >= 100 || Stat.Random(1, 100) <= Chance) && IsReady(UseCharge: true))
				{
					GameObject gameObjectParameter = E.GetGameObjectParameter("Attacker");
					GameObject gameObjectParameter2 = E.GetGameObjectParameter("Defender");
                    IPart.AddPlayerMessage(gameObjectParameter2.The+gameObjectParameter2.DisplayNameOnly+gameObjectParameter2.GetVerb("is")+" disturbed by "+ParentObject.the+ParentObject.DisplayNameOnly+"'s intimidating eyes.");
					gameObjectParameter2.ApplyEffect(new Shaken(Stat.Random(300, 360), 1));				}
			}
			else if (E.ID == "GetShortDescription")
			{
				E.AddParameter("Postfix", E.GetStringParameter("Postfix") + "\n&yg&Ko&yo&Kg&yly: This item has been adorned with an intimidating set of artificial eyes.");
			}
			else if ((E.ID == "GetDisplayName" || E.ID == "GetShortDisplayName") && ParentObject.Understood() && !ParentObject.HasProperName)
			{
				E.GetParameter<StringBuilder>("Prefix").Append("&yg&Ko&yo&Kg&yly &y");
			}
			return base.FireEvent(E);
		}
	}
}

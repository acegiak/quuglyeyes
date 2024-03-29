using System;
using System.Text;
using XRL.Rules;
using XRL.Core;
using XRL.World.Parts.Skill;
using XRL.World.Parts.Effects;
using XRL.UI;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_ModGoogly : IModification
	{
		public int Chance = 5;

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
			Object.RegisterPartEvent(this, "TakeDamage");
			Object.RegisterPartEvent(this, "AfterLookedAt");
			Object.RegisterPartEvent(this, "DefendMeleeHit");
			Object.RegisterPartEvent(this, "Equipped");
			Object.RegisterPartEvent(this, "Unequipped");
			base.Register(Object);
		}

		public override bool FireEvent(Event E)
		{

			int StartAngle = 85;
			int EndAngle = 185;
			float num = 0f;
			float num2 = 0f;
			float num3 = (float)XRL.Rules.Stat.RandomCosmetic(StartAngle, EndAngle) / 58f;
			num = (float)Math.Sin(num3) / 6f;
			num2 = (float)Math.Cos(num3) / 6f;
            if (E.ID == "AfterLookedAt")
			{
				GameObject gameObjectParameter = E.GetGameObjectParameter("Looker");
				if(gameObjectParameter == null){
					gameObjectParameter = IPart.ThePlayer;
				}
				if (gameObjectParameter != null && !gameObjectParameter.HasEffect("Shaken"))
				{
                    if(gameObjectParameter.IsPlayer()){
                        Popup.Show(gameObjectParameter.It+gameObjectParameter.Is+" disturbed by "+ParentObject.the+ParentObject.DisplayNameOnly+"'s intimidating eyes.");
                    }else{
                        IPart.AddPlayerMessage(gameObjectParameter.The+gameObjectParameter.DisplayNameOnly+gameObjectParameter.Is+" disturbed by "+ParentObject.the+ParentObject.DisplayNameOnly+"'s intimidating eyes.");
                    }
					XRLCore.ParticleManager.Add("&R!", gameObjectParameter.CurrentCell.X, gameObjectParameter.CurrentCell.Y, num,num2);

					gameObjectParameter.ApplyEffect(new Shaken(Stat.Random(300, 360), 1));
				}
			}
			if (E.ID == "WeaponHit")
			{
				//IPart.AddPlayerMessage("Chance for scary offense");
				if ((Chance >= 100 || Stat.Random(1, 100) <= Chance))
				{
					GameObject gameObjectParameter = E.GetGameObjectParameter("Attacker");
					GameObject gameObjectParameter2 = E.GetGameObjectParameter("Defender");
                    IPart.AddPlayerMessage(gameObjectParameter2.The+gameObjectParameter2.DisplayNameOnly+gameObjectParameter2.Is+" disturbed by "+ParentObject.the+ParentObject.DisplayNameOnly+"'s intimidating eyes.");
					gameObjectParameter2.ApplyEffect(new Shaken(Stat.Random(300, 360), 1));
					XRLCore.ParticleManager.Add("&R!", gameObjectParameter2.CurrentCell.X, gameObjectParameter2.CurrentCell.Y, num, num2);

				}
			}if (E.ID == "DefendMeleeHit")
			{
				//IPart.AddPlayerMessage("Chance for scary defense");
				if ((ParentObject.GetPart<Armor>() != null || ParentObject.GetPart<Body>() != null)&& (Chance >= 100 || Stat.Random(1, 100) <= Chance))
				{
					GameObject gameObjectParameter = E.GetGameObjectParameter("Attacker");
                    IPart.AddPlayerMessage(gameObjectParameter.The+gameObjectParameter.DisplayNameOnly+gameObjectParameter.Is+" disturbed by "+ParentObject.the+ParentObject.DisplayNameOnly+"'s intimidating eyes.");
					gameObjectParameter.ApplyEffect(new Shaken(Stat.Random(300, 360), 1));

					XRLCore.ParticleManager.Add("&R!", gameObjectParameter.CurrentCell.X, gameObjectParameter.CurrentCell.Y, num, num2);
				}
			}
			else if (E.ID == "GetShortDescription")
			{
				E.AddParameter("Postfix", E.GetStringParameter("Postfix") + "\n&yg&Ko&yo&Kg&yly: This item has been adorned with an intimidating set of artificial eyes.");
			}
			else if ((E.ID == "GetDisplayName" || E.ID == "GetShortDisplayName") && ParentObject.Understood() && !ParentObject.HasProperName)
			{
				E.GetParameter<StringBuilder>("DisplayName").Replace("eyeless ","");
				E.GetParameter<StringBuilder>("Prefix").Append("&yg&Ko&yo&Kg&yly &y");
			}
			else if (E.ID == "Equipped")
			{
				GameObject gameObjectParameter = E.GetGameObjectParameter("EquippingObject");
				// gameObjectParameter.ApplyEffect(new XRL.World.Parts.Effects.Spectacles());
				gameObjectParameter.RegisterPartEvent(this, "DefendMeleeHit");
			}
			else if (E.ID == "Unequipped")
			{
				GameObject gameObjectParameter2 = E.GetGameObjectParameter("UnequippingObject");
				// gameObjectParameter2.RemoveEffect("Spectacles");
				gameObjectParameter2.UnregisterPartEvent(this, "DefendMeleeHit");
			}
			return base.FireEvent(E);
		}
	}
}

using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace EdB.Interface
{
	public class ComponentTabReplacement : IUpdatedComponent, IInitializedComponent, ICustomTextureComponent, IComponentWithPreferences
	{
		protected List<IPreference> preferences = new List<IPreference>();

		protected PreferenceEnableTabReplacement preferenceEnableTabReplacement = new PreferenceEnableTabReplacement();

		protected PreferenceTabBrowseButtons preferenceTabBrowseButtons = new PreferenceTabBrowseButtons();

		protected PreferenceTabCharacter preferenceTabCharacter = new PreferenceTabCharacter();

		protected PreferenceTabGear preferenceTabGear = new PreferenceTabGear();

		protected PreferenceTabNeeds preferenceTabNeeds = new PreferenceTabNeeds();

		protected PreferenceTabHealth preferenceTabHealth = new PreferenceTabHealth();

		protected PreferenceTabGuestAndPrisoner preferenceTabGuestAndPrisoner = new PreferenceTabGuestAndPrisoner();

		protected PreferenceTabTraining preferenceTabTraining = new PreferenceTabTraining();

		protected PreferenceTabGrowing preferenceTabGrowing = new PreferenceTabGrowing();

		protected PreferenceTabBills preferenceTabBills = new PreferenceTabBills();

		protected PreferenceTabArt preferenceTabArt = new PreferenceTabArt();

		protected bool dirtyFlag;

		protected ITab_Bills_Alternate tabBills;

		protected ITab_Growing_Alternate tabGrowing;

		protected ITab_Pawn_Character_Alternate tabCharacter;

		protected ITab_Pawn_Character_Vanilla tabCharacterVanilla;

		protected ITab_Pawn_Gear_Alternate tabGear;

		protected ITab_Pawn_Gear_Vanilla tabGearVanilla;

		protected ITab_Pawn_Needs_Alternate tabNeeds;

		protected ITab_Pawn_Needs_Vanilla tabNeedsVanilla;

		protected ITab_Pawn_Health_Alternate tabHealth;

		protected ITab_Pawn_Health_Vanilla tabHealthVanilla;

		protected ITab_Pawn_Training_Alternate tabTraining;

		protected ITab_Pawn_Training_Vanilla tabTrainingVanilla;

		protected ITab_Pawn_Guest_Alternate tabGuest;

		protected ITab_Pawn_Guest_Vanilla tabGuestVanilla;

		protected ITab_Pawn_Prisoner_Alternate tabPrisoner;

		protected ITab_Pawn_Prisoner_Vanilla tabPrisonerVanilla;

		protected ITab_Art_Alternate tabArt;

		protected ReplacementTabs replacementTabs = new ReplacementTabs();

		public IEnumerable<IPreference> Preferences
		{
			get
			{
				return this.preferences;
			}
		}

		public ComponentTabReplacement()
		{
			this.preferences.Add(this.preferenceTabBrowseButtons);
			this.preferences.Add(this.preferenceEnableTabReplacement);
			this.preferences.Add(this.preferenceTabCharacter);
			this.preferences.Add(this.preferenceTabGear);
			this.preferences.Add(this.preferenceTabNeeds);
			this.preferences.Add(this.preferenceTabHealth);
			this.preferences.Add(this.preferenceTabGuestAndPrisoner);
			this.preferences.Add(this.preferenceTabTraining);
			this.preferences.Add(this.preferenceTabGrowing);
			this.preferences.Add(this.preferenceTabBills);
			this.preferences.Add(this.preferenceTabArt);
			this.preferenceTabCharacter.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.preferenceTabGear.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.preferenceTabNeeds.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.preferenceTabHealth.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.preferenceTabGuestAndPrisoner.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.preferenceTabTraining.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.preferenceTabGrowing.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.preferenceTabBills.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.preferenceTabArt.PreferenceEnableTabReplacement = this.preferenceEnableTabReplacement;
			this.tabBills = new ITab_Bills_Alternate();
			this.tabGrowing = new ITab_Growing_Alternate();
			this.tabArt = new ITab_Art_Alternate();
			this.tabCharacter = new ITab_Pawn_Character_Alternate(this.preferenceTabBrowseButtons);
			this.tabCharacterVanilla = new ITab_Pawn_Character_Vanilla(this.preferenceTabBrowseButtons);
			this.tabGear = new ITab_Pawn_Gear_Alternate(this.preferenceTabBrowseButtons);
			this.tabGearVanilla = new ITab_Pawn_Gear_Vanilla(this.preferenceTabBrowseButtons);
			this.tabNeeds = new ITab_Pawn_Needs_Alternate(this.preferenceTabBrowseButtons);
			this.tabNeedsVanilla = new ITab_Pawn_Needs_Vanilla(this.preferenceTabBrowseButtons);
			this.tabHealth = new ITab_Pawn_Health_Alternate(this.preferenceTabBrowseButtons);
			this.tabHealthVanilla = new ITab_Pawn_Health_Vanilla(this.preferenceTabBrowseButtons);
			this.tabTraining = new ITab_Pawn_Training_Alternate(this.preferenceTabBrowseButtons);
			this.tabTrainingVanilla = new ITab_Pawn_Training_Vanilla(this.preferenceTabBrowseButtons);
			this.tabGuest = new ITab_Pawn_Guest_Alternate(this.preferenceTabBrowseButtons);
			this.tabGuestVanilla = new ITab_Pawn_Guest_Vanilla(this.preferenceTabBrowseButtons);
			this.tabPrisoner = new ITab_Pawn_Prisoner_Alternate(this.preferenceTabBrowseButtons);
			this.tabPrisonerVanilla = new ITab_Pawn_Prisoner_Vanilla(this.preferenceTabBrowseButtons);
		}

		public void ResetTextures()
		{
			BrowseButtonDrawer.ResetTextures();
			ITab_Pawn_Health_Alternate.ResetTextures();
			MedicalCareUtility.Reset();
			BillDrawer.ResetTextures();
			TabDrawer.ResetTextures();
		}

		public void PrepareDependencies(UserInterface userInterface)
		{
			this.preferenceEnableTabReplacement.ValueChanged += delegate(bool value)
			{
				this.dirtyFlag = true;
			};
			this.preferenceTabBrowseButtons.ValueChanged += delegate(bool value)
			{
				this.dirtyFlag = true;
			};
			this.preferenceTabCharacter.ValueChanged += delegate(bool value)
			{
				this.dirtyFlag = true;
			};
			this.preferenceTabGear.ValueChanged += delegate(bool value)
			{
				this.dirtyFlag = true;
			};
			this.preferenceTabNeeds.ValueChanged += delegate(bool value)
			{
				this.dirtyFlag = true;
			};
			this.preferenceTabHealth.ValueChanged += delegate(bool value)
			{
				this.dirtyFlag = true;
			};
			this.preferenceTabGuestAndPrisoner.ValueChanged += delegate(bool value)
			{
				this.dirtyFlag = true;
			};
			this.preferenceTabTraining.ValueChanged += delegate(bool value)
			{
				this.dirtyFlag = true;
			};
			this.preferenceTabGrowing.ValueChanged += delegate(bool value)
			{
				this.dirtyFlag = true;
			};
			this.preferenceTabBills.ValueChanged += delegate(bool value)
			{
				this.dirtyFlag = true;
			};
			this.preferenceTabArt.ValueChanged += delegate(bool value)
			{
				this.dirtyFlag = true;
			};
		}

		public void Initialize(UserInterface userInterface)
		{
			MainTabWindow_Inspect mainTabWindow_Inspect = userInterface.FindMainTabOfType<MainTabWindow_Inspect>();
			if (mainTabWindow_Inspect != null)
			{
				this.ResetReplacementTabs(mainTabWindow_Inspect);
			}
		}

		public void Update()
		{
			if (this.dirtyFlag)
			{
				UserInterface userInterface = Find.UIRoot_Map as UserInterface;
				if (userInterface != null)
				{
					MainTabWindow_Inspect mainTabWindow_Inspect = userInterface.FindMainTabOfType<MainTabWindow_Inspect>();
					if (mainTabWindow_Inspect != null)
					{
						this.ResetReplacementTabs(mainTabWindow_Inspect);
					}
				}
				this.dirtyFlag = false;
			}
		}

		public void ResetReplacementTabs(MainTabWindow_Inspect window)
		{
			ReplacementTabs replacementTabs = this.CreateReplacementTabs();
			if (!replacementTabs.Empty)
			{
				window.ReplacementTabs = this.CreateReplacementTabs();
			}
			else
			{
				window.ReplacementTabs = null;
			}
		}

		public ReplacementTabs CreateReplacementTabs()
		{
			this.replacementTabs.Clear();
			Dictionary<Type, ITab> dictionary = new Dictionary<Type, ITab>();
			if (this.preferenceTabCharacter.Value)
			{
				dictionary[typeof(ITab_Pawn_Character)] = this.tabCharacter;
			}
			else if (this.preferenceTabBrowseButtons.Value)
			{
				dictionary[typeof(ITab_Pawn_Character)] = this.tabCharacterVanilla;
			}
			if (this.preferenceTabGear.Value)
			{
				dictionary[typeof(ITab_Pawn_Gear)] = this.tabGear;
			}
			else if (this.preferenceTabBrowseButtons.Value)
			{
				dictionary[typeof(ITab_Pawn_Gear)] = this.tabGearVanilla;
			}
			if (this.preferenceTabNeeds.Value)
			{
				dictionary[typeof(ITab_Pawn_Needs)] = this.tabNeeds;
			}
			else if (this.preferenceTabBrowseButtons.Value)
			{
				dictionary[typeof(ITab_Pawn_Needs)] = this.tabNeedsVanilla;
			}
			if (this.preferenceTabHealth.Value)
			{
				dictionary[typeof(ITab_Pawn_Health)] = this.tabHealth;
			}
			else if (this.preferenceTabBrowseButtons.Value)
			{
				dictionary[typeof(ITab_Pawn_Health)] = this.tabHealthVanilla;
			}
			if (this.preferenceTabGuestAndPrisoner.Value)
			{
				dictionary[typeof(ITab_Pawn_Guest)] = this.tabGuest;
				dictionary[typeof(ITab_Pawn_Prisoner)] = this.tabPrisoner;
			}
			else if (this.preferenceTabBrowseButtons.Value)
			{
				dictionary[typeof(ITab_Pawn_Guest)] = this.tabGuestVanilla;
				dictionary[typeof(ITab_Pawn_Prisoner)] = this.tabPrisonerVanilla;
			}
			if (this.preferenceTabTraining.Value)
			{
				dictionary[typeof(ITab_Pawn_Training)] = this.tabTraining;
			}
			else if (this.preferenceTabBrowseButtons.Value)
			{
				dictionary[typeof(ITab_Pawn_Training)] = this.tabTrainingVanilla;
			}
			if (this.preferenceTabBills.Value)
			{
				dictionary[typeof(ITab_Bills)] = this.tabBills;
			}
			if (this.preferenceTabArt.Value)
			{
				dictionary[typeof(ITab_Art)] = this.tabArt;
			}
			foreach (ThingDef current in DefDatabase<ThingDef>.AllDefs)
			{
				if (current.inspectorTabsResolved != null)
				{
					bool flag = false;
					foreach (ITab current2 in current.inspectorTabsResolved)
					{
						if (dictionary.ContainsKey(current2.GetType()))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						List<ITab> list = new List<ITab>();
						foreach (ITab current3 in current.inspectorTabsResolved)
						{
							ITab item;
							if (dictionary.TryGetValue(current3.GetType(), out item))
							{
								list.Add(item);
							}
							else
							{
								list.Add(current3);
							}
						}
						this.replacementTabs.AddThingDef(current, list);
					}
				}
			}
			if (this.preferenceTabGrowing.Value)
			{
				this.replacementTabs.AddZoneType(typeof(ITab_Growing), this.tabGrowing);
			}
			return this.replacementTabs;
		}
	}
}

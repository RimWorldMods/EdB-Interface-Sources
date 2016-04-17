using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Verse;

namespace EdB.Interface
{
	public class Preferences
	{
		private static Preferences instance;

		protected List<PreferenceGroup> groups = new List<PreferenceGroup>();

		protected Dictionary<string, PreferenceGroup> groupDictionary = new Dictionary<string, PreferenceGroup>();

		protected Dictionary<string, IPreference> preferenceDictionary = new Dictionary<string, IPreference>();

		protected List<PreferenceGroup> miscellaneousGroup = new List<PreferenceGroup>();

		protected bool atLeastOne;

		public static Preferences Instance
		{
			get
			{
				if (Preferences.instance == null)
				{
					Preferences.instance = new Preferences();
				}
				return Preferences.instance;
			}
		}

		public IEnumerable<PreferenceGroup> Groups
		{
			get
			{
				if (this.miscellaneousGroup[0].PreferenceCount > 0)
				{
					return this.groups.Concat(this.miscellaneousGroup);
				}
				return this.groups;
			}
		}

		public bool AtLeastOne
		{
			get
			{
				return this.atLeastOne;
			}
		}

		protected string FilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "EdBInterface.xml");
			}
		}

		public Preferences()
		{
			this.Reset();
		}

		public void Reset()
		{
			this.groups.Clear();
			this.groupDictionary.Clear();
			this.preferenceDictionary.Clear();
			this.miscellaneousGroup.Clear();
			this.miscellaneousGroup.Add(new PreferenceGroup("EdB.InterfaceOptions.Prefs.Miscellaneous"));
			this.atLeastOne = false;
		}

		public void Add(IPreference preference)
		{
			if (this.preferenceDictionary.ContainsKey(preference.Name))
			{
				Log.Warning("Preference already added to EdB.Interface.Preferences: " + preference.Name);
				return;
			}
			string group = preference.Group;
			PreferenceGroup preferenceGroup;
			if (group == null)
			{
				preferenceGroup = this.miscellaneousGroup[0];
			}
			else if (this.groupDictionary.ContainsKey(group))
			{
				preferenceGroup = this.groupDictionary[group];
			}
			else
			{
				preferenceGroup = new PreferenceGroup(group);
				this.groups.Add(preferenceGroup);
				this.groupDictionary.Add(group, preferenceGroup);
			}
			preferenceGroup.Add(preference);
			this.preferenceDictionary.Add(preference.Name, preference);
			this.atLeastOne = true;
		}

		public void Save()
		{
			try
			{
				XDocument xDocument = new XDocument();
				XElement xElement = new XElement("Preferences");
				xDocument.Add(xElement);
				foreach (PreferenceGroup current in this.Groups)
				{
					foreach (IPreference current2 in current.Preferences)
					{
						if (!string.IsNullOrEmpty(current2.ValueForSerialization))
						{
							XElement content = new XElement(current2.Name, current2.ValueForSerialization);
							xElement.Add(content);
						}
					}
				}
				xDocument.Save(this.FilePath);
			}
			catch (Exception arg)
			{
				Log.Warning("Exception saving EdB Interface preferences: " + arg);
			}
		}

		public void Load()
		{
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				try
				{
					xmlDocument.LoadXml(File.ReadAllText(this.FilePath));
				}
				catch (FileNotFoundException)
				{
					return;
				}
				IEnumerator enumerator = xmlDocument.ChildNodes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.Current;
						if (current is XmlElement)
						{
							XmlElement xmlElement = current as XmlElement;
							if ("Preferences".Equals(xmlElement.Name))
							{
								IEnumerator enumerator2 = xmlElement.ChildNodes.GetEnumerator();
								try
								{
									while (enumerator2.MoveNext())
									{
										object current2 = enumerator2.Current;
										if (current2 is XmlElement)
										{
											XmlElement xmlElement2 = current2 as XmlElement;
											string name = xmlElement2.Name;
											if (this.preferenceDictionary.ContainsKey(name))
											{
												IPreference preference = this.preferenceDictionary[name];
												preference.ValueForSerialization = xmlElement2.InnerText;
											}
											else
											{
												Log.Warning("Unrecognized EdB Interface preference: " + name);
											}
										}
									}
								}
								finally
								{
									IDisposable disposable;
									if ((disposable = (enumerator2 as IDisposable)) != null)
									{
										disposable.Dispose();
									}
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
			}
			catch (Exception arg)
			{
				Log.Warning("Exception loading EdB Interface preferences: " + arg);
			}
		}
	}
}

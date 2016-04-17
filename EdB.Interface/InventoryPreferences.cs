using System;

namespace EdB.Interface
{
	public class InventoryPreferences
	{
		private PreferenceIncludeUnfinished includeUnfinished;

		private PreferenceCompressedStorage compressedStorage;

		public PreferenceIncludeUnfinished IncludeUnfinished
		{
			get
			{
				return this.includeUnfinished;
			}
			set
			{
				this.includeUnfinished = value;
			}
		}

		public PreferenceCompressedStorage CompressedStorage
		{
			get
			{
				return this.compressedStorage;
			}
			set
			{
				this.compressedStorage = value;
			}
		}
	}
}

﻿// Concept Matrix 3.
// Licensed under the MIT license.

namespace ConceptMatrix.EquipmentModule.Views
{
	using System.ComponentModel;
	using System.Windows.Media.Media3D;
	using ConceptMatrix;
	using ConceptMatrix.Services;

	public abstract class EquipmentBaseViewModel : INotifyPropertyChanged
	{
		public static readonly DummyNoneItem NoneItem = new DummyNoneItem();
		public static readonly DummyNoneDye NoneDye = new DummyNoneDye();

		protected IGameDataService gameData;

		protected ushort modelBase;
		protected ushort modelSet;
		protected ushort modelVariant;
		protected byte dyeId;

		private IItem item;
		private IDye dye;
		private Selection target;

		public EquipmentBaseViewModel(ItemSlots slot, Selection selection)
		{
			this.target = selection;
			this.gameData = Module.Services.Get<IGameDataService>();
			this.Slot = slot;
		}

		public delegate void ChangedHandler();

		public event PropertyChangedEventHandler PropertyChanged;
		public event ChangedHandler Changed;

		public ItemSlots Slot
		{
			get;
			private set;
		}

		public IItem Item
		{
			get
			{
				return this.item;
			}

			set
			{
				IItem oldItem = this.item;
				this.item = value;

				if (value != null)
				{
					this.modelSet = value.WeaponSet;
					this.modelBase = value.ModelBase;
					this.modelVariant = value.ModelVariant;
				}
				else
				{
					this.modelSet = 0;
					this.modelBase = 0;
					this.modelVariant = 0;
				}

				if (oldItem != null && oldItem != this.item)
				{
					this.Apply();
					this.target.ActorRefresh();
				}
			}
		}

		public IDye Dye
		{
			get
			{
				return this.dye;
			}

			set
			{
				IDye oldDye = this.dye;
				this.dye = value;
				this.dyeId = this.dye != null ? value.Id : (byte)0;

				if (oldDye != null && oldDye != this.dye)
				{
					this.Apply();
					this.target.ActorRefresh();
				}
			}
		}

		public bool CanDye
		{
			get;
			set;
		}

		public byte DyeId
		{
			get
			{
				return this.dyeId;
			}
			set
			{
				this.dyeId = value;
				this.Dye = this.GetDye();
			}
		}

		public Color Color
		{
			get;
			set;
		}

		public bool CanColor
		{
			get;
			set;
		}

		public Vector3D Scale
		{
			get;
			set;
		}

		public bool CanScale
		{
			get;
			set;
		}

		public bool HasModelSet
		{
			get;
			set;
		}

		public string SlotName
		{
			get
			{
				return this.Slot.ToDisplayName();
			}
		}

		public string Key
		{
			get
			{
				return this.Item?.Key.ToString();
			}
			set
			{
				int val = int.Parse(value);
				this.Item = this.gameData.Items.Get(val);
			}
		}

		public ushort ModelBase
		{
			get
			{
				return this.modelBase;
			}
			set
			{
				this.modelBase = value;
				this.Item = this.GetItem();
			}
		}

		public ushort ModelVariant
		{
			get
			{
				return this.modelVariant;
			}

			set
			{
				this.modelVariant = value;
				this.Item = this.GetItem();
			}
		}

		public ushort ModelSet
		{
			get
			{
				return this.modelSet;
			}
			set
			{
				this.modelSet = value;
				this.Item = this.GetItem();
			}
		}

		protected abstract void Apply();

		protected IItem GetItem()
		{
			if (this.ModelBase == 0 && this.modelVariant == 0 && this.modelSet == 0)
				return NoneItem;

			foreach (IItem tItem in this.gameData.Items.All)
			{
				if (!tItem.FitsInSlot(this.Slot))
					continue;

				// Big old hack, but we prefer the emperors bracelets to the promise bracelets (even though they are the same model)
				if (this.Slot == ItemSlots.Wrists && tItem.Name.StartsWith("Promise of"))
					continue;

				if (this.HasModelSet && tItem.WeaponSet != this.ModelSet)
					continue;

				if (tItem.ModelBase == this.ModelBase && tItem.ModelVariant == this.ModelVariant)
				{
					return tItem;
				}
			}

			return new DummyItem(this.ModelSet, this.ModelBase, this.ModelVariant);
		}

		protected IDye GetDye()
		{
			// None
			if (this.DyeId == 0)
				return NoneDye;

			foreach (IDye dye in this.gameData.Dyes.All)
			{
				if (dye.Id == this.DyeId)
				{
					return dye;
				}
			}

			return NoneDye;
		}

		public class DummyNoneItem : IItem
		{
			public string Name
			{
				get
				{
					return "None";
				}
			}

			public string Description
			{
				get
				{
					return null;
				}
			}

			public IImage Icon
			{
				get
				{
					return null;
				}
			}

			public ushort ModelBase
			{
				get
				{
					return 0;
				}
			}

			public ushort ModelVariant
			{
				get
				{
					return 0;
				}
			}

			public ushort WeaponSet
			{
				get
				{
					return 0;
				}
			}

			public int Key
			{
				get
				{
					return 0;
				}
			}

			public bool FitsInSlot(ItemSlots slot)
			{
				return true;
			}
		}

		public class DummyNoneDye : IDye
		{
			public byte Id
			{
				get
				{
					return 0;
				}
			}

			public string Name
			{
				get
				{
					return "None";
				}
			}

			public string Description { get; }
			public IImage Icon { get; }

			public int Key
			{
				get
				{
					return 0;
				}
			}
		}

		public class DummyItem : IItem
		{
			public DummyItem(ushort modelSet, ushort modelBase, ushort modelVariant)
			{
				this.WeaponSet = modelSet;
				this.ModelBase = modelBase;
				this.ModelVariant = modelVariant;
			}

			public string Name
			{
				get
				{
					return "Unknown";
				}
			}

			public string Description
			{
				get
				{
					return null;
				}
			}

			public IImage Icon
			{
				get
				{
					return null;
				}
			}

			public ushort ModelBase
			{
				get;
				private set;
			}

			public ushort ModelVariant
			{
				get;
				private set;
			}

			public ushort WeaponSet
			{
				get;
				private set;
			}

			public int Key
			{
				get
				{
					return 0;
				}
			}

			public bool FitsInSlot(ItemSlots slot)
			{
				return true;
			}
		}
	}
}
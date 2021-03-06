﻿// Concept Matrix 3.
// Licensed under the MIT license.

namespace ConceptMatrix.AppearanceModule.Pages
{
	using System.Windows;
	using System.Windows.Controls;
	using ConceptMatrix.AppearanceModule.Dialogs;
	using ConceptMatrix.AppearanceModule.Files;

	/// <summary>
	/// Interaction logic for AppearancePage.xaml.
	/// </summary>
	public partial class AppearancePage : UserControl
	{
		private IBaseMemoryOffset baseOffset;
		private AppearanceFile refreshCacheFile;

		public AppearancePage()
		{
			this.InitializeComponent();

			ISelectionService selectionService = Services.Get<ISelectionService>();
			selectionService.SelectionChanged += this.OnSelectionChanged;

			IActorRefreshService refreshService = Services.Get<IActorRefreshService>();
			refreshService.OnRefreshStarting += this.RefreshService_OnRefreshStarting;
			refreshService.OnRefreshComplete += this.RefreshService_OnRefreshComplete;

			this.OnSelectionChanged(selectionService.CurrentSelection);
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			ISelectionService selectionService = Services.Get<ISelectionService>();
			selectionService.SelectionChanged -= this.OnSelectionChanged;
		}

		private async void OnLoadClicked(object sender, RoutedEventArgs e)
		{
			IFileService fileService = Services.Get<IFileService>();
			IViewService viewService = Services.Get<IViewService>();

			FileBase file = await fileService.OpenAny(
				LegacyEquipmentSetFile.FileType,
				LegacyAppearanceFile.AllFileType,
				AppearanceFile.FileType);

			if (file is LegacyAppearanceFile legacyAllFile)
				file = legacyAllFile.Upgrade();

			if (file is LegacyEquipmentSetFile legacyEquipmentFile)
				file = legacyEquipmentFile.Upgrade();

			if (file is AppearanceFile apFile)
			{
				AppearanceFile.SaveModes mode = await viewService.ShowDialog<AppearanceModeSelectorDialog, AppearanceFile.SaveModes>("Load Appearance...");

				if (mode == AppearanceFile.SaveModes.None)
					return;

				apFile.Write(this.Appearance, this.Equipment, mode);
			}
		}

		private async void OnSaveClicked(object sender, RoutedEventArgs e)
		{
			IViewService viewService = Services.Get<IViewService>();
			AppearanceFile.SaveModes mode = await viewService.ShowDialog<AppearanceModeSelectorDialog, AppearanceFile.SaveModes>("Save Appearance...");

			if (mode == AppearanceFile.SaveModes.None)
				return;

			IFileService fileService = Services.Get<IFileService>();
			AppearanceFile file = new AppearanceFile();
			file.Read(this.Appearance, this.Equipment, mode);
			await fileService.Save(file);
		}

		private void OnSelectionChanged(Selection selection)
		{
			bool hasValidSelection = selection != null && (selection.Type == ActorTypes.Player || selection.Type == ActorTypes.BattleNpc || selection.Type == ActorTypes.EventNpc);

			if (hasValidSelection)
				this.baseOffset = selection.BaseAddress;

			Application.Current.Dispatcher.Invoke(() =>
			{
				this.IsEnabled = hasValidSelection;
			});
		}

		private void RefreshService_OnRefreshStarting(IBaseMemoryOffset baseOffset)
		{
			if (this.baseOffset != baseOffset)
				return;

			this.refreshCacheFile = new AppearanceFile();
			this.refreshCacheFile.Read(this.Appearance, this.Equipment, AppearanceFile.SaveModes.All);
		}

		private void RefreshService_OnRefreshComplete(IBaseMemoryOffset baseOffset)
		{
			if (this.refreshCacheFile != null)
			{
				this.refreshCacheFile.Write(this.Appearance, this.Equipment, AppearanceFile.SaveModes.All);
				this.refreshCacheFile = null;
			}
		}
	}
}

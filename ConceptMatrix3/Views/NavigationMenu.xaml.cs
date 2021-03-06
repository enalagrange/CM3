﻿// Concept Matrix 3.
// Licensed under the MIT license.

namespace ConceptMatrix.GUI.Views
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Windows.Controls;
	using System.Windows.Data;
	using ConceptMatrix.GUI.Services;

	/// <summary>
	/// Interaction logic for NavigationMenu.xaml.
	/// </summary>
	public partial class NavigationMenu : UserControl
	{
		private ViewService viewService;

		public NavigationMenu()
		{
			this.InitializeComponent();

			if (DesignerProperties.GetIsInDesignMode(this))
				return;

			ICollectionView view = CollectionViewSource.GetDefaultView(this.Items);
			view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(NavigationItem.Category)));
			view.SortDescriptions.Add(new SortDescription(nameof(NavigationItem.Sort), ListSortDirection.Ascending));
			view.SortDescriptions.Add(new SortDescription(nameof(NavigationItem.Category), ListSortDirection.Ascending));
			view.SortDescriptions.Add(new SortDescription(nameof(NavigationItem.Name), ListSortDirection.Ascending));

			this.ViewList.ItemsSource = view;

			this.viewService = App.Services.Get<ViewService>();
			this.viewService.AddingPage += this.OnAddPage;

			foreach (ViewService.Page page in this.viewService.Pages)
			{
				this.OnAddPage(page);
			}
		}

		public ObservableCollection<NavigationItem> Items { get; set; } = new ObservableCollection<NavigationItem>();

		private void OnAddPage(ViewService.Page page)
		{
			this.Items.Add(new NavigationItem(page.Path));
		}

		private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			NavigationItem item = (NavigationItem)this.ViewList.SelectedItem;
			this.viewService.ShowPage(item.Path);
		}

		public class NavigationItem : INotifyPropertyChanged
		{
			public NavigationItem(string path)
			{
				this.Path = path;

				string[] parts = path.Split('/', '\\');
				if (parts.Length == 2)
				{
					this.Category = parts[0];
					this.Name = parts[1];
				}
				else
				{
					this.Category = null;
					this.Name = path;
				}

				// Bit of a hack. These should come in with the path
				if (this.Category == "Advanced")
				{
					this.Sort = 99;
				}
			}

			#pragma warning disable CS0067
			public event PropertyChangedEventHandler PropertyChanged;

			public int Sort { get; set; }
			public string Name { get; set; }
			public string Category { get; set; }
			public string Path { get; set; }
		}
	}
}

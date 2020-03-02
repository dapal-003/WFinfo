using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WFInfo {
	/// <summary>
	/// Interaction logic for Inventory.xaml
	/// </summary>
	public partial class Inventory : Window {

        private bool searchActive = false;
        private bool showAllItems = false;
        public static List<TreeNode> InventoryNodes { get; set; }
        public static string[] searchText;

        private void WindowLoaded(object sender, RoutedEventArgs e) { // triggers when the window is first loaded, populates all the listviews once.

        }

        private void VaultedClick(object sender, RoutedEventArgs e) {
            if ((bool)vaulted.IsChecked) {
                foreach (TreeNode era in InventoryNodes)
                    era.FilterOutVaulted(true);

                RefreshVisibleeItems();
            } else
                ReapplyFilters();
        }

        private void RefreshVisibleeItems() {
            int index = 0;
            if (showAllItems) {
                List<TreeNode> activeNodes = new List<TreeNode>();
                foreach (TreeNode item in InventoryNodes)
                        activeNodes.Add(item);


                for (index = 0; index < InventoryTree.Items.Count;) {
                    TreeNode item = (TreeNode)InventoryTree.Items.GetItemAt(index);
                    if (!activeNodes.Contains(item))
                        InventoryTree.Items.RemoveAt(index);
                    else {
                        activeNodes.Remove(item);
                        index++;
                    }
                }

                foreach (TreeNode relic in activeNodes)
                    InventoryTree.Items.Add(relic);

                SortBoxChanged(null, null);
            } else {
                foreach (TreeNode item in InventoryNodes) {
                    int curr = InventoryTree.Items.IndexOf(item);
                    if (item.ChildrenFiltered.Count == 0) {
                        if (curr != -1)
                            InventoryTree.Items.RemoveAt(curr);
                    } else {
                        if (curr == -1)
                            InventoryTree.Items.Insert(index, item);

                        index++;
                    }
                    item.RecolorChildren();
                }
            }
            InventoryTree.Items.Refresh();
        }

        private void ReapplyFilters() {

            foreach (TreeNode item in InventoryNodes)
                item.ResetFilter();

            if ((bool)vaulted.IsChecked)
                foreach (TreeNode item in InventoryNodes)
                    item.FilterOutVaulted(true);

            if (searchText != null && searchText.Length != 0)
                foreach (TreeNode item in InventoryNodes)
                    item.FilterSearchText(searchText, false, true);

            RefreshVisibleeItems();
        }


        private void TextboxTextChanged(object sender, TextChangedEventArgs e) {
            searchActive = textBox.Text.Length > 0 && textBox.Text != "Filter Terms";
            if (textBox.IsLoaded) {
                if (searchActive || (searchText != null && searchText.Length > 0)) {
                    if (searchActive)
                        searchText = textBox.Text.Split(' ');
                    else
                        searchText = null;
                    ReapplyFilters();
                }
            }
        }

        private void SortBoxChanged(object sender, SelectionChangedEventArgs e) {
            // 0 - Name
            // 1 - Plat
            // 2 - Ducats
            // 3 - Ducats to plat

            if (IsLoaded) {
                foreach (TreeNode era in InventoryNodes) {
                    era.Sort(SortBox.SelectedIndex);
                    era.RecolorChildren();
                }
                if (showAllItems) {
                    InventoryTree.Items.SortDescriptions.Clear();
                    InventoryTree.Items.IsLiveSorting = true;
                    switch (SortBox.SelectedIndex) {
                        case 1:
                        InventoryTree.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Plat_Val", System.ComponentModel.ListSortDirection.Descending));
                        break;
                        case 2:
                        InventoryTree.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Ducat_Val", System.ComponentModel.ListSortDirection.Descending));
                        break;
                        case 3:
                        InventoryTree.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Ratio_Val", System.ComponentModel.ListSortDirection.Descending));
                        break;
                        default:
                        InventoryTree.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name_Sort", System.ComponentModel.ListSortDirection.Ascending));
                        break;
                    }
                    bool i = false;
                    foreach (TreeNode relic in InventoryTree.Items) {
                        i = !i;
                        if (i)
                            relic.Background_Color = TreeNode.BACK_D_BRUSH;
                        else
                            relic.Background_Color = TreeNode.BACK_U_BRUSH;
                    }
                }
            }
        }

        private void TextBoxFocus(object sender, RoutedEventArgs e) {
            if (!searchActive)
                textBox.Clear();
        }

        private void TextBoxLostFocus(object sender, RoutedEventArgs e) {
            if (!searchActive)
                textBox.Text = "Filter Terms";
        }

        #region Boring basic window stuff
        // Allows the draging of the window
        private new void MouseDown(object sender, MouseButtonEventArgs e) {
			if (e.ChangedButton == MouseButton.Left)
				DragMove();
		}
		private void Hide(object sender, RoutedEventArgs e) {
			Hide();
		}
		public Inventory() {
			InitializeComponent();
		}
		#endregion
	}
}

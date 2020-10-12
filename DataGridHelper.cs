using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DeweyDecimalTrainer
{
    public class DataGridHelper
    {

        public static readonly DependencyProperty EnableRowsMoveProperty =
            DependencyProperty.RegisterAttached("EnableRowsMove", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(false, EnableRowsMoveChanged));

        private static readonly DependencyProperty DraggedItemProperty =
            DependencyProperty.RegisterAttached("DraggedItem", typeof(object), typeof(DataGridHelper), new PropertyMetadata(null));

        public static bool GetEnableRowsMove(DataGrid obj)
        {
            return (bool)obj.GetValue(EnableRowsMoveProperty);
        }

        public static void SetEnableRowsMove(DataGrid obj, bool value)
        {
            obj.SetValue(EnableRowsMoveProperty, value);
        }

        private static object GetDraggedItem(DependencyObject obj)
        {
            return (object)obj.GetValue(DraggedItemProperty);
        }

        private static void SetDraggedItem(DependencyObject obj, object value)
        {
            obj.SetValue(DraggedItemProperty, value);
        }

        private static void EnableRowsMoveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = (d as DataGrid);
            if (grid == null) return;
            if ((bool)e.NewValue)
            {
                grid.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
                grid.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
                grid.PreviewMouseMove += OnMouseMove;
            }
            else
            {
                grid.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
                grid.PreviewMouseLeftButtonUp -= OnMouseLeftButtonUp;
                grid.PreviewMouseMove -= OnMouseMove;
            }
        }

        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var row = TryFindFromPoint<DataGridRow>((UIElement)sender, e.GetPosition((sender as DataGrid)));
            if (row == null || row.IsEditing) return;
            DataGridHelper.SetDraggedItem(sender as DataGrid, row.Item);
        }

        private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var draggeditem = DataGridHelper.GetDraggedItem(sender as DependencyObject);
            if (draggeditem == null) return;
            ExchangeItems(sender, (sender as DataGrid).SelectedItem);

            (sender as DataGrid).SelectedItem = draggeditem;

            DataGridHelper.SetDraggedItem(sender as DataGrid, null);
        }

        private static void OnMouseMove(object sender, MouseEventArgs e)
        {
            var draggeditem = DataGridHelper.GetDraggedItem(sender as DependencyObject);
            if (draggeditem == null) return;
            var row = TryFindFromPoint<DataGridRow>((UIElement)sender, e.GetPosition((sender as DataGrid)));
            if (row == null || row.IsEditing) return;
            ExchangeItems(sender, row.Item);
        }

        private static void ExchangeItems(object sender, object targetItem)
        {
            var draggeditem = DataGridHelper.GetDraggedItem(sender as DependencyObject);
            if (draggeditem == null) return;
            if (targetItem != null && !ReferenceEquals(draggeditem, targetItem))
            {
                var list = (sender as DataGrid).ItemsSource as IList;

                var targetIndex = list.IndexOf(targetItem);

                list.Remove(draggeditem);

                list.Insert(targetIndex, draggeditem);
            }
        }

        public static T FindVisualParent<T>(DependencyObject child)
          where T : DependencyObject
        {

            var parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindVisualParent<T>(parentObject);
            }
        }

        public static T TryFindFromPoint<T>(UIElement reference, Point point)
          where T : DependencyObject
        {
            var element = reference.InputHitTest(point) as DependencyObject;
            if (element == null) return null;
            if (element is T) return (T)element;
            return FindVisualParent<T>(element);
        }

    }
}

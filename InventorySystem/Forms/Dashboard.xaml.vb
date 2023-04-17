Imports InventorySystem.InventorySystem.DataSets.ProductsDataSet
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSetTableAdapters

Public Class Dashboard

    Private Sub Dashboard_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim dataTable As New salesDataTable
        Dim productAdapter As New salesTableAdapter
        productAdapter.FillBySales(dataTable)
        SalesDataGridView.ItemsSource = dataTable
        Debug.Write(SalesDataGridView.Columns.Count)
        If SalesDataGridView.Columns.Count > 1 Then
            SalesDataGridView.Columns(0).Visibility = Visibility.Collapsed
            SalesDataGridView.Columns(1).Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Sub ChangePanelEvents(sender As Object, e As EventArgs) Handles SalesButton.Click, StocksButton.Click,
            ItemsButton.Click, InvoicesButton.Click
        Dim panels() As Grid = {SalesPanel, StocksPanel, ItemsPanel}
        ' Hide all the panels before setting the visibility of the requested panel
        For Each panel As Grid In panels
            If panel.IsVisible Then
                panel.Visibility = Visibility.Collapsed
            End If
        Next
        If sender.Equals(SalesButton) Then
            SalesPanel.Visibility = Visibility.Visible
        ElseIf sender.Equals(StocksButton) Then
            StocksPanel.Visibility = Visibility.Visible
        ElseIf sender.Equals(ItemsButton) Then
            ItemsPanel.Visibility = Visibility.Visible
        End If
    End Sub

    Private Sub ClickEvents(sender As Object, e As RoutedEventArgs) Handles CloseButton.Click, RestoreDButton.Click,
        MinimizeButton.Click
        If sender.Equals(CloseButton) Then
            Me.Close()                                                                          ' Close the window
        ElseIf sender.Equals(RestoreDButton) Then
            Dim rectArea As Rect = SystemParameters.WorkArea
            ' Is the window in maximized state?
            If Me.Width = rectArea.Width AndAlso Me.Height = rectArea.Height Then
                ' If so then go back to the initial height and width
                Me.Width = Me.MinWidth
                Me.Height = Me.MinHeight
                RestoreDIcon.Source = TryCast(FindResource("ic_maximizebit"), ImageSource)      ' Change the icon of restore down
            Else
                ' Position the window at the leftmost and topmost
                Me.Left = 0
                Me.Top = 0
                Me.Width = rectArea.Width
                Me.Height = rectArea.Height

                RestoreDIcon.Source = TryCast(FindResource("ic_restoredownbit"), ImageSource)   ' Change the icon of the restore down
            End If
        ElseIf sender.Equals(MinimizeButton) Then
            Me.WindowState = WindowState.Minimized                                              ' Minimize the window
        End If
    End Sub

    Private Sub Dashboard_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseLeftButtonDown
        DragMove()
    End Sub


End Class

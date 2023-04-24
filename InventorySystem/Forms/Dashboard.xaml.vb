Imports System.Data
Imports System.Windows.Controls.Primitives
Imports HandyControl.Controls
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSet
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSetTableAdapters

Public Class Dashboard
    Private salesTable As New salesDataTable
    Private salesAdapter As New salesTableAdapter
    Private stocksTable As New productDataTable
    Private stocksAdapter As New productTableAdapter

    Private Sub Dashboard_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        salesAdapter.FillBySales(salesTable)
        stocksAdapter.FillByProducts(stocksTable)
        StocksDataGridView.ItemsSource = stocksTable                     ' Fill the stocks datagrid view with products
        SalesDataGridView.ItemsSource = salesTable                       ' Fill the sales datagrid view with orders
        FillProducts(PosProductContainer)                                ' Fill the products datagrid view

        ' Add a click handler for every product
        For i = 0 To PosProductContainer.Children.Count - 1
            PosProductContainer.Children.Item(i).AddHandler(ButtonBase.ClickEvent, New RoutedEventHandler(AddressOf AddRow))
        Next
    End Sub

    ' Triggered when we click the product cards
    Private Sub AddRow(sender As Object, e As EventArgs)
        Dim parent As Object = TryCast(sender, UserControl)
        PosDataGridView.Items.Add(New ProductDetails With {.PRODUCT_NAME = TryCast(parent.FindName("ProductName"), TextBlock).Text, .PRODUCT_PRICE = TryCast(parent.FindName("ProductPrice"), TextBlock).Text})
        PriceTotal.Text = Math.Round(Double.Parse(PriceTotal.Text) + Double.Parse(TryCast(parent.FindName("ProductPrice"), TextBlock).Text), 2)
    End Sub

    Private Sub ChangePanelEvents(sender As Object, e As EventArgs) Handles SalesButton.Click, StocksButton.Click,
            POSButton.Click, InvoicesButton.Click
        Dim panels() As Grid = {SalesPanel, StocksPanel, POSPanel}
        ' Hide all the panels before setting the visibility of the requested panel
        For Each panel As Grid In panels
            If panel.IsVisible Then
                panel.Visibility = Visibility.Collapsed
            End If
        Next

        ' Determine which button triggered the click event so we can identify which panel should we open
        If sender.Equals(SalesButton) Then
            SalesPanel.Visibility = Visibility.Visible
        ElseIf sender.Equals(StocksButton) Then
            StocksPanel.Visibility = Visibility.Visible
        ElseIf sender.Equals(POSButton) Then
            POSPanel.Visibility = Visibility.Visible
            PosProductContainer.Children.Clear()
            FillProducts(PosProductContainer)                       ' Refresh the products list because the user might have changed the list from other panel
        End If
    End Sub

    Private Sub TitleBarEvents(sender As Object, e As RoutedEventArgs) Handles SettingsButton.Click
        Dialog.Show(New SettingsDialog(Me))
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

    Private Sub SalesDataGridView_AutoGeneratingColumn(sender As Object, e As DataGridAutoGeneratingColumnEventArgs) Handles SalesDataGridView.AutoGeneratingColumn, StocksDataGridView.AutoGeneratingColumn
        ' Check if which header should we hide
        If e.Column.Header.ToString() = "id" OrElse e.Column.Header.ToString() = "order_id" OrElse e.Column.Header.ToString() = "unit_id" _
                        OrElse e.Column.Header.ToString() = "image_path" OrElse e.Column.Header.ToString() = "category_id" OrElse e.Column.Header.ToString() = "discount_percent" _
                        OrElse e.Column.Header.ToString() = "product_code" OrElse e.Column.Header.ToString() = "product_name" OrElse e.Column.Header.ToString() = "unit_in_stock" _
                        OrElse e.Column.Header.ToString() = "unit_price" Then
            e.Column.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Sub StocksDataGridView_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles StocksDataGridView.MouseLeftButtonUp
        Dim row As DataRowView = StocksDataGridView.SelectedItem
        'MsgBox(row.Row.Item(3))
        With row.Row
            ' A painful way of assigning values lol
            Dim product_id As String = .Item(0)
            Dim product_code As String = .Item(3)
            Dim product_name As String = .Item(4)
            Dim product_stock As String = .Item(5)
            Dim product_price As String = .Item(6)
            Dim product_image As String = .Item(8)
            Dialog.Show(New ProductDialog(Me, StocksDataGridView, product_id, product_code, product_name, product_stock, product_price, product_image))
        End With
    End Sub

    Private Sub ProfileButton(sender As Object, e As EventArgs) Handles AvatarButton.Click
        Dialog.Show(New ProfileDialog(Me, ))
    End Sub
End Class

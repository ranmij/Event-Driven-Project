' This is for me

Imports System.Data
Imports System.Net.NetworkInformation
Imports System.Windows.Controls.Primitives
Imports HandyControl.Controls
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSet
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSetTableAdapters

Public Class Dashboard
    Private ReadOnly salesTable As New salesDataTable
    Private ReadOnly salesAdapter As New salesTableAdapter
    Private ReadOnly stocksTable As New productDataTable
    Private ReadOnly stocksAdapter As New productTableAdapter
    Private ReadOnly orderDataTable As New ordersDataTable
    Private ReadOnly orderTableAdapter As New ordersTableAdapter

    Private Sub Dashboard_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        WeeklySalesLabel.Text = "₱ " & orderTableAdapter.ScalarQueryWeeklySales().ToString
        MonthlySalesLabel.Text = "₱ " & orderTableAdapter.ScalarQueryMonthlySales().ToString
        YearlySalesLabel.Text = "₱ " & orderTableAdapter.ScalarQueryYearlySales().ToString
        FormTitle.Text = "Dashboard"
    End Sub


    Private Sub ProductHandler(parent As WrapPanel)
        ' Add a click handler for every product
        For i = 0 To parent.Children.Count - 1
            parent.Children.Item(i).AddHandler(ButtonBase.ClickEvent, New RoutedEventHandler(AddressOf AddRow))
        Next
    End Sub

    ' Triggered when we click the product cards
    Private Sub AddRow(sender As Object, e As EventArgs)
        Dim parent As Object = TryCast(sender, UserControl)
        PosDataGridView.Items.Add(New ProductDetails With {.PRODUCT_NAME = TryCast(parent.FindName("ProductName"), TextBlock).Text, .PRODUCT_PRICE = TryCast(parent.FindName("ProductPrice"), TextBlock).Text})
        PriceTotal.Text = Math.Round(Double.Parse(PriceTotal.Text) + Double.Parse(TryCast(parent.FindName("ProductPrice"), TextBlock).Text), 2)
    End Sub

    Private Async Sub ChangePanelEvents(sender As Object, e As EventArgs) Handles SalesButton.Click, StocksButton.Click,
            POSButton.Click, DashboardButton.Click, HelpButton.Click
        Dim panels() As Grid = {SalesPanel, StocksPanel, POSPanel, HelpPanel, DashboardPanel}
        ' Hide all the panels before setting the visibility of the requested panel
        For Each panel As Grid In panels
            If panel.IsVisible Then
                panel.Visibility = Visibility.Collapsed
            End If
        Next

        ' Determine which button triggered the click event so we can identify which panel should we open
        If sender.Equals(DashboardButton) Then
            DashboardPanel.Visibility = Visibility.Visible
            WeeklySalesLabel.Text = "₱ " & orderTableAdapter.ScalarQueryWeeklySales().ToString
            MonthlySalesLabel.Text = "₱ " & orderTableAdapter.ScalarQueryMonthlySales().ToString
            YearlySalesLabel.Text = "₱ " & orderTableAdapter.ScalarQueryYearlySales().ToString
            FormTitle.Text = "Dashboard"
        ElseIf sender.Equals(SalesButton) Then
            SalesPanel.Visibility = Visibility.Visible
            SalesDataGridView.ItemsSource = orderTableAdapter.GetDataByOrders()
            FormTitle.Text = "Sales".ToUpper
        ElseIf sender.Equals(StocksButton) Then
            StocksPanel.Visibility = Visibility.Visible
            stocksAdapter.FillByProducts(stocksTable)
            StocksDataGridView.ItemsSource = stocksTable                                            ' Fill the stocks datagrid view with products
            FormTitle.Text = "Stocks".ToUpper
        ElseIf sender.Equals(POSButton) Then
            POSPanel.Visibility = Visibility.Visible
            PosProductContainer.Children.Clear()
            ' Is firebase enabled?
            If My.Settings.firebaseEnable AndAlso NetworkInterface.GetIsNetworkAvailable() Then
                If Not isExecuting Then
                    Await FirebaseFillProducts(PosProductContainer)                                 ' Refresh the products list from real time database
                End If
            Else
                FillProducts(PosProductContainer)                                                   ' Refresh the products list from local database
            End If
            ProductHandler(PosProductContainer)                                                     ' To add click handler for every card.
            FormTitle.Text = "POS"
        ElseIf sender.Equals(HelpButton) Then
            HelpPanel.Visibility = Visibility.Visible
            FormTitle.Text = "Help".ToUpper
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
        Dim columnsToHide() As String = {"id", "order_id", "order_date", "payment_id", "ship_date", "user_id", "total_price", "item_count", "product_id", "unit_id", "image_path", "category_id", "discount_percent", "product_code", "category_id", "unit_in_stock", "unit_price"}
        If columnsToHide.Contains(e.Column.Header.ToString()) Then
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
        ' TODO Change this to actual data
        Dialog.Show(New ProfileDialog(Me, "John Doe", "/Resources/intel.jpg"))
    End Sub

    Private Sub StocksAddButton_Click(sender As Object, e As RoutedEventArgs) Handles StocksAddButton.Click
        Dialog.Show(New NewProductDialog(Me))
        'Dim productData As New FireBaseProductProp With {
        '    .product_code = "1111",
        '    .image_path = "/Resources/intel.jpg",
        '    .product_name = "Intel Core i9-13900K",
        '    .unit_in_stock = 30,
        '    .unit_price = 321300
        '}
        'If My.Settings.firebaseEnable AndAlso NetworkInterface.GetIsNetworkAvailable() Then
        '    If Await FirebaseAddProductAsync(productData) Then
        '        HandyControl.Controls.MessageBox.Info("Product added successfully.")
        '    Else
        '        Dialog.Show(New ErrorDialog("Unable to add product please try again."))
        '    End If
        'Else

        'End If
    End Sub
End Class

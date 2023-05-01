Imports System.Net.NetworkInformation
Imports System.Threading
Imports HandyControl.Controls
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSet
Imports InventorySystem.InventorySystem.DataSets.ProductsDataSetTableAdapters
Imports Microsoft.Win32

Public Class NewProductDialog

    Private ReadOnly unitDataTable As New unitDataTable
    Private ReadOnly categoryDataTable As New categoryDataTable

    Private ReadOnly unitTableAdapter As New unitTableAdapter
    Private ReadOnly categoryTableAdapter As New categoryTableAdapter
    Private ReadOnly productTableAdapter As New productTableAdapter

    ' The file path of the image, this always changes every time the user pick
    Private filePath As String = Nothing

    Public Sub New(ByRef parent As Dashboard)
        InitializeComponent()

        ' We set the standard height and width of the dialog
        Me.Width = parent.MinWidth - 100
        Me.Height = parent.MinHeight - 100
    End Sub

    Private Sub AddProductImageButton_Click(sender As Object, e As RoutedEventArgs) Handles AddProductImageButton.Click
        Dim fileDialog As New OpenFileDialog
        Dim fileName As String = Nothing
        ' For the file dialog
        fileDialog.Title = "Add Image"
        fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
        fileDialog.Filter = "*.png|*.jpg"
        fileDialog.RestoreDirectory = True

        If fileDialog.ShowDialog() = True Then
            fileName = SaveImage(fileDialog.FileName)
            If fileName <> Nothing Then
                filePath = fileName
                ProductImage.Source = New BitmapImage(New Uri(filePath))
            Else
                MsgBox("An error occured")
            End If
        End If
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click
        Dim productObject As New FireBaseProductProp With {
            .product_code = ProductCodeTextBox.Text.Trim(),
            .product_name = ProductNameTextBox.Text.Trim(),
            .unit_price = ProductPriceTextBox.Text.Trim(),
            .unit_in_stock = ProductStocksTextBox.Text.Trim(),
            .category = ProductCategoryComboBox.Text.Trim(),
            .unit = ProductUnitComboBox.Text.Trim(),
            .image_path = filePath
        }
        If My.Settings.firebaseEnable AndAlso NetworkInterface.GetIsNetworkAvailable Then
            ' Add the product in real time databse
            If Await FirebaseAddProductAsync(productObject) Then
                HandyControl.Controls.MessageBox.Info("Product has been added successfully.", "Success")
            Else
                HandyControl.Controls.MessageBox.Info("Failed to add the product.", "Failed")
            End If
        Else
            Dim category_id As Integer = ProductCategoryComboBox.SelectedIndex + 1
            Dim unit_id As Integer = ProductUnitComboBox.SelectedIndex + 1
            If productTableAdapter.InsertQueryProduct(unit_id, category_id, ProductCodeTextBox.TextWrapping, ProductNameTextBox.Text, ProductStocksTextBox.Text, ProductPriceTextBox.Text, 0, filePath) <> 0 Then
                HandyControl.Controls.MessageBox.Info("Product has been added successfully.", "Success")
            Else
                HandyControl.Controls.MessageBox.Info("Failed to add the product.", "Failed")
            End If
        End If

            filePath = Nothing
    End Sub

    Private Sub NewProductDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If My.Settings.firebaseEnable AndAlso NetworkInterface.GetIsNetworkAvailable Then
            Dim controls() As Object = {ProductCategoryComboBox, ProductUnitComboBox}

            ' We pass this async methods as a parameter so that the dialog can access them
            ' Right now I don't know if this is the best practice for it but this works for now
            Dim asyncMethods() As Func(Of Task(Of List(Of String))) = {AddressOf FirebaseGetCategory, AddressOf FirebaseGetUnit}

            ' Show the async dialog and pass the controls and async methods as parameters
            Dialog.Show(New AsyncLoading(asyncMethods, controls))
        Else
            categoryTableAdapter.Fill(categoryDataTable)
            unitTableAdapter.Fill(unitDataTable)

            Dim categoriesProperty As New List(Of String)
            Dim unitProperty As New List(Of String)

            For i = 0 To categoryDataTable.Count - 1
                With categoryDataTable.Item(i)
                    categoriesProperty.Add(.Item(1).ToString.ToUpper)
                End With
            Next

            For i = 0 To unitDataTable.Count - 1
                With unitDataTable.Item(i)
                    unitProperty.Add(.Item(1).ToString.ToUpper)
                End With
            Next

            ProductCategoryComboBox.ItemsSource = categoriesProperty
            ProductUnitComboBox.ItemsSource = unitProperty

            ProductCategoryComboBox.SelectedIndex = 0
            ProductUnitComboBox.SelectedIndex = 0
        End If
    End Sub

End Class

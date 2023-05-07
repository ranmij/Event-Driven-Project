﻿Imports System.Net.NetworkInformation
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

    Private Sub SaveButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click
        Dim category_id As Integer = ProductCategoryComboBox.SelectedValue
        Dim unit_id As Integer = ProductUnitComboBox.SelectedValue
        If productTableAdapter.InsertQueryProduct(unit_id, category_id, ProductCodeTextBox.TextWrapping, ProductNameTextBox.Text, ProductStocksTextBox.Text, ProductPriceTextBox.Text, 0, filePath) <> 0 Then
            HandyControl.Controls.MessageBox.Info("Product has been added successfully.", "Success")
        Else
            HandyControl.Controls.MessageBox.Info("Failed to add the product.", "Failed")
        End If

        filePath = Nothing
    End Sub

    ' Load all the categories and units in the combo box
    Private Sub NewProductDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        categoryTableAdapter.Fill(categoryDataTable)
        unitTableAdapter.Fill(unitDataTable)

        ProductCategoryComboBox.ItemsSource = categoryDataTable
        ProductCategoryComboBox.DisplayMemberPath = "category"
        ProductCategoryComboBox.SelectedValuePath = "id"

        ProductUnitComboBox.ItemsSource = unitDataTable
        ProductUnitComboBox.DisplayMemberPath = "unit_name"
        ProductUnitComboBox.SelectedValuePath = "id"

        ProductCategoryComboBox.SelectedIndex = 0
    End Sub

End Class

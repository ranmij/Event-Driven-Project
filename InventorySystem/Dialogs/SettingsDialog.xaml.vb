﻿' This is for me

Public Class SettingsDialog
    Public Sub New(parentControl As Window)
        InitializeComponent()
        IPText.Text = My.Settings.smsIP
        Me.Height = parentControl.MinHeight - 100
        Me.Width = parentControl.MinWidth - 100
    End Sub

    Private Sub SaveSettingsButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveSettingsButton.Click
        My.Settings.smsIP = IPText.Text
        My.Settings.Save()
    End Sub
End Class

' This is for me

Public Class SettingsDialog
    Public Sub New(parentControl As Window)
        InitializeComponent()
        IsEnableFirebase.IsChecked = My.Settings.firebaseEnable
        IPText.Text = My.Settings.smsIP
        BasePathText.Text = My.Settings.firebasePath
        FirebaseKeyText.Text = My.Settings.firebaseSecret
        Me.Height = parentControl.MinHeight - 100
        Me.Width = parentControl.MinWidth - 100
    End Sub

    Private Sub SaveSettingsButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveSettingsButton.Click
        If My.Settings.firebaseEnable AndAlso Not String.IsNullOrEmpty(BasePathText.Text) AndAlso Not String.IsNullOrEmpty(FirebaseKeyText.Text) Then
            My.Settings.smsIP = IPText.Text
            My.Settings.firebaseEnable = If(IsEnableFirebase.IsChecked, True, False)
            My.Settings.Save()
        Else
            BasePathText.BorderBrush = New SolidColorBrush(Colors.Red)
            FirebaseKeyText.BorderBrush = New SolidColorBrush(Colors.Red)
        End If

    End Sub
End Class

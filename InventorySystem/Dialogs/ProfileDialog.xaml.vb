' This is for me

Imports HandyControl.Controls
Imports System.Windows.Threading

Public Class ProfileDialog
    Private _parent As Dashboard
    Public Sub New(parent As Dashboard, profile_name As String, profile_picture As String)
        InitializeComponent()
        Dim contextData As New UserData With {
            .USER_NAME = profile_name,
            .USER_PROFILE = profile_picture
        }
        Me.DataContext = contextData
        _parent = parent
        Me.Height = parent.MinHeight - 100
        Me.Width = parent.MinWidth - 100
    End Sub

    Private Sub LogOutButton_Click(sender As Object, e As RoutedEventArgs) Handles LogOutButton.Click
        My.Settings.UserID = -1
        My.Settings.Save()
        Dialog.Show(New LoadingDialog())
        Dim timer As New DispatcherTimer()                                  ' We will let the user wait for 2.5 seconds, dunno just wanna do this
        AddHandler timer.Tick, AddressOf CloseTick
        timer.Interval = New TimeSpan(0, 0, 2.5)                            ' Time interval to trigger the tick, in this case it's 2.5 seconds
        timer.Start()
    End Sub

    Private Sub CloseTick(sender As Object, e As EventArgs)
        TryCast(sender, DispatcherTimer).Stop()
        Dim loginF As New LoginForm
        loginF.Show()
        _parent.Close()
    End Sub
End Class

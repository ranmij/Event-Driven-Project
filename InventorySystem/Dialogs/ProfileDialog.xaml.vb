' This is for me

Imports HandyControl.Controls
Imports System.Data
Imports System.Windows.Threading

Public Class ProfileDialog
    Private _parent As Dashboard
    Public Sub New(ByRef parent As Dashboard)
        InitializeComponent()
        Dim userData As DataTable = GetCurrentUser()
        Dim contextData As New UserData With {
            .USER_NAME = userData.Rows(0).Item(0),
            .USER_PROFILE = "/Resources/ic_user.png"
        }
        Me.DataContext = contextData
        _parent = parent
        Me.Height = parent.MinHeight - 100
        Me.Width = parent.MinWidth - 100
    End Sub

    Private Sub LogOutButton_Click(sender As Object, e As RoutedEventArgs) Handles LogOutButton.Click
        UserLog(2)
        My.Settings.UserID = -1
        My.Settings.isAdmin = False
        My.Settings.Save()
        Dialog.Show(New AsyncLoading())
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

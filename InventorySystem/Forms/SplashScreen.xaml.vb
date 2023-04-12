Imports System.Windows.Threading

Public Class SplashScreen

    Private timer As New DispatcherTimer
    Private ReadOnly NO_USER As Integer = -1
    Public Sub New()
        InitializeComponent()
        AddHandler timer.Tick, AddressOf MySplashHandler
        timer.Interval = New TimeSpan(0, 0, 4.5)
        timer.Start()
    End Sub

    Private Sub MySplashHandler()
        timer.Stop()
        If My.Settings.UserID <> NO_USER Then
            Dim dash As New Dashboard()
            dash.Show()
        Else
            Dim login As New LoginForm()
            login.Show()
        End If
        Me.Close()
    End Sub

    Private Sub SplashScreen_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim appVersion As Version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version
        Dim format As String = versionLabel.Text
        versionLabel.Text = String.Format(versionLabel.Text, appVersion.Major, appVersion.Minor, appVersion.MinorRevision)
    End Sub
End Class
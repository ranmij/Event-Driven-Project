Imports System.Windows.Threading
Public Class SplashScreen

    Private timerDispatcher As New DispatcherTimer                                      ' A new instance of timer dispacher
    Private ReadOnly NO_USER As Integer = -1                                            ' If the user has already logged in
    Public Sub New()
        InitializeComponent()
        AddHandler timerDispatcher.Tick, AddressOf MySplashHandler                      ' Add a tick handler if the timer reaches the tick
        timerDispatcher.Interval = New TimeSpan(0, 0, 4.5)                              ' In this case the tick is triggered at 4.5 seconds
        timerDispatcher.Start()                                                         ' Start the timer
        'My.Settings.UserID = NO_USER
        'My.Settings.Save()
    End Sub

    Private Sub MySplashHandler()
        timerDispatcher.Stop()                                                          ' If the timer has ticked then let's stop
        ' Has the user already logged in?
        If My.Settings.UserID <> NO_USER Then
            Dim dash As New Dashboard()                                                 ' If so then redirect the user to the dashboard form
            dash.Show()
        Else
            Dim login As New LoginForm()                                                ' Else redirect the user to the login form
            login.Show()
        End If
        Me.Close()                                                                      ' Close this form
    End Sub

    Private Sub SplashScreen_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ' When this form is loaded then display the version of this application
        Dim appVersion As Version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version
        Dim format As String = versionLabel.Text
        versionLabel.Text = String.Format(versionLabel.Text, appVersion.Major, appVersion.Minor, appVersion.MinorRevision)
    End Sub
End Class
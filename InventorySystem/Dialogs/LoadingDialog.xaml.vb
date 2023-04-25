' This is for me

Option Strict On
Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider
Imports System.Windows.Threading

Public Class LoadingDialog
    Dim timerDispatcher As New DispatcherTimer

    Public Sub New()
        InitializeComponent()
        AddHandler timerDispatcher.Tick, AddressOf MySplashHandler                          ' We add a handler if the timer has ticked
        timerDispatcher.Interval = New TimeSpan(0, 0, 2)                                    ' The interval for every tick is 2 seconds
        timerDispatcher.Start()

    End Sub

    Private Sub MySplashHandler(sender As Object, e As EventArgs)
        timerDispatcher.Stop()                                                              ' If the dispatcher has triggered the tick we stop it
        ' We use this to close the dialog, idk how to use it so I look for a workaround to close it.

        Dim peer As ButtonAutomationPeer = TryCast(UIElementAutomationPeer.CreatePeerForElement(closebtn), ButtonAutomationPeer)
        ' If the peer variable has found the button
        If peer IsNot Nothing Then
            ' We invoke the click so that it the dialog will close
            Dim provider As IInvokeProvider = TryCast(peer.GetPattern(PatternInterface.Invoke), IInvokeProvider)
            If provider IsNot Nothing Then
                provider.Invoke()
            End If
        End If
    End Sub
End Class

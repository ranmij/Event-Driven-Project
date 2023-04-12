
Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider
Imports System.Windows.Threading
Imports HandyControl.Tools.Interop.InteropValues

Public Class LoadingDialog
    Dim timer As New DispatcherTimer

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        AddHandler timer.Tick, AddressOf MySplashHandler
        timer.Interval = New TimeSpan(0, 0, 2)
        timer.Start()
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub MySplashHandler()
        timer.Stop()
        ' Use ButtonAutomationPeer to simulate a button click
        Dim peer As ButtonAutomationPeer = TryCast(UIElementAutomationPeer.CreatePeerForElement(closebtn), ButtonAutomationPeer)
        If peer IsNot Nothing Then
            Dim provider As IInvokeProvider = TryCast(peer.GetPattern(PatternInterface.Invoke), IInvokeProvider)
            If provider IsNot Nothing Then
                provider.Invoke()
            End If
        End If
    End Sub
End Class

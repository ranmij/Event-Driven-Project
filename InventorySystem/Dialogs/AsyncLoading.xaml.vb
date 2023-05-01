Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider
Imports System.Windows.Threading

Public Class AsyncLoading
    Private asyncMethods() As Func(Of Task(Of List(Of String)))
    Private timerDispatcher As New DispatcherTimer
    Private tasksResults As New List(Of Task(Of List(Of String)))
    Private _controls() As Object

    Public Sub New(Optional asyncParam() As Func(Of Task(Of List(Of String))) = Nothing, Optional controls() As Object = Nothing)
        InitializeComponent()
        asyncMethods = asyncParam
        AddHandler timerDispatcher.Tick, AddressOf TickHandler
        timerDispatcher.Interval = New TimeSpan(0, 0, 2)                                    ' The interval for every tick is 2 seconds
        timerDispatcher.Start()
        _controls = controls
    End Sub

    Private Sub TickHandler(sender As Object, e As EventArgs)
        If asyncMethods Is Nothing AndAlso _controls Is Nothing Then
            TriggerCloseEvent()
        Else
            If tasksResults.All(Function(x As Task(Of List(Of String))) x.IsCompletedSuccessfully) Then
                timerDispatcher.Stop()                                                              ' If the dispatcher has triggered the tick and all the tasks are done then we stop it
                For i = 0 To tasksResults.Count - 1
                    DirectCast(_controls(i), ComboBox).ItemsSource = tasksResults(i).Result
                    DirectCast(_controls(i), ComboBox).SelectedIndex = 0
                Next
                TriggerCloseEvent()
            End If
        End If
    End Sub

    Private Sub AsyncLoading_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ' Start all the tasks
        If asyncMethods IsNot Nothing Then
            For Each asyncMethod As Func(Of Task(Of List(Of String))) In asyncMethods
                Dim asyncTask As Task(Of List(Of String)) = asyncMethod()
                tasksResults.Add(asyncTask)
            Next
        End If
    End Sub

    Private Sub TriggerCloseEvent()
        ' We use this to automatically close the dialog.

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

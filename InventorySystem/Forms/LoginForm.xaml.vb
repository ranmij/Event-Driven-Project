﻿Imports System.Windows.Threading
Imports HandyControl.Controls                               ' This is only use for the Dialog class

Public Class LoginForm
    Private signForm As SignUpForm

    ''' <summary>
    '''  We use this process to perform the log in action
    ''' </summary>
    Private Sub Verified()
        Dim controls() As Object = {UsernameTextBox, PasswordTextBox}
        ' Are the textboxes empty?
        If IsNotEmpty(controls) Then
            ' Is the user logged in?
            If Login(UsernameTextBox.Text, PasswordTextBox.Password) Then
                Dialog.Show(New LoadingDialog())
                Dim timer As New DispatcherTimer()                                  ' We will let the user wait for 2.5 seconds, dunno just wanna do this
                AddHandler timer.Tick, AddressOf CloseTick
                timer.Interval = New TimeSpan(0, 0, 2.5)                            ' Time interval to trigger the tick, in this case it's 2.5 seconds
                timer.Start()
            Else
                ' Display the incorect username or password error
                ' TODO A string literal!
                ErrorLabel.Visibility = Visibility.Visible
                ErrorLabel.Text = "Incorrect username or password."
            End If
        Else
            ' Display the empty field error
            ' TODO A string literal!
            ErrorLabel.Visibility = Visibility.Visible
            ErrorLabel.Text = "Please fill the empty fields."
        End If
    End Sub

    Private Sub ClickEvents(sender As Object, e As RoutedEventArgs) Handles CloseButton.Click, LoginButton.Click, SignupButton.Click
        If sender.Equals(CloseButton) Then              ' CLOSE
            Me.Close()
        ElseIf sender.Equals(LoginButton) Then          ' LOG IN
            Verified()
        ElseIf sender.Equals(SignupButton) Then         ' SIGN UP
            signForm = New SignUpForm()
            signForm.Show()
            Me.Close()
        End If
    End Sub

    ''' <summary>
    ''' Cose tick handler for the dispacher, we will close the form here and redirect the user to the dashboard
    ''' </summary>
    Private Sub CloseTick(sender As Object, e As EventArgs)
        TryCast(sender, DispatcherTimer).Stop()
        Dim dash As New Dashboard()
        dash.Show()
        Me.Close()

    End Sub

    ' To drag move the form
    Private Sub DragForm(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseLeftButtonDown
        DragMove()
    End Sub
End Class

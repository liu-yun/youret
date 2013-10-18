Public Class SplashWindow
    Dim count As Single = 0
    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        count = count + 1
        If count > 2 Then
            Dim mainwindow1 As New MainWindow
            mainwindow1.Show()
            Me.Close()
        End If
    End Sub
    Protected Overrides Sub OnMouseLeftButtonDown(e As MouseButtonEventArgs)
        MyBase.OnMouseLeftButtonDown(e)
        Me.DragMove()
    End Sub
    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
End Class


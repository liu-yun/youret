Public Class SplashWindow
    Dim _count As Single = 0
    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        _count = _count + 1
        If _count > 2 Then
            Dim mainwindow1 As New MainWindow
            mainwindow1.Show()
            Close()
        End If
    End Sub
    Protected Overrides Sub OnMouseLeftButtonDown(e As MouseButtonEventArgs)
        MyBase.OnMouseLeftButtonDown(e)
        DragMove()
    End Sub
    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub
End Class


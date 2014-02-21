Option Explicit On
Imports System.Windows.Interop
Public Class AboutWindow
    Protected Overrides Sub OnSourceInitialized(ByVal e As System.EventArgs)
        MyBase.OnSourceInitialized(e)
        ExtendGlass()
        labelversion.Content = "Version " + My.Application.Info.Version.Major.ToString + "." + My.Application.Info.Version.Minor.ToString + "." + My.Application.Info.Version.Build.ToString
    End Sub
    Private Declare Sub DwmIsCompositionEnabled Lib "dwmapi.dll" (ByRef b As Boolean)
    Private Declare Sub DwmExtendFrameIntoClientArea Lib "dwmapi.dll" (ByVal hWnd As IntPtr, ByRef pMarInset As Margins)
    Private Structure Margins
        Public Sub New(ByVal t As Thickness)
            Left = CInt(t.Left)
            Right = CInt(t.Right)
            Top = CInt(t.Top)
            Bottom = CInt(t.Bottom)
        End Sub

        Public Left As Integer
        Public Right As Integer
        Public Top As Integer
        Public Bottom As Integer

    End Structure
    Private Sub ExtendGlass()
        Try
            Dim b As Boolean
            DwmIsCompositionEnabled(b)
            If b Then
                Dim hWnd As IntPtr = New WindowInteropHelper(Me).Handle
                If hWnd <> IntPtr.Zero Then
                    Me.Background = Brushes.Transparent
                    HwndSource.FromHwnd(hWnd).CompositionTarget.BackgroundColor = Colors.Transparent
                    Dim m As New Margins(New Thickness(-1))
                    DwmExtendFrameIntoClientArea(hWnd, m)
                End If
            End If
        Catch
        End Try
    End Sub
    Private mouse_offset As Point
    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Protected Overrides Sub OnMouseLeftButtonDown(e As MouseButtonEventArgs)
        MyBase.OnMouseLeftButtonDown(e)
        Me.DragMove()
    End Sub
End Class

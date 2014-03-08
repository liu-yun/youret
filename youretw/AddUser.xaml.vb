Option Explicit Off
Imports System.Data.SqlClient
Imports System.Windows.Interop

Public Class AddUser
    Protected Overrides Sub OnSourceInitialized(ByVal e As EventArgs)
        MyBase.OnSourceInitialized(e)
        ExtendGlass()
    End Sub
    Protected Overrides Sub OnMouseLeftButtonDown(e As MouseButtonEventArgs)
        MyBase.OnMouseLeftButtonDown(e)
        DragMove()
    End Sub
    Private Declare Sub DwmIsCompositionEnabled Lib "dwmapi.dll" (ByRef b As Boolean)
    Private Declare Sub DwmExtendFrameIntoClientArea Lib "dwmapi.dll" (ByVal hWnd As IntPtr, ByRef pMarInset As Margins)
    Private Structure Margins
        Public Sub New(ByVal t As Thickness)
            _left = CInt(t.Left)
            _right = CInt(t.Right)
            _top = CInt(t.Top)
            _bottom = CInt(t.Bottom)
        End Sub

        Private _left As Integer
        Private _right As Integer
        Private _top As Integer
        Private _bottom As Integer
    End Structure
    Private Sub ExtendGlass()
        Try
            Dim b As Boolean
            DwmIsCompositionEnabled(b)
            If b Then
                Dim hWnd As IntPtr = New WindowInteropHelper(Me).Handle
                If hWnd <> IntPtr.Zero Then
                    Background = Brushes.Transparent
                    HwndSource.FromHwnd(hWnd).CompositionTarget.BackgroundColor = Colors.Transparent
                    Dim m As New Margins(New Thickness(-1))
                    DwmExtendFrameIntoClientArea(hWnd, m)
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim id As String = TextBoxId.Text
        Dim username As String = TextBoxUsername.Text
        Dim password As String = TextBoxPassword.Text
        If id <> "" And username <> "" And password <> "" Then
            Dim conn As SqlConnection = New SqlConnection("Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\Passwords.mdf;Integrated Security=True")
            Dim sql As String = "INSERT INTO [Table] ([Id], [Username], [Password]) VALUES ('" + id + "', '" + username + "', '" + password + "' )"
            Dim cmd As New SqlCommand(sql, conn)
            conn.Open()
            Try
                cmd.ExecuteNonQuery()
                conn.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
        Close()
    End Sub
End Class

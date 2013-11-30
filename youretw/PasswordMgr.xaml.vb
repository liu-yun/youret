Option Explicit On
Imports System.Windows.Interop
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data

Public Class PasswordMgr
    Protected Overrides Sub OnSourceInitialized(ByVal e As System.EventArgs)
        MyBase.OnSourceInitialized(e)
        ExtendGlass()
    End Sub
    Protected Overrides Sub OnMouseLeftButtonDown(e As MouseButtonEventArgs)
        MyBase.OnMouseLeftButtonDown(e)
        Me.DragMove()
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

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded
        Try
            Dim PasswordsDataSet As youret.PasswordsDataSet = CType(Me.FindResource("PasswordsDataSet"), youret.PasswordsDataSet)
            Dim PasswordsDataSetTableTableAdapter As youret.PasswordsDataSetTableAdapters.TableTableAdapter = New youret.PasswordsDataSetTableAdapters.TableTableAdapter()
            PasswordsDataSetTableTableAdapter.Fill(PasswordsDataSet.Table)
            Dim TableViewSource As System.Windows.Data.CollectionViewSource = CType(Me.FindResource("TableViewSource"), System.Windows.Data.CollectionViewSource)
            TableViewSource.View.MoveCurrentToFirst()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub addbt_Click(sender As Object, e As RoutedEventArgs) Handles addbt.Click
        Dim add As New AddUser
        add.Owner = Me
        add.ShowDialog()
        Try
            Dim PasswordsDataSet As youret.PasswordsDataSet = CType(Me.FindResource("PasswordsDataSet"), youret.PasswordsDataSet)
            Dim PasswordsDataSetTableTableAdapter As youret.PasswordsDataSetTableAdapters.TableTableAdapter = New youret.PasswordsDataSetTableAdapters.TableTableAdapter()
            PasswordsDataSetTableTableAdapter.Fill(PasswordsDataSet.Table)
            Dim TableViewSource As System.Windows.Data.CollectionViewSource = CType(Me.FindResource("TableViewSource"), System.Windows.Data.CollectionViewSource)
            TableViewSource.View.MoveCurrentToFirst()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim selected As String = TableDataGrid.SelectedItem(0)
        Dim conn As SqlConnection = New SqlConnection("Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\Passwords.mdf;Integrated Security=True")
        Dim sql As String = "DELETE FROM [Table] WHERE ([Id] = '" + selected + "')"
        Dim cmd As New SqlCommand(sql, conn)
        Try
            conn.Open()
            Dim res As String = cmd.ExecuteNonQuery()
            conn.Close()
            conn.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Try
            Dim PasswordsDataSet As youret.PasswordsDataSet = CType(Me.FindResource("PasswordsDataSet"), youret.PasswordsDataSet)
            Dim PasswordsDataSetTableTableAdapter As youret.PasswordsDataSetTableAdapters.TableTableAdapter = New youret.PasswordsDataSetTableAdapters.TableTableAdapter()
            PasswordsDataSetTableTableAdapter.Fill(PasswordsDataSet.Table)
            Dim TableViewSource As System.Windows.Data.CollectionViewSource = CType(Me.FindResource("TableViewSource"), System.Windows.Data.CollectionViewSource)
            TableViewSource.View.MoveCurrentToFirst()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class

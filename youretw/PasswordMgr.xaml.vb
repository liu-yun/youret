Option Explicit Off
Imports System.Windows.Interop
Imports System.Data.SqlClient
Imports youret.PasswordsDataSetTableAdapters

Public Class PasswordMgr
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
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded
        Try
            Dim passwordsDataSet As PasswordsDataSet = CType(FindResource("PasswordsDataSet"), PasswordsDataSet)
            Dim passwordsDataSetTableTableAdapter As TableTableAdapter = New TableTableAdapter()
            passwordsDataSetTableTableAdapter.Fill(passwordsDataSet.Table)
            Dim tableViewSource As CollectionViewSource = CType(FindResource("TableViewSource"), CollectionViewSource)
            tableViewSource.View.MoveCurrentToFirst()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonAdd_Click(sender As Object, e As RoutedEventArgs) Handles ButtonAdd.Click
        Dim add As New AddUser
        add.Owner = Me
        add.ShowDialog()
        Try
            Dim passwordsDataSet As PasswordsDataSet = CType(FindResource("PasswordsDataSet"), PasswordsDataSet)
            Dim passwordsDataSetTableTableAdapter As TableTableAdapter = New TableTableAdapter()
            passwordsDataSetTableTableAdapter.Fill(passwordsDataSet.Table)
            Dim tableViewSource As CollectionViewSource = CType(FindResource("TableViewSource"), CollectionViewSource)
            tableViewSource.View.MoveCurrentToFirst()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim selected As String = TableDataGrid.SelectedItem(0)
        Dim conn As SqlConnection = New SqlConnection("Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\Passwords.mdf;Integrated Security=True")
        Dim sql As String = "DELETE FROM [Table] WHERE ([Id] = '" + selected + "')"
        Dim cmd As New SqlCommand(sql, conn)
        Try
            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()
            conn.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Try
            Dim passwordsDataSet As PasswordsDataSet = CType(FindResource("PasswordsDataSet"), PasswordsDataSet)
            Dim passwordsDataSetTableTableAdapter As TableTableAdapter = New TableTableAdapter()
            passwordsDataSetTableTableAdapter.Fill(passwordsDataSet.Table)
            Dim tableViewSource As CollectionViewSource = CType(FindResource("TableViewSource"), CollectionViewSource)
            tableViewSource.View.MoveCurrentToFirst()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class

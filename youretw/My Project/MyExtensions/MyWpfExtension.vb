Imports System.Diagnostics.CodeAnalysis
Imports System.ComponentModel
Imports Microsoft.VisualBasic.Logging
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.Devices

Imports System.Reflection

#If _MyType <> "Empty" Then

Namespace My
    ''' <summary>
    ''' 用于定义“我的 WPF 命名空间”中的可用属性的模块
    ''' </summary>
    ''' <remarks></remarks>
    <HideModuleName()> _
    Module MyWpfExtension
        Private s_Computer As New ThreadSafeObjectProvider(Of Computer)
        Private s_User As New ThreadSafeObjectProvider(Of User)
        Private s_Windows As New ThreadSafeObjectProvider(Of MyWindows)
        Private s_Log As New ThreadSafeObjectProvider(Of Log)
        ''' <summary>
        ''' 返回正在运行的应用程序的应用程序对象
        ''' </summary>
        <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend ReadOnly Property Application() As Application
            Get
                Return CType(System.Windows.Application.Current, Application)
            End Get
        End Property
        ''' <summary>
        ''' 返回有关主机计算机的信息。
        ''' </summary>
        <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend ReadOnly Property Computer() As Computer
            Get
                Return s_Computer.GetInstance()
            End Get
        End Property
        ''' <summary>
        ''' 返回当前用户的信息。如果希望使用当前的 
        ''' Windows 用户凭据来运行应用程序，请调用 My.User.InitializeWithWindowsUser()。
        ''' </summary>
        <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend ReadOnly Property User() As User
            Get
                Return s_User.GetInstance()
            End Get
        End Property
        ''' <summary>
        ''' 返回应用程序日志。可以使用应用程序的配置文件配置侦听器。
        ''' </summary>
        <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend ReadOnly Property Log() As Log
            Get
                Return s_Log.GetInstance()
            End Get
        End Property

        ''' <summary>
        ''' 返回项目中定义的 Windows 集合。
        ''' </summary>
        <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend ReadOnly Property Windows() As MyWindows
            <DebuggerHidden()> _
            Get
                Return s_Windows.GetInstance()
            End Get
        End Property
        <EditorBrowsable(EditorBrowsableState.Never)> _
        <MyGroupCollection("System.Windows.Window", "Create__Instance__", "Dispose__Instance__", "My.MyWpfExtenstionModule.Windows")> _
        Friend NotInheritable Class MyWindows
            <DebuggerHidden()> _
            Private Shared Function Create__Instance__(Of T As {New, Window})(ByVal Instance As T) As T
                If Instance Is Nothing Then
                    If s_WindowBeingCreated IsNot Nothing Then
                        If s_WindowBeingCreated.ContainsKey(GetType(T)) = True Then
                            Throw New InvalidOperationException("The window cannot be accessed via My.Windows from the Window constructor.")
                        End If
                    Else
                        s_WindowBeingCreated = New Hashtable()
                    End If
                    s_WindowBeingCreated.Add(GetType(T), Nothing)
                    Return New T()
                    s_WindowBeingCreated.Remove(GetType(T))
                Else
                    Return Instance
                End If
            End Function
            <SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>  _
            <DebuggerHidden()> _
            Private Sub Dispose__Instance__(Of T As Window)(ByRef instance As T)
                instance = Nothing
            End Sub
            <DebuggerHidden()> _
            <EditorBrowsable(EditorBrowsableState.Never)> _
            Public Sub New()
                MyBase.New()
            End Sub
            <ThreadStatic()> Private Shared s_WindowBeingCreated As Hashtable
            <EditorBrowsable(EditorBrowsableState.Never)> Public Overrides Function Equals(ByVal o As Object) As Boolean
                Return MyBase.Equals(o)
            End Function
            <EditorBrowsable(EditorBrowsableState.Never)> Public Overrides Function GetHashCode() As Integer
                Return MyBase.GetHashCode
            End Function
            <SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>  _
            <EditorBrowsable(EditorBrowsableState.Never)> _
            Friend Overloads Function [GetType]() As Type
                Return GetType(MyWindows)
            End Function
            <EditorBrowsable(EditorBrowsableState.Never)> Public Overrides Function ToString() As String
                Return MyBase.ToString
            End Function
        End Class
    End Module
End Namespace
Partial Class Application
    Inherits Windows.Application
    <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")> _
    <SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")> _
    Friend ReadOnly Property Info() As AssemblyInfo
        <DebuggerHidden()> _
        Get
            Return New AssemblyInfo(Assembly.GetExecutingAssembly())
        End Get
    End Property
End Class
#End If
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AdminDashboard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.adminLogout = New System.Windows.Forms.Button()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.customersNav = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.reportsNav = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.bookingsNav = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.usersNav = New System.Windows.Forms.Button()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel5)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel1)
        Me.SplitContainer1.Panel1.Padding = New System.Windows.Forms.Padding(5)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Padding = New System.Windows.Forms.Padding(5)
        Me.SplitContainer1.Size = New System.Drawing.Size(851, 529)
        Me.SplitContainer1.SplitterDistance = 213
        Me.SplitContainer1.TabIndex = 0
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.adminLogout)
        Me.Panel5.Location = New System.Drawing.Point(3, 345)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(209, 45)
        Me.Panel5.TabIndex = 4
        '
        'adminLogout
        '
        Me.adminLogout.Location = New System.Drawing.Point(0, 4)
        Me.adminLogout.Name = "adminLogout"
        Me.adminLogout.Size = New System.Drawing.Size(206, 38)
        Me.adminLogout.TabIndex = 0
        Me.adminLogout.Text = "Logout"
        Me.adminLogout.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.customersNav)
        Me.Panel4.Location = New System.Drawing.Point(3, 197)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(209, 45)
        Me.Panel4.TabIndex = 3
        '
        'customersNav
        '
        Me.customersNav.Location = New System.Drawing.Point(0, 4)
        Me.customersNav.Name = "customersNav"
        Me.customersNav.Size = New System.Drawing.Size(206, 38)
        Me.customersNav.TabIndex = 0
        Me.customersNav.Text = "Customers"
        Me.customersNav.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.reportsNav)
        Me.Panel3.Location = New System.Drawing.Point(3, 146)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(209, 45)
        Me.Panel3.TabIndex = 2
        '
        'reportsNav
        '
        Me.reportsNav.Location = New System.Drawing.Point(0, 4)
        Me.reportsNav.Name = "reportsNav"
        Me.reportsNav.Size = New System.Drawing.Size(206, 38)
        Me.reportsNav.TabIndex = 0
        Me.reportsNav.Text = "Reports"
        Me.reportsNav.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.bookingsNav)
        Me.Panel2.Location = New System.Drawing.Point(3, 95)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(209, 45)
        Me.Panel2.TabIndex = 1
        '
        'bookingsNav
        '
        Me.bookingsNav.Location = New System.Drawing.Point(0, 4)
        Me.bookingsNav.Name = "bookingsNav"
        Me.bookingsNav.Size = New System.Drawing.Size(206, 38)
        Me.bookingsNav.TabIndex = 0
        Me.bookingsNav.Text = "Bookings"
        Me.bookingsNav.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.usersNav)
        Me.Panel1.Location = New System.Drawing.Point(3, 44)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(209, 45)
        Me.Panel1.TabIndex = 0
        '
        'usersNav
        '
        Me.usersNav.Location = New System.Drawing.Point(0, 4)
        Me.usersNav.Name = "usersNav"
        Me.usersNav.Size = New System.Drawing.Size(206, 38)
        Me.usersNav.TabIndex = 0
        Me.usersNav.Text = "Users"
        Me.usersNav.UseVisualStyleBackColor = True
        '
        'AdminDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(851, 529)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "AdminDashboard"
        Me.Text = "AdminDashboard"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents Panel5 As Panel
    Friend WithEvents adminLogout As Button
    Friend WithEvents Panel4 As Panel
    Friend WithEvents customersNav As Button
    Friend WithEvents Panel3 As Panel
    Friend WithEvents reportsNav As Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents bookingsNav As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents usersNav As Button
End Class

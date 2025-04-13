<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ClerkDashboard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.accountantLogout = New System.Windows.Forms.Button()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.reportsNav = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.refundNav = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.paymentNav = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.bookingsNav = New System.Windows.Forms.Button()
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
        Me.SplitContainer1.Size = New System.Drawing.Size(643, 450)
        Me.SplitContainer1.SplitterDistance = 214
        Me.SplitContainer1.TabIndex = 0
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.accountantLogout)
        Me.Panel5.Location = New System.Drawing.Point(3, 340)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(209, 45)
        Me.Panel5.TabIndex = 9
        '
        'accountantLogout
        '
        Me.accountantLogout.Location = New System.Drawing.Point(0, 4)
        Me.accountantLogout.Name = "accountantLogout"
        Me.accountantLogout.Size = New System.Drawing.Size(206, 38)
        Me.accountantLogout.TabIndex = 0
        Me.accountantLogout.Text = "Logout"
        Me.accountantLogout.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.reportsNav)
        Me.Panel4.Location = New System.Drawing.Point(3, 192)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(209, 45)
        Me.Panel4.TabIndex = 8
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
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.refundNav)
        Me.Panel3.Location = New System.Drawing.Point(3, 141)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(209, 45)
        Me.Panel3.TabIndex = 7
        '
        'refundNav
        '
        Me.refundNav.Location = New System.Drawing.Point(0, 4)
        Me.refundNav.Name = "refundNav"
        Me.refundNav.Size = New System.Drawing.Size(206, 38)
        Me.refundNav.TabIndex = 0
        Me.refundNav.Text = "Refunds and Discounts"
        Me.refundNav.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.paymentNav)
        Me.Panel2.Location = New System.Drawing.Point(3, 90)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(209, 45)
        Me.Panel2.TabIndex = 6
        '
        'paymentNav
        '
        Me.paymentNav.Location = New System.Drawing.Point(0, 4)
        Me.paymentNav.Name = "paymentNav"
        Me.paymentNav.Size = New System.Drawing.Size(206, 38)
        Me.paymentNav.TabIndex = 0
        Me.paymentNav.Text = "Record Payment"
        Me.paymentNav.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.bookingsNav)
        Me.Panel1.Location = New System.Drawing.Point(3, 39)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(209, 45)
        Me.Panel1.TabIndex = 5
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
        'ClerkDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(643, 450)
        Me.Controls.Add(Me.SplitContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "ClerkDashboard"
        Me.Text = "Clerk & Accountant Dashboard"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
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
    Friend WithEvents accountantLogout As Button
    Friend WithEvents Panel4 As Panel
    Friend WithEvents reportsNav As Button
    Friend WithEvents Panel3 As Panel
    Friend WithEvents refundNav As Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents paymentNav As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents bookingsNav As Button
End Class

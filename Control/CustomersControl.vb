Public Class CustomersControl
    Inherits UserControl

    Public Sub New()
        InitializeComponent()
        SetupLayout()
    End Sub

    Private Sub SetupLayout()
        ' Add DataGridView
        Dim usersDataGrid As New DataGridView With {
            .Name = "usersDataGrid",
            .Dock = DockStyle.Top,
            .Height = 200,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        }

        ' Add columns
        usersDataGrid.Columns.Add("ID", "ID")
        usersDataGrid.Columns.Add("Name", "Name")
        usersDataGrid.Columns.Add("Username", "Username")
        usersDataGrid.Columns.Add("Role", "Role")
        usersDataGrid.Columns.Add("Status", "Status")

        ' Add input controls
        Dim lblName As New Label With {.Text = "Name"}
        Dim txtName As New TextBox With {.Name = "txtName"}

        Dim lblUsername As New Label With {.Text = "Username"}
        Dim txtUsername As New TextBox With {.Name = "txtUsername"}

        Dim lblPassword As New Label With {.Text = "Password"}
        Dim txtPassword As New TextBox With {.Name = "txtPassword", .PasswordChar = "*"c}

        Dim lblRole As New Label With {.Text = "Role"}
        Dim cmbRole As New ComboBox With {.Name = "cmbRole"}
        cmbRole.Items.AddRange({"Admin", "Staff", "Clerk"})

        Dim lblStatus As New Label With {.Text = "Status"}
        Dim cmbStatus As New ComboBox With {.Name = "cmbStatus"}
        cmbStatus.Items.AddRange({"Active", "Inactive"})

        ' Add buttons
        Dim btnAdd As New Button With {.Text = "Add", .Name = "btnAdd"}
        Dim btnUpdate As New Button With {.Text = "Update", .Name = "btnUpdate"}
        Dim btnDelete As New Button With {.Text = "Delete", .Name = "btnDelete"}
        Dim btnClear As New Button With {.Text = "Clear", .Name = "btnClear"}

        ' Layout
        Dim layout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 7,
            .Padding = New Padding(10)
        }
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70))

        ' Add controls to layout
        layout.Controls.Add(lblName, 0, 0)
        layout.Controls.Add(txtName, 1, 0)

        layout.Controls.Add(lblUsername, 0, 1)
        layout.Controls.Add(txtUsername, 1, 1)

        layout.Controls.Add(lblPassword, 0, 2)
        layout.Controls.Add(txtPassword, 1, 2)

        layout.Controls.Add(lblRole, 0, 3)
        layout.Controls.Add(cmbRole, 1, 3)

        layout.Controls.Add(lblStatus, 0, 4)
        layout.Controls.Add(cmbStatus, 1, 4)

        layout.Controls.Add(btnAdd, 0, 5)
        layout.Controls.Add(btnUpdate, 1, 5)
        layout.Controls.Add(btnDelete, 0, 6)
        layout.Controls.Add(btnClear, 1, 6)

        ' Add to UserControl
        Me.Controls.Add(layout)
        Me.Controls.Add(usersDataGrid)
    End Sub
End Class

Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Drawing
Imports System.ComponentModel

Public Class StaffCustomerControl
    Inherits UserControl

    ' UI Controls
    Private customerGrid As DataGridView
    Private txtSearch As TextBox
    Private btnSearch As Button
    Private txtFirstName, txtLastName, txtMiddleName, txtContact, txtEmail, txtAddress, txtCity, txtProvince, txtZip As TextBox
    Private cmbGender, cmbCustomerType, cmbStatus As ComboBox
    Private dtpBirthDate As DateTimePicker
    Private btnAdd, btnUpdate, btnClear As Button

    ' Colors for modern theme
    Private ReadOnly primaryColor As Color = Color.FromArgb(100, 181, 246)    ' Light blue
    Private ReadOnly accentColor As Color = Color.FromArgb(255, 112, 167)      ' Pink
    Private ReadOnly textColor As Color = Color.FromArgb(60, 60, 60)           ' Dark gray
    Private ReadOnly lightColor As Color = Color.FromArgb(248, 248, 252)       ' Off-white

    Public Sub New()
        InitializeComponent()
        SetupLayout()
        ApplyStyles()
        LoadCustomers()
    End Sub

    Private Sub SetupLayout()
        ' Main layout container
        Dim mainContainer As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 4,
            .Padding = New Padding(15),
            .BackColor = lightColor
        }

        mainContainer.RowStyles.Add(New RowStyle(SizeType.Absolute, 60))     ' Header section
        mainContainer.RowStyles.Add(New RowStyle(SizeType.Absolute, 250))    ' DataGridView
        mainContainer.RowStyles.Add(New RowStyle(SizeType.Percent, 100))     ' Form fields
        mainContainer.RowStyles.Add(New RowStyle(SizeType.Absolute, 50))     ' Buttons

        ' ==== HEADER SECTION ====
        Dim headerPanel As New Panel With {
            .Dock = DockStyle.Fill,
            .BackColor = primaryColor,
            .Margin = New Padding(0, 0, 0, 10)
        }

        ' Title and search controls
        Dim lblTitle As New Label With {
            .Text = "Customer Management",
            .Font = New Font("Segoe UI Semibold", 14),
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(15, 15)
        }

        txtSearch = New TextBox With {
            .Width = 250,
            .Height = 28,
            .Location = New Point(500, 15),
            .Font = New Font("Segoe UI", 10),
            .BorderStyle = BorderStyle.None
        }

        ' Cute search button with icon-like appearance
        btnSearch = New Button With {
            .Text = "🔍",
            .Font = New Font("Segoe UI", 12),
            .Width = 40,
            .Height = 28,
            .Location = New Point(750, 15),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = accentColor,
            .ForeColor = Color.White,
            .Cursor = Cursors.Hand
        }

        AddHandler txtSearch.TextChanged, AddressOf txtSearch_TextChanged
        AddHandler btnSearch.Click, AddressOf btnSearch_Click

        headerPanel.Controls.Add(lblTitle)
        headerPanel.Controls.Add(txtSearch)
        headerPanel.Controls.Add(btnSearch)

        ' ==== DATA GRID SECTION ====
        customerGrid = New DataGridView With {
            .Dock = DockStyle.Fill,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False,
            .BorderStyle = BorderStyle.None,
            .BackgroundColor = Color.White,
            .RowHeadersVisible = False,
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            .RowTemplate = New DataGridViewRow With {.Height = 30},
            .Margin = New Padding(0, 0, 0, 10)
        }

        AddHandler customerGrid.CellClick, AddressOf customerGrid_CellClick

        ' ==== FORM FIELDS SECTION ====
        ' Cute container with rounded corners effect
        Dim formPanel As New Panel With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.White,
            .Padding = New Padding(20)
        }

        ' Two-column layout for form fields
        Dim formLayout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 4,
            .RowCount = 7,
            .Padding = New Padding(5)
        }

        ' Set column styles
        formLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 15))
        formLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 35))
        formLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 15))
        formLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 35))

        ' Initialize form controls
        txtFirstName = CreateTextBox()
        txtLastName = CreateTextBox()
        txtMiddleName = CreateTextBox()
        txtContact = CreateTextBox()
        txtEmail = CreateTextBox()
        txtAddress = CreateTextBox()
        txtCity = CreateTextBox()
        txtProvince = CreateTextBox()
        txtZip = CreateTextBox()

        cmbGender = CreateComboBox({"Male", "Female", "Other"})
        cmbCustomerType = CreateComboBox({"Regular", "Walk-in", "Corporate"})
        cmbStatus = CreateComboBox({"Active", "Inactive"})

        dtpBirthDate = New DateTimePicker With {
            .Dock = DockStyle.Fill,
            .Format = DateTimePickerFormat.Short,
            .Font = New Font("Segoe UI", 9.5F)
        }

        ' Add controls to form layout - Left column
        AddFormField(formLayout, "First Name:", txtFirstName, 0, 0)
        AddFormField(formLayout, "Last Name:", txtLastName, 0, 1)
        AddFormField(formLayout, "Middle Name:", txtMiddleName, 0, 2)
        AddFormField(formLayout, "Gender:", cmbGender, 0, 3)
        AddFormField(formLayout, "Birth Date:", dtpBirthDate, 0, 4)
        AddFormField(formLayout, "Contact:", txtContact, 0, 5)
        AddFormField(formLayout, "Email:", txtEmail, 0, 6)

        ' Add controls to form layout - Right column
        AddFormField(formLayout, "Address:", txtAddress, 2, 0)
        AddFormField(formLayout, "City:", txtCity, 2, 1)
        AddFormField(formLayout, "Province:", txtProvince, 2, 2)
        AddFormField(formLayout, "Zip Code:", txtZip, 2, 3)
        AddFormField(formLayout, "Customer Type:", cmbCustomerType, 2, 4)
        AddFormField(formLayout, "Status:", cmbStatus, 2, 5)

        formPanel.Controls.Add(formLayout)

        ' ==== BUTTONS SECTION ====
        Dim buttonPanel As New FlowLayoutPanel With {
            .Dock = DockStyle.Fill,
            .FlowDirection = FlowDirection.RightToLeft,
            .WrapContents = False,
            .Padding = New Padding(0, 5, 0, 0)
        }

        btnClear = CreateButton("Clear", Color.Gray)
        btnUpdate = CreateButton("Update", primaryColor)
        btnAdd = CreateButton("Register", accentColor)

        AddHandler btnAdd.Click, AddressOf btnAdd_Click
        AddHandler btnUpdate.Click, AddressOf btnUpdate_Click
        AddHandler btnClear.Click, AddressOf btnClear_Click

        buttonPanel.Controls.Add(btnClear)
        buttonPanel.Controls.Add(btnUpdate)
        buttonPanel.Controls.Add(btnAdd)

        ' Add all sections to main container
        mainContainer.Controls.Add(headerPanel, 0, 0)
        mainContainer.Controls.Add(customerGrid, 0, 1)
        mainContainer.Controls.Add(formPanel, 0, 2)
        mainContainer.Controls.Add(buttonPanel, 0, 3)

        Me.Controls.Add(mainContainer)
    End Sub

    Private Sub ApplyStyles()
        ' DataGridView styling
        customerGrid.EnableHeadersVisualStyles = False
        customerGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        customerGrid.ColumnHeadersDefaultCellStyle.BackColor = primaryColor
        customerGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        customerGrid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI Semibold", 10)
        customerGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        customerGrid.ColumnHeadersHeight = 40

        customerGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 234, 245)
        customerGrid.DefaultCellStyle.SelectionForeColor = textColor
        customerGrid.DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
        customerGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250)

        ' Set border for search box
        SetTextBoxBorder(txtSearch)
    End Sub

    ' Helper method to create styled TextBox
    Private Function CreateTextBox() As TextBox
        Dim txt As New TextBox With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.5F),
            .BackColor = Color.White,
            .Margin = New Padding(0, 0, 15, 10)
        }
        SetTextBoxBorder(txt)
        Return txt
    End Function

    ' Helper method to create styled ComboBox
    Private Function CreateComboBox(items As String()) As ComboBox
        Dim cmb As New ComboBox With {
            .Dock = DockStyle.Fill,
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Font = New Font("Segoe UI", 9.5F),
            .BackColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Margin = New Padding(0, 0, 15, 10)
        }
        cmb.Items.AddRange(items)
        Return cmb
    End Function

    ' Helper method to create styled Button
    Private Function CreateButton(text As String, bgColor As Color) As Button
        Dim btn As New Button With {
            .text = text,
            .Width = 120,
            .Height = 35,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = bgColor,
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI Semibold", 10),
            .Margin = New Padding(10, 0, 0, 0),
            .Cursor = Cursors.Hand
        }
        Return btn
    End Function

    ' Helper method to add label + control pair to form layout
    Private Sub AddFormField(layout As TableLayoutPanel, labelText As String, control As Control, col As Integer, row As Integer)
        Dim lbl As New Label With {
            .Text = labelText,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.5F),
            .ForeColor = textColor,
            .Margin = New Padding(0, 0, 0, 10)
        }

        layout.Controls.Add(lbl, col, row)
        layout.Controls.Add(control, col + 1, row)
    End Sub

    ' Helper method to set custom border for TextBox
    Private Sub SetTextBoxBorder(txt As TextBox)
        txt.BorderStyle = BorderStyle.FixedSingle
    End Sub

    ' ====== DATA OPERATIONS ======
    Private Sub LoadCustomers()
        Dim query As String = "SELECT * FROM customer"
        customerGrid.DataSource = GetData(query)
    End Sub


    ' ====== EVENT HANDLERS ======
    Private Sub btnAdd_Click(sender As Object, e As EventArgs)
        If ValidateInput() = False Then
            MessageBox.Show("Please fill in all required fields.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim query As String = "INSERT INTO customer (FirstName, LastName, MiddleName, Gender, BirthDate, ContactNumber, Email, AddressLine, City, Province, ZipCode, CustomerType, Status)
                               VALUES (@FirstName, @LastName, @MiddleName, @Gender, @BirthDate, @ContactNumber, @Email, @AddressLine, @City, @Province, @ZipCode, @CustomerType, @Status)"

        Dim parameters As New Dictionary(Of String, Object) From {
            {"@FirstName", txtFirstName.Text},
            {"@LastName", txtLastName.Text},
            {"@MiddleName", txtMiddleName.Text},
            {"@Gender", cmbGender.Text},
            {"@BirthDate", dtpBirthDate.Value},
            {"@ContactNumber", txtContact.Text},
            {"@Email", txtEmail.Text},
            {"@AddressLine", txtAddress.Text},
            {"@City", txtCity.Text},
            {"@Province", txtProvince.Text},
            {"@ZipCode", txtZip.Text},
            {"@CustomerType", cmbCustomerType.Text},
            {"@Status", cmbStatus.Text}
        }

        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("Customer registered successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadCustomers()
            ClearFields()
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs)
        If customerGrid.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a customer to update.", "Selection Required",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If ValidateInput() = False Then
            MessageBox.Show("Please fill in all required fields.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim customerID As Integer = Convert.ToInt32(customerGrid.SelectedRows(0).Cells("CustomerID").Value)
        Dim query As String = "UPDATE customer SET FirstName=@FirstName, LastName=@LastName, MiddleName=@MiddleName,
                               Gender=@Gender, BirthDate=@BirthDate, ContactNumber=@ContactNumber, Email=@Email,
                               AddressLine=@AddressLine, City=@City, Province=@Province, ZipCode=@ZipCode,
                               CustomerType=@CustomerType, Status=@Status
                               WHERE CustomerID=@CustomerID"

        Dim parameters As New Dictionary(Of String, Object) From {
            {"@CustomerID", customerID},
            {"@FirstName", txtFirstName.Text},
            {"@LastName", txtLastName.Text},
            {"@MiddleName", txtMiddleName.Text},
            {"@Gender", cmbGender.Text},
            {"@BirthDate", dtpBirthDate.Value},
            {"@ContactNumber", txtContact.Text},
            {"@Email", txtEmail.Text},
            {"@AddressLine", txtAddress.Text},
            {"@City", txtCity.Text},
            {"@Province", txtProvince.Text},
            {"@ZipCode", txtZip.Text},
            {"@CustomerType", cmbCustomerType.Text},
            {"@Status", cmbStatus.Text}
        }

        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("Customer information updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadCustomers()
            ClearFields()
        End If
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs)
        PerformSearch()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs)
        PerformSearch()
    End Sub

    Private Sub PerformSearch()
        Dim searchText As String = txtSearch.Text.Trim()
        Dim query As String = "SELECT * FROM customer WHERE FirstName LIKE @search OR LastName LIKE @search OR MiddleName LIKE @search"
        Dim parameters As New Dictionary(Of String, Object) From {
            {"@search", "%" & searchText & "%"}
        }
        customerGrid.DataSource = GetData(query, parameters)
    End Sub

    Private Sub customerGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim row = customerGrid.Rows(e.RowIndex)
            txtFirstName.Text = row.Cells("FirstName").Value.ToString()
            txtLastName.Text = row.Cells("LastName").Value.ToString()
            txtMiddleName.Text = row.Cells("MiddleName").Value.ToString()
            cmbGender.Text = row.Cells("Gender").Value.ToString()
            dtpBirthDate.Value = Convert.ToDateTime(row.Cells("BirthDate").Value)
            txtContact.Text = row.Cells("ContactNumber").Value.ToString()
            txtEmail.Text = row.Cells("Email").Value.ToString()
            txtAddress.Text = row.Cells("AddressLine").Value.ToString()
            txtCity.Text = row.Cells("City").Value.ToString()
            txtProvince.Text = row.Cells("Province").Value.ToString()
            txtZip.Text = row.Cells("ZipCode").Value.ToString()
            cmbCustomerType.Text = row.Cells("CustomerType").Value.ToString()
            cmbStatus.Text = row.Cells("Status").Value.ToString()

            ' Highlight selected row with a subtle indication
            customerGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 250)
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearFields()
        customerGrid.ClearSelection()
    End Sub

    Private Sub ClearFields()
        txtFirstName.Clear()
        txtLastName.Clear()
        txtMiddleName.Clear()
        txtContact.Clear()
        txtEmail.Clear()
        txtAddress.Clear()
        txtCity.Clear()
        txtProvince.Clear()
        txtZip.Clear()
        cmbCustomerType.SelectedIndex = -1
        cmbStatus.SelectedIndex = -1
        cmbGender.SelectedIndex = -1
        dtpBirthDate.Value = DateTime.Now
    End Sub

    Private Function ValidateInput() As Boolean
        ' Basic validation for required fields
        If String.IsNullOrWhiteSpace(txtFirstName.Text) OrElse
           String.IsNullOrWhiteSpace(txtLastName.Text) OrElse
           cmbGender.SelectedIndex = -1 OrElse
           String.IsNullOrWhiteSpace(txtContact.Text) OrElse
           cmbCustomerType.SelectedIndex = -1 OrElse
           cmbStatus.SelectedIndex = -1 Then
            Return False
        End If

        Return True
    End Function
End Class
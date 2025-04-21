Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Drawing
Imports System.Windows.Forms

Public Class AccountantRefundControl
    Inherits UserControl

    ' UI Controls
    Private dgvRefunds As DataGridView
    Private txtPaymentID, txtBookingID, txtRefundAmount, txtDiscountAmount As TextBox
    Private txtRemarks As RichTextBox
    Private btnProcess, btnClear As Button
    Private pnlHeader As Panel
    Private lblTitle As Label
    Private mainPanel As TableLayoutPanel
    Private pnlForm As Panel
    Private pnlButtons As FlowLayoutPanel
    Private toolTip As ToolTip
    Private splitContainer As SplitContainer

    Public Sub New()
        InitializeComponent()
        CreateCustomControls()
        SetupLayout()
        LoadRefunds()
    End Sub

    Private Sub CreateCustomControls()
        If dgvRefunds Is Nothing Then dgvRefunds = New DataGridView()
        If txtPaymentID Is Nothing Then txtPaymentID = New TextBox()
        If txtBookingID Is Nothing Then txtBookingID = New TextBox()
        If txtRefundAmount Is Nothing Then txtRefundAmount = New TextBox()
        If txtDiscountAmount Is Nothing Then txtDiscountAmount = New TextBox()
        If txtRemarks Is Nothing Then txtRemarks = New RichTextBox()
        If btnProcess Is Nothing Then btnProcess = New Button()
        If btnClear Is Nothing Then btnClear = New Button()
        If toolTip Is Nothing Then toolTip = New ToolTip()
        If pnlHeader Is Nothing Then pnlHeader = New Panel()
        If lblTitle Is Nothing Then lblTitle = New Label()
        If mainPanel Is Nothing Then mainPanel = New TableLayoutPanel()
        If pnlForm Is Nothing Then pnlForm = New Panel()
        If pnlButtons Is Nothing Then pnlButtons = New FlowLayoutPanel()

        If Not Controls.Contains(pnlHeader) Then Controls.Add(pnlHeader)
        If Not pnlHeader.Controls.Contains(lblTitle) Then pnlHeader.Controls.Add(lblTitle)
    End Sub

    Private Sub SetupLayout()
        toolTip.SetToolTip(txtRefundAmount, "Enter the amount to be refunded to the customer")
        toolTip.SetToolTip(txtDiscountAmount, "Enter the discount amount to be applied")

        ' Header panel
        pnlHeader.Dock = DockStyle.Top
        pnlHeader.Height = 60
        pnlHeader.BackColor = Color.FromArgb(41, 128, 185)

        lblTitle.Text = "✨ Refunds & Discounts Management"
        lblTitle.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        lblTitle.ForeColor = Color.White
        lblTitle.Dock = DockStyle.Fill
        lblTitle.TextAlign = ContentAlignment.MiddleLeft
        lblTitle.Padding = New Padding(15, 0, 0, 0)

        ' Configure DataGridView
        dgvRefunds.Dock = DockStyle.Fill
        dgvRefunds.ReadOnly = True
        dgvRefunds.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill ' Changed to Fill to use all available space
        dgvRefunds.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvRefunds.AllowUserToAddRows = False
        dgvRefunds.AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle With {
            .BackColor = Color.FromArgb(240, 240, 240)
        }
        dgvRefunds.BackgroundColor = Color.White
        dgvRefunds.BorderStyle = BorderStyle.None
        dgvRefunds.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgvRefunds.ColumnHeadersHeight = 40 ' Increase header height
        dgvRefunds.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        dgvRefunds.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgvRefunds.ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
            .BackColor = Color.FromArgb(52, 152, 219),
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 11, FontStyle.Regular),
            .Padding = New Padding(8),
            .SelectionBackColor = Color.FromArgb(52, 152, 219)
        }
        dgvRefunds.EnableHeadersVisualStyles = False
        dgvRefunds.GridColor = Color.FromArgb(230, 230, 230)
        dgvRefunds.RowHeadersVisible = False
        dgvRefunds.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
        dgvRefunds.RowTemplate = New DataGridViewRow With {
            .Height = 40,
            .DefaultCellStyle = New DataGridViewCellStyle With {
                .Font = New Font("Segoe UI", 10, FontStyle.Regular),
                .Padding = New Padding(5),
                .SelectionBackColor = Color.FromArgb(187, 222, 251),
                .SelectionForeColor = Color.Black,
                .WrapMode = DataGridViewTriState.True ' Enable wrapping for cell content
            }
        }
        AddHandler dgvRefunds.CellClick, AddressOf dgvRefunds_CellClick

        ' Configure textboxes
        ConfigureTextBoxes()

        ' Configure rich text box for remarks
        txtRemarks.Dock = DockStyle.Fill
        txtRemarks.Font = New Font("Segoe UI", 10)
        txtRemarks.BorderStyle = BorderStyle.FixedSingle
        txtRemarks.Multiline = True
        txtRemarks.Height = 120 ' Increased height from 80 to 120

        ' Configure buttons
        ConfigureButtons()

        ' Configure form panel
        pnlForm.Dock = DockStyle.Fill
        pnlForm.Padding = New Padding(15)

        ' Configure buttons panel
        pnlButtons.Dock = DockStyle.Bottom
        pnlButtons.Height = 60
        pnlButtons.FlowDirection = FlowDirection.RightToLeft
        pnlButtons.Padding = New Padding(10)
        pnlButtons.BackColor = Color.FromArgb(245, 245, 245)
        pnlButtons.Controls.Add(btnProcess)
        pnlButtons.Controls.Add(btnClear)

        ' Create split container
        splitContainer = New SplitContainer With {
            .Dock = DockStyle.Fill,
            .Orientation = Orientation.Horizontal,
            .BackColor = Color.FromArgb(250, 250, 250)
        }

        splitContainer.Panel1MinSize = 100
        splitContainer.Panel2MinSize = 100

        ' Configure the form layout
        ConfigureFormLayout()

        ' Setup DataGridView panel with a title
        Dim pnlGrid As New Panel With {
            .Dock = DockStyle.Fill,
            .Padding = New Padding(15)
        }

        Dim lblGridTitle As New Label With {
            .Text = "Recent Refunds & Discounts",
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .ForeColor = Color.FromArgb(52, 73, 94),
            .Dock = DockStyle.Top,
            .AutoSize = True, ' Ensure label sizes to fit text
            .Padding = New Padding(0, 0, 0, 15) ' Increased bottom padding
        }

        pnlGrid.Controls.Add(dgvRefunds)
        pnlGrid.Controls.Add(lblGridTitle)

        splitContainer.Panel1.Controls.Add(pnlGrid)
        splitContainer.Panel2.Controls.Add(pnlForm)

        Me.Controls.Add(splitContainer)
        Me.Controls.Add(pnlButtons)
        Me.Controls.Add(pnlHeader)

        Me.Size = New Size(900, 600) ' Increased width to accommodate content
        Me.MinimumSize = New Size(900, 600)

        AddHandler Me.Resize, AddressOf AccountantRefundControl_Resize

        If splitContainer.Height >= (splitContainer.Panel1MinSize + splitContainer.Panel2MinSize) Then
            ' Adjust ratio to 70/30 to give more space to the table
            Dim desiredDistance As Integer = CInt(splitContainer.Height * 0.4)
            splitContainer.SplitterDistance = Math.Max(splitContainer.Panel1MinSize,
                                                      Math.Min(desiredDistance,
                                                               splitContainer.Height - splitContainer.Panel2MinSize))
        End If
    End Sub

    Private Sub AccountantRefundControl_Resize(sender As Object, e As EventArgs)
        If splitContainer IsNot Nothing AndAlso splitContainer.Height >= (splitContainer.Panel1MinSize + splitContainer.Panel2MinSize) Then
            ' Maintain 70/30 ratio when resizing
            Dim desiredDistance As Integer = CInt(splitContainer.Height * 0.4)
            splitContainer.SplitterDistance = Math.Max(splitContainer.Panel1MinSize,
                                                      Math.Min(desiredDistance,
                                                               splitContainer.Height - splitContainer.Panel2MinSize))

            ' Force the DataGridView to refresh its layout
            dgvRefunds.Refresh()
        ElseIf splitContainer IsNot Nothing Then
            splitContainer.SplitterDistance = splitContainer.Panel1MinSize
        End If
    End Sub

    Private Sub ConfigureFormLayout()
        ' Create scrollable panel for the form
        Dim formScrollPanel As New Panel With {
            .Dock = DockStyle.Fill,
            .AutoScroll = True  ' Enable scrolling
        }

        mainPanel.Dock = DockStyle.Top  ' Change from Fill to Top
        mainPanel.AutoSize = True       ' Allow it to size to content
        mainPanel.ColumnCount = 2
        mainPanel.RowCount = 6
        mainPanel.Padding = New Padding(15)
        mainPanel.BackColor = Color.White

        mainPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        mainPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70))

        ' Explicitly set row styles to ensure all rows are visible
        For i As Integer = 0 To mainPanel.RowCount - 1
            mainPanel.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        Next

        AddFormRow(mainPanel, "Payment ID 💳", txtPaymentID, 0)
        AddFormRow(mainPanel, "Booking ID 🔖", txtBookingID, 1)
        AddFormRow(mainPanel, "Refund Amount 💵", txtRefundAmount, 2)
        AddFormRow(mainPanel, "Discount Amount 🏷️", txtDiscountAmount, 3)
        AddFormRow(mainPanel, "Remarks 📝", txtRemarks, 4)

        Dim lblInfo As New Label With {
            .Text = "📌 Select a payment record from the table above to process a refund or discount.",
            .ForeColor = Color.FromArgb(41, 128, 185),
            .Font = New Font("Segoe UI", 9, FontStyle.Italic),
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .Padding = New Padding(0, 5, 0, 10)
        }
        mainPanel.Controls.Add(lblInfo, 0, 5)
        mainPanel.SetColumnSpan(lblInfo, 2)

        Dim lblFormTitle As New Label With {
            .Text = "Payment Details",
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .ForeColor = Color.FromArgb(52, 73, 94),
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .Padding = New Padding(0, 0, 0, 15) ' Increased bottom padding
        }

        txtRemarks.MinimumSize = New Size(0, 120) ' Increased from 80 to 120
        txtRemarks.MaximumSize = New Size(0, 200) ' Increased from 150 to 200

        formScrollPanel.Controls.Add(mainPanel)
        pnlForm.Controls.Add(formScrollPanel)
        pnlForm.Controls.Add(lblFormTitle)
    End Sub

    Private Sub AddFormRow(panel As TableLayoutPanel, labelText As String, control As Control, rowIndex As Integer)
        Dim lbl As New Label With {
            .Text = labelText,
            .Font = New Font("Segoe UI", 10),
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Padding = New Padding(0, 0, 10, 0),
            .AutoSize = False ' Ensure label is not autosized to prevent truncation
        }
        panel.Controls.Add(lbl, 0, rowIndex)
        panel.Controls.Add(control, 1, rowIndex)
    End Sub

    Private Sub LoadRefunds()
        Dim query As String = "
            SELECT 
                PaymentID, BookingID, AmountPaid, RefundedAmount, DiscountAmount, Remarks, PaymentDate, ProcessedBy 
            FROM payment 
            WHERE RefundedAmount > 0 OR DiscountAmount > 0 
            ORDER BY PaymentDate DESC"

        Try
            dgvRefunds.DataSource = GetData(query)

            ' Format currency columns
            FormatCurrencyColumn("AmountPaid")
            FormatCurrencyColumn("RefundedAmount")
            FormatCurrencyColumn("DiscountAmount")

            ' Format date column
            If dgvRefunds.Columns.Contains("PaymentDate") Then
                dgvRefunds.Columns("PaymentDate").DefaultCellStyle.Format = "dd MMM yyyy HH:mm"
            End If

            ' Set friendly column headers and adjust column widths
            SetFriendlyColumnNames()
            AdjustDataGridViewColumns()
        Catch ex As Exception
            MessageBox.Show("Error loading refund data: " & ex.Message, "Data Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub AdjustDataGridViewColumns()
        ' Set specific widths for columns to ensure all text is visible
        If dgvRefunds.Columns.Contains("PaymentID") Then
            dgvRefunds.Columns("PaymentID").Width = 80
            dgvRefunds.Columns("PaymentID").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        End If
        If dgvRefunds.Columns.Contains("BookingID") Then
            dgvRefunds.Columns("BookingID").Width = 80
            dgvRefunds.Columns("BookingID").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        End If
        If dgvRefunds.Columns.Contains("AmountPaid") Then
            dgvRefunds.Columns("AmountPaid").Width = 100
            dgvRefunds.Columns("AmountPaid").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        End If
        If dgvRefunds.Columns.Contains("RefundedAmount") Then
            dgvRefunds.Columns("RefundedAmount").Width = 100
            dgvRefunds.Columns("RefundedAmount").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        End If
        If dgvRefunds.Columns.Contains("DiscountAmount") Then
            dgvRefunds.Columns("DiscountAmount").Width = 100
            dgvRefunds.Columns("DiscountAmount").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        End If
        If dgvRefunds.Columns.Contains("PaymentDate") Then
            dgvRefunds.Columns("PaymentDate").Width = 150
            dgvRefunds.Columns("PaymentDate").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        End If
        If dgvRefunds.Columns.Contains("ProcessedBy") Then
            dgvRefunds.Columns("ProcessedBy").Width = 100
            dgvRefunds.Columns("ProcessedBy").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        End If
        If dgvRefunds.Columns.Contains("Remarks") Then
            dgvRefunds.Columns("Remarks").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvRefunds.Columns("Remarks").DefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvRefunds.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        End If
    End Sub

    Private Sub FormatCurrencyColumn(columnName As String)
        If dgvRefunds.Columns.Contains(columnName) Then
            dgvRefunds.Columns(columnName).DefaultCellStyle.Format = "C2"
        End If
    End Sub

    Private Sub SetFriendlyColumnNames()
        If dgvRefunds.Columns.Contains("PaymentID") Then
            dgvRefunds.Columns("PaymentID").HeaderText = "Payment ID"
        End If
        If dgvRefunds.Columns.Contains("BookingID") Then
            dgvRefunds.Columns("BookingID").HeaderText = "Booking ID"
        End If
        If dgvRefunds.Columns.Contains("AmountPaid") Then
            dgvRefunds.Columns("AmountPaid").HeaderText = "Amount Paid"
        End If
        If dgvRefunds.Columns.Contains("RefundedAmount") Then
            dgvRefunds.Columns("RefundedAmount").HeaderText = "Refunded"
        End If
        If dgvRefunds.Columns.Contains("DiscountAmount") Then
            dgvRefunds.Columns("DiscountAmount").HeaderText = "Discount"
        End If
        If dgvRefunds.Columns.Contains("PaymentDate") Then
            dgvRefunds.Columns("PaymentDate").HeaderText = "Payment Date"
        End If
        If dgvRefunds.Columns.Contains("ProcessedBy") Then
            dgvRefunds.Columns("ProcessedBy").HeaderText = "Processed By"
        End If
        If dgvRefunds.Columns.Contains("Remarks") Then
            dgvRefunds.Columns("Remarks").HeaderText = "Remarks"
        End If
    End Sub

    Private Sub ConfigureTextBoxes()
        ConfigureTextBox(txtPaymentID, True)
        ConfigureTextBox(txtBookingID, True)
        ConfigureTextBox(txtRefundAmount, False)
        ConfigureTextBox(txtDiscountAmount, False)
    End Sub

    Private Sub ConfigureTextBox(txt As TextBox, isReadOnly As Boolean)
        txt.Dock = DockStyle.Fill
        txt.Font = New Font("Segoe UI", 10)
        txt.BorderStyle = BorderStyle.FixedSingle
        txt.ReadOnly = isReadOnly
        txt.BackColor = If(isReadOnly, Color.FromArgb(245, 245, 245), Color.White)
    End Sub

    Private Sub ConfigureButtons()
        btnProcess.Text = "💰 Apply Refund/Discount"
        btnProcess.BackColor = Color.FromArgb(46, 204, 113)
        btnProcess.ForeColor = Color.White
        btnProcess.FlatStyle = FlatStyle.Flat
        btnProcess.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnProcess.Padding = New Padding(15, 8, 15, 8)
        btnProcess.Margin = New Padding(5)
        btnProcess.Cursor = Cursors.Hand
        btnProcess.AutoSize = True
        btnProcess.FlatAppearance.BorderSize = 0
        AddHandler btnProcess.Click, AddressOf btnProcess_Click

        btnClear.Text = "🧹 Clear Fields"
        btnClear.BackColor = Color.FromArgb(230, 126, 34)
        btnClear.ForeColor = Color.White
        btnClear.FlatStyle = FlatStyle.Flat
        btnClear.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnClear.Padding = New Padding(15, 8, 15, 8)
        btnClear.Margin = New Padding(5)
        btnClear.Cursor = Cursors.Hand
        btnClear.AutoSize = True
        btnClear.FlatAppearance.BorderSize = 0
        AddHandler btnClear.Click, AddressOf btnClear_Click
    End Sub

    Private Sub btnProcess_Click(sender As Object, e As EventArgs)
        If dgvRefunds.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a payment record to apply refund/discount.",
                            "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If Not ValidateInputs() Then
            Return
        End If

        Dim selectedRow As DataGridViewRow = dgvRefunds.SelectedRows(0)
        Dim paymentID As Object = selectedRow.Cells("PaymentID").Value
        Dim bookingID As Object = selectedRow.Cells("BookingID").Value

        Dim refundAmount As Decimal = 0
        Dim discountAmount As Decimal = 0
        Decimal.TryParse(txtRefundAmount.Text, refundAmount)
        Decimal.TryParse(txtDiscountAmount.Text, discountAmount)

        Dim query As String = "
        UPDATE payment 
        SET RefundedAmount = @RefundedAmount, DiscountAmount = @DiscountAmount, Remarks = @Remarks, ProcessedBy = @ProcessedBy 
        WHERE PaymentID = @PaymentID"

        Dim parameters As New Dictionary(Of String, Object) From {
            {"@RefundedAmount", refundAmount},
            {"@DiscountAmount", discountAmount},
            {"@Remarks", txtRemarks.Text},
            {"@ProcessedBy", SessionInfo.LoggedInUserFullName},
            {"@PaymentID", paymentID}
        }

        Try
            If ExecuteQuery(query, parameters) Then
                MessageBox.Show("Refund/Discount successfully processed! 🎉",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadRefunds()
                ClearFields()
            Else
                MessageBox.Show("Failed to process refund/discount. Please try again.",
                                "Processing Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function ValidateInputs() As Boolean
        Dim refundAmount As Decimal = 0
        Dim discountAmount As Decimal = 0

        If Not Decimal.TryParse(txtRefundAmount.Text.Trim(), refundAmount) AndAlso Not String.IsNullOrEmpty(txtRefundAmount.Text.Trim()) Then
            MessageBox.Show("Please enter a valid refund amount.",
                            "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtRefundAmount.Focus()
            Return False
        End If

        If Not Decimal.TryParse(txtDiscountAmount.Text.Trim(), discountAmount) AndAlso Not String.IsNullOrEmpty(txtDiscountAmount.Text.Trim()) Then
            MessageBox.Show("Please enter a valid discount amount.",
                            "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDiscountAmount.Focus()
            Return False
        End If

        If refundAmount = 0 AndAlso discountAmount = 0 Then
            MessageBox.Show("Please enter either a refund amount or a discount amount.",
                            "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtRefundAmount.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtRemarks.Text) Then
            MessageBox.Show("Please enter remarks explaining the reason for refund/discount.",
                            "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtRemarks.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub dgvRefunds_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 AndAlso dgvRefunds.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = dgvRefunds.Rows(e.RowIndex)

            txtPaymentID.Text = selectedRow.Cells("PaymentID").Value.ToString()
            txtBookingID.Text = selectedRow.Cells("BookingID").Value.ToString()
            txtRefundAmount.Text = selectedRow.Cells("RefundedAmount").Value.ToString()
            txtDiscountAmount.Text = selectedRow.Cells("DiscountAmount").Value.ToString()
            txtRemarks.Text = selectedRow.Cells("Remarks").Value.ToString()
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearFields()
    End Sub

    Private Sub ClearFields()
        txtPaymentID.Clear()
        txtBookingID.Clear()
        txtRefundAmount.Clear()
        txtDiscountAmount.Clear()
        txtRemarks.Clear()

        If dgvRefunds.Rows.Count > 0 Then
            dgvRefunds.Focus()
        End If
    End Sub
End Class
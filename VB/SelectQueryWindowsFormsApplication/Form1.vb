﻿Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.DataAccess.Sql
Imports DevExpress.XtraReports.UI
Imports System
Imports System.Windows.Forms
' ...

Namespace SelectQueryWindowsFormsApplication
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()

            ' Register the custom function.
            StDevOperator.Register()
        End Sub

        Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button1.Click
            ' Create a data source connection.
            Dim connectionParameters As New Access97ConnectionParameters("../../nwind.mdb", "", "")
            Dim ds As New SqlDataSource(connectionParameters)

            ' Create a query and specify its SELECT expression.
            Dim query As SelectQuery = SelectQueryFluentBuilder.AddTable("Products").SelectColumn("CategoryID").GroupBy("CategoryID").SelectExpression("StDev([Products].[UnitPrice])", "PriceDeviation").Build("Products")

            'Add the query to the data source.

            ds.Queries.Add(query)

            ' Fill the data source.
            ds.Fill()

            ' Create a new report and bind it to the data source.
            Dim report As New XtraReport()
            report.DataSource = ds
            report.DataMember = "Products"

            ' Create a report layout.
            Dim detailBand As New DetailBand()
            detailBand.Height = 50
            report.Bands.Add(detailBand)

            Dim labelCategory As New XRLabel()
            labelCategory.DataBindings.Add("Text", report.DataSource, "Products.CategoryID", "Category ID: {0}")
            labelCategory.TopF = 15
            detailBand.Controls.Add(labelCategory)

            Dim labelDeviation As New XRLabel()
            labelDeviation.DataBindings.Add("Text", report.DataSource, "Products.PriceDeviation", "Price Deviation: {0}")
            labelDeviation.TopF = 30
            labelDeviation.WidthF = 500
            detailBand.Controls.Add(labelDeviation)

            ' Publish the report.
            Dim pt As New ReportPrintTool(report)
            pt.ShowPreviewDialog()
        End Sub
    End Class
End Namespace

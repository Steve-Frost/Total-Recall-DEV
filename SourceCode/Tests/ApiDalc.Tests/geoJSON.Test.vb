﻿#Region " Imports "

Imports Newtonsoft.Json.Linq
Imports NUnit.Framework

#End Region

<TestFixture()>
Public Class geoJSON

    <TestCase()>
    Public Sub ReadData_Nothing()

        Dim mockRestClient = New Mocks.MockRestClient("FeatureCollection.json")
        Dim geoJson As JObject

        'Dim service = New ApiDalc.OpenFda(mockRestClient)
        'Dim result = service.GetDrugEventsByDrugName("Augmentin")

        Assert.AreEqual(Nothing, geoJson)

    End Sub

    <TestCase()>
    Public Sub ReadData_NotNothing()

        Dim mockRestClient = New Mocks.MockRestClient("FeatureCollection.json")
        Dim geoJson = mockRestClient.Execute(String.Empty)

        'Dim service = New ApiDalc.OpenFda(mockRestClient)
        'Dim result = service.GetDrugEventsByDrugName("Augmentin")

        Assert.AreNotEqual(Nothing, geoJson)

    End Sub

    <TestCase()>
    Public Sub ReadData_ObjectNSON()

        Dim mockRestClient = New Mocks.MockRestClient("FeatureCollection.json")
        Dim geoJson = mockRestClient.Execute(String.Empty)

        Dim jo As JObject = JObject.Parse(geoJson)
        Assert.IsInstanceOf(Of JObject)(jo)


    End Sub

    <TestCase()>
    Public Sub ReadData_currentTest()

        Dim mockRestClient = New Mocks.MockRestClient("FeatureCollection.json")
        Dim geoJson = mockRestClient.Execute(String.Empty)

        Dim jo As JObject = JObject.Parse(geoJson)
        For Each feature In jo("features")

        Next


        Assert.IsInstanceOf(Of JObject)(jo)


    End Sub


End Class

